using Ratran.Segments;

namespace Ratran;

public class Layer<TPixel> where TPixel : unmanaged, IPixel<TPixel>
{
    public required List<Segment<TPixel>> Segments = new();
}