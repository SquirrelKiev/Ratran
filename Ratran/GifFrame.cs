namespace Ratran;

public class GifFrame<TPixel> where TPixel : unmanaged, IPixel<TPixel>
{
    /// <summary>
    /// In 1/100ths of seconds.
    /// </summary>
    public required int FrameDuration;

    public required Image<TPixel> Frame;
}