// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Metadata;
using SixLabors.ImageSharp.PixelFormats;

namespace SixLabors.ImageSharp.Drawing.Tests.TestUtilities.ReferenceCodecs
{
    public class SystemDrawingReferenceDecoder : IImageDecoder, IImageInfoDetector
    {
        public static SystemDrawingReferenceDecoder Instance { get; } = new SystemDrawingReferenceDecoder();

        public Image<TPixel> Decode<TPixel>(Configuration configuration, Stream stream, CancellationToken cancellationToken)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            using (var sourceBitmap = new System.Drawing.Bitmap(stream))
            {
                if (sourceBitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                {
                    return SystemDrawingBridge.From32bppArgbSystemDrawingBitmap<TPixel>(sourceBitmap);
                }

                using (var convertedBitmap = new System.Drawing.Bitmap(
                    sourceBitmap.Width,
                    sourceBitmap.Height,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                {
                    using (var g = System.Drawing.Graphics.FromImage(convertedBitmap))
                    {
                        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                        g.DrawImage(sourceBitmap, 0, 0, sourceBitmap.Width, sourceBitmap.Height);
                    }

                    return SystemDrawingBridge.From32bppArgbSystemDrawingBitmap<TPixel>(convertedBitmap);
                }
            }
        }

        public IImageInfo Identify(Configuration configuration, Stream stream, CancellationToken cancellationToken)
        {
            using (var sourceBitmap = new System.Drawing.Bitmap(stream))
            {
                var pixelType = new PixelTypeInfo(System.Drawing.Image.GetPixelFormatSize(sourceBitmap.PixelFormat));
                return new ImageInfo(pixelType, sourceBitmap.Width, sourceBitmap.Height, new ImageMetadata());
            }
        }

        public Task<IImageInfo> IdentifyAsync(Configuration configuration, Stream stream, CancellationToken cancellationToken)
            => throw new System.NotImplementedException();

        public Image Decode(Configuration configuration, Stream stream, CancellationToken cancellationToken)
            => this.Decode<Rgba32>(configuration, stream, cancellationToken);

        public Task<Image<TPixel>> DecodeAsync<TPixel>(Configuration configuration, Stream stream, CancellationToken cancellationToken)
            where TPixel : unmanaged, IPixel<TPixel>
            => throw new System.NotImplementedException();

        public Task<Image> DecodeAsync(Configuration configuration, Stream stream, CancellationToken cancellationToken)
            => throw new System.NotImplementedException();
    }
}
