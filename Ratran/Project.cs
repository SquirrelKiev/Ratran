using System.Reflection.Emit;

namespace Ratran;

// TODO: should handle differently timed layers better. currently every layer must be the same length, or weird things happen.
public class Project<TPixel> where TPixel : unmanaged, IPixel<TPixel>
{
    public required int Width;
    public required int Height;

    public required List<Layer<TPixel>> Layers = new();

    public IEnumerable<GifFrame<TPixel>> ExportAndFlattenToGifFrames(int fps)
    {
        var times = new SortedSet<int>();
        var renderedLayers = new List<List<GifFrame<TPixel>>>();

        foreach (var layer in Layers)
        {
            var frames = new List<GifFrame<TPixel>>();
            var time = 0;
            foreach (var segmentFrame in layer.Segments.Select(segment => segment.ExportForGif(fps)).SelectMany(segmentFrames => segmentFrames))
            {
                times.Add(time);
                time += segmentFrame.FrameDuration;

                frames.Add(segmentFrame);
            }

            renderedLayers.Add(frames);
        }

        var finalRender = new List<GifFrame<TPixel>>();
        var timeArray = times.ToArray();

        for (int i = 0; i < timeArray.Length; i++)
        {
            var time = timeArray[i];
            var tempFrame = new Image<TPixel>(Width, Height);

            var nextTime = (i < timeArray.Length - 1) ? timeArray[i + 1] : time + FindLastFrameDuration(renderedLayers);

            foreach (var layerFrames in renderedLayers)
            {
                var frame = FindFrameAtTime(layerFrames, time);
                if (frame != null)
                {
                    tempFrame.Mutate(x => x.DrawImage(frame.Frame, 1.0f));
                }
            }

            finalRender.Add(new GifFrame<TPixel>
            {
                Frame = tempFrame,
                FrameDuration = nextTime - time // Set the duration to the gap until the next frame
            });
        }

        return finalRender;
    }

    private GifFrame<TPixel>? FindFrameAtTime(List<GifFrame<TPixel>> layer, int time)
    {
        int currentTime = 0;
        foreach (var frame in layer)
        {
            if (time >= currentTime && time < currentTime + frame.FrameDuration)
                return frame;
            currentTime += frame.FrameDuration;
        }
        return null;
    }

    private int FindLastFrameDuration(List<List<GifFrame<TPixel>>> renderedLayers)
    {
        // Assuming that the last frame should stay on screen for the duration of the longest layer's last frame
        int longestDuration = int.MaxValue;
        foreach (var layer in renderedLayers)
        {
            if (layer.Count > 0)
            {
                var lastFrameDuration = layer.Last().FrameDuration;
                longestDuration = Math.Min(longestDuration, lastFrameDuration);
            }
        }
        return longestDuration;
    }

}