using Net.Codecrete.QrCodeGenerator;
using SkiaSharp;

namespace ServeSync.Application.Common.Helpers;

public static class QrCodeGenerator
{
    public static Stream GeneratePng(string content)
    {
        var qrCode = QrCode.EncodeText(content, QrCode.Ecc.Medium);
        return qrCode.ToPngStream(10, 4, SKColors.Black, SKColors.White);
    }
}

public static class QrCodeBitmapExtensions
{
    public static SKBitmap ToBitmap(this QrCode qrCode, int scale, int border, SKColor foreground, SKColor background)
    {
        // check arguments
        if (scale <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(scale), "Value out of range");
        }
        if (border < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(border), "Value out of range");
        }

        int size = qrCode.Size;
        int dim = (size + border * 2) * scale;

        if (dim > short.MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(scale), "Scale or border too large");
        }

        var bitmap = new SKBitmap(dim, dim, SKColorType.Rgb888x, SKAlphaType.Opaque);

        using (var canvas = new SKCanvas(bitmap))
        {
            using (var paint = new SKPaint { Color = background })
            {
                canvas.DrawRect(0, 0, dim, dim, paint);
            }

            using (var paint = new SKPaint { Color = foreground })
            {
                for (int y = 0; y < size; y++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        if (qrCode.GetModule(x, y))
                        {
                            canvas.DrawRect((x + border) * scale, (y + border) * scale, scale, scale, paint);
                        }
                    }
                }
            }
        }

        return bitmap;
    }
    
    public static Stream ToPngStream(this QrCode qrCode, int scale, int border, SKColor foreground, SKColor background)
    {
        var bitmap = qrCode.ToBitmap(scale, border, foreground, background);
        var data = bitmap.Encode(SKEncodedImageFormat.Png, 90);
        return data.AsStream();
    }
}