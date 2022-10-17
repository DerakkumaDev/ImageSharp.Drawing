// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

using Xunit;

// ReSharper disable InconsistentNaming
namespace SixLabors.ImageSharp.Drawing.Tests.Drawing
{
    [GroupOutput("Drawing")]
    public class FillImageBrushTests
    {
        [Fact]
        public void DoesNotDisposeImage()
        {
            using (var src = new Image<Rgba32>(5, 5))
            {
                var brush = new ImageBrush(src);
                using (var dest = new Image<Rgba32>(10, 10))
                {
                    dest.Mutate(c => c.Fill(brush, new Rectangle(0, 0, 10, 10)));
                    dest.Mutate(c => c.Fill(brush, new Rectangle(0, 0, 10, 10)));
                }
            }
        }

        [Theory]
        [WithTestPatternImage(200, 200, PixelTypes.Rgba32 | PixelTypes.Bgra32)]
        public void UseBrushOfDifferentPixelType<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            byte[] data = TestFile.Create(TestImages.Png.Ducky).Bytes;
            using Image<TPixel> background = provider.GetImage();
            using Image overlay = provider.PixelType == PixelTypes.Rgba32
                                       ? Image.Load<Bgra32>(data)
                                       : Image.Load<Rgba32>(data);

            var brush = new ImageBrush(overlay);
            background.Mutate(c => c.Fill(brush));

            background.DebugSave(provider, appendSourceFileOrDescription: false);
            background.CompareToReferenceOutput(provider, appendSourceFileOrDescription: false);
        }

        [Theory]
        [WithTestPatternImage(200, 200, PixelTypes.Rgba32)]
        public void CanDrawLandscapeImage<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            byte[] data = TestFile.Create(TestImages.Png.Ducky).Bytes;
            using Image<TPixel> background = provider.GetImage();
            using Image overlay = Image.Load<Rgba32>(data);

            overlay.Mutate(c => c.Crop(new Rectangle(0, 0, 125, 90)));

            var brush = new ImageBrush(overlay);
            background.Mutate(c => c.Fill(brush));

            background.DebugSave(provider, appendSourceFileOrDescription: false);
            background.CompareToReferenceOutput(provider, appendSourceFileOrDescription: false);
        }

        [Theory]
        [WithTestPatternImage(200, 200, PixelTypes.Rgba32)]
        public void CanDrawPortraitImage<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            byte[] data = TestFile.Create(TestImages.Png.Ducky).Bytes;
            using Image<TPixel> background = provider.GetImage();
            using Image overlay = Image.Load<Rgba32>(data);

            overlay.Mutate(c => c.Crop(new Rectangle(0, 0, 90, 125)));

            var brush = new ImageBrush(overlay);
            background.Mutate(c => c.Fill(brush));

            background.DebugSave(provider, appendSourceFileOrDescription: false);
            background.CompareToReferenceOutput(provider, appendSourceFileOrDescription: false);
        }
    }
}
