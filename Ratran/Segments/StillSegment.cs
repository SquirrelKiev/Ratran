namespace Ratran.Segments;

public class StillSegment<TPixel> : Segment<TPixel> where TPixel : unmanaged, IPixel<TPixel>
{
    public required Image<TPixel> Image;

    public override float Duration { get; set; }
    public override IEnumerable<GifFrame<TPixel>> ExportForGif(int fps)
    {
        var frame = new GifFrame<TPixel>()
        {
            Frame = Image,
            FrameDuration = (int)(Duration * 100f)
        };

        return new[] { frame };
    }
}