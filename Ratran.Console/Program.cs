using Ratran;
using Ratran.Segments;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing.Processors.Quantization;

const int width = 319;
const int height = 319;

var frame1 = Image.Load<Rgba32>(@"C:\Users\SquirrelKiev\Downloads\pfp-attempts\maomao3.png");
var frame2 = Image.Load<Rgba32>(@"C:\Users\SquirrelKiev\Downloads\pfp-attempts\maomao4.png");

var project = new Project<Rgba32>
{
    Width = width,
    Height = height,
    Layers = new()
    {
        new Layer<Rgba32>()
        {
            Segments = new()
            {
                new StillSegment<Rgba32>()
                {
                    Duration = 1,
                    Image = frame1
                },
                new BlendAToBSegment<Rgba32>()
                {
                    Duration = 1,
                    StartImage = frame1,
                    EndImage = frame2
                },
                new StillSegment<Rgba32>()
                {
                    Duration = 10,
                    Image = frame2
                },
                new BlendAToBSegment<Rgba32>()
                {
                    Duration = 1,
                    StartImage = frame2,
                    EndImage = frame1
                },
                new StillSegment<Rgba32>()
                {
                    Duration = 1,
                    Image = frame1
                },
            }
        },
    }
};

project.Layers.Reverse();

var image = new Image<Rgba32>(width, height);
image.Metadata.GetGifMetadata().RepeatCount = 0;

var frames = project.ExportAndFlattenToGifFrames(24);

var i = 0;
foreach (var frame in frames)
{
    frame.Frame.Frames.RootFrame.Metadata.GetGifMetadata().FrameDelay = frame.FrameDuration;

    image.Frames.AddFrame(frame.Frame.Frames.RootFrame);

    Console.WriteLine(i);
    i++;
}

image.Frames.RemoveFrame(0);

var gifEncoder = new GifEncoder
{
    PixelSamplingStrategy = new ExtensivePixelSamplingStrategy(),
    Quantizer = new WuQuantizer(),
    ColorTableMode = GifColorTableMode.Local
};

image.SaveAsGif(@"C:\Users\SquirrelKiev\Downloads\pfp-attempts\blend.gif", gifEncoder);