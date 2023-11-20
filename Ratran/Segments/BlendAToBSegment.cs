namespace Ratran.Segments;

public class BlendAToBSegment<TPixel> : Segment<TPixel> where TPixel : unmanaged, IPixel<TPixel>
{
    public required Image<TPixel> StartImage;
    public required Image<TPixel> EndImage;

    public override float Duration { get; set; }
    public override IEnumerable<GifFrame<TPixel>> ExportForGif(int fps)
    {
        return Export(fps).Select(frame => new GifFrame<TPixel>
        {
            Frame = frame,
            FrameDuration = (int)(1f / fps * 100f)
        });
    }

    public IEnumerable<Image<TPixel>> Export(int fps)
    {
        var frameCount = Duration * fps;

        for (int i = 0; i < frameCount; i++)
        {
            var image1Clone = StartImage.Clone();

            var t = i / frameCount;
            image1Clone.Mutate(x => x.Transition(EndImage, t));

            yield return image1Clone;
        }
    }
}