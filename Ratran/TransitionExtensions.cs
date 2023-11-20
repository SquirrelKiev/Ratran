namespace Ratran;

public static class TransitionExtensions
{
    public static void Transition(this IImageProcessingContext image1, Image image2, float t)
    {
        t = Math.Clamp(t, 0, 1);

        image1.Opacity(1 - t);
        image1.DrawImage(image2, t);
    }
}