// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Drawing.Processing.Processors.Drawing;
using SixLabors.ImageSharp.Drawing.Tests.Processing;
using SixLabors.ImageSharp.Drawing.Tests.TestUtilities;
using Xunit;

namespace SixLabors.ImageSharp.Drawing.Tests.Drawing.Paths
{
    public class DrawRectangle : BaseImageOperationsExtensionTest
    {
        private readonly Pen pen = Pens.Solid(Color.HotPink, 2);
        private RectangleF rectangle = new RectangleF(10, 10, 20, 20);

        private RectangularPolygon RectanglePolygon => new RectangularPolygon(this.rectangle);

        [Fact]
        public void CorrectlySetsPenAndPath()
        {
            this.operations.Draw(new DrawingOptions(), this.pen, this.rectangle);

            DrawPathProcessor processor = this.Verify<DrawPathProcessor>();

            Assert.NotEqual(this.shapeOptions, processor.Options.ShapeOptions);
            Assert.True(RectangularPolygonValueComparer.Equals(this.RectanglePolygon, processor.Path));
            Assert.Equal(this.pen, processor.Pen);
        }

        [Fact]
        public void CorrectlySetsPenAndPathDefaultOptions()
        {
            this.operations.Draw(this.pen, this.rectangle);

            DrawPathProcessor processor = this.Verify<DrawPathProcessor>();

            Assert.Equal(this.shapeOptions, processor.Options.ShapeOptions);
            Assert.True(RectangularPolygonValueComparer.Equals(this.RectanglePolygon, processor.Path));
            Assert.Equal(this.pen, processor.Pen);
        }

        [Fact]
        public void BrushAndThickness()
        {
            this.operations.Draw(new DrawingOptions(), this.pen.StrokeFill, 10, this.rectangle);

            DrawPathProcessor processor = this.Verify<DrawPathProcessor>();

            Assert.NotEqual(this.shapeOptions, processor.Options.ShapeOptions);
            Assert.True(RectangularPolygonValueComparer.Equals(this.RectanglePolygon, processor.Path));
            Assert.NotEqual(this.pen, processor.Pen);
            Assert.Equal(this.pen.StrokeFill, processor.Pen.StrokeFill);
            Assert.Equal(10, processor.Pen.StrokeWidth);
        }

        [Fact]
        public void BrushAndThicknessDefaultOptions()
        {
            this.operations.Draw(this.pen.StrokeFill, 10, this.rectangle);

            DrawPathProcessor processor = this.Verify<DrawPathProcessor>();

            Assert.Equal(this.shapeOptions, processor.Options.ShapeOptions);
            Assert.True(RectangularPolygonValueComparer.Equals(this.RectanglePolygon, processor.Path));
            Assert.NotEqual(this.pen, processor.Pen);
            Assert.Equal(this.pen.StrokeFill, processor.Pen.StrokeFill);
            Assert.Equal(10, processor.Pen.StrokeWidth);
        }

        [Fact]
        public void ColorAndThickness()
        {
            this.operations.Draw(new DrawingOptions(), Color.Red, 10, this.rectangle);

            DrawPathProcessor processor = this.Verify<DrawPathProcessor>();

            Assert.NotEqual(this.shapeOptions, processor.Options.ShapeOptions);
            Assert.True(RectangularPolygonValueComparer.Equals(this.RectanglePolygon, processor.Path));
            Assert.NotEqual(this.pen, processor.Pen);
            SolidBrush brush = Assert.IsType<SolidBrush>(processor.Pen.StrokeFill);
            Assert.Equal(Color.Red, brush.Color);
            Assert.Equal(10, processor.Pen.StrokeWidth);
        }

        [Fact]
        public void ColorAndThicknessDefaultOptions()
        {
            this.operations.Draw(Color.Red, 10, this.rectangle);

            DrawPathProcessor processor = this.Verify<DrawPathProcessor>();

            Assert.Equal(this.shapeOptions, processor.Options.ShapeOptions);
            Assert.True(RectangularPolygonValueComparer.Equals(this.RectanglePolygon, processor.Path));
            Assert.NotEqual(this.pen, processor.Pen);
            SolidBrush brush = Assert.IsType<SolidBrush>(processor.Pen.StrokeFill);
            Assert.Equal(Color.Red, brush.Color);
            Assert.Equal(10, processor.Pen.StrokeWidth);
        }

        [Fact]
        public void JointAndEndCapStyle()
        {
            this.operations.Draw(new DrawingOptions(), this.pen.StrokeFill, 10, this.rectangle);

            DrawPathProcessor processor = this.Verify<DrawPathProcessor>();

            Assert.NotEqual(this.shapeOptions, processor.Options.ShapeOptions);
            Assert.True(RectangularPolygonValueComparer.Equals(this.RectanglePolygon, processor.Path));
            Assert.NotEqual(this.pen, processor.Pen);
            Assert.Equal(this.pen.JointStyle, processor.Pen.JointStyle);
            Assert.Equal(this.pen.EndCapStyle, processor.Pen.EndCapStyle);
        }
    }
}
