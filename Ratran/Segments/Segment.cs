namespace Ratran.Segments;

public abstract class Segment<TPixel> where TPixel : unmanaged, IPixel<TPixel>
{
    public abstract float Duration { get; set; }

    public abstract IEnumerable<GifFrame<TPixel>> ExportForGif(int fps);
}