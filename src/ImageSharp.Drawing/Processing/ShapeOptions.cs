// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

namespace SixLabors.ImageSharp.Drawing.Processing
{
    /// <summary>
    /// Options for influencing the drawing functions.
    /// </summary>
    public class ShapeOptions : IDeepCloneable<ShapeOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShapeOptions"/> class.
        /// </summary>
        public ShapeOptions()
        {
        }

        private ShapeOptions(ShapeOptions source) => this.IntersectionRule = source.IntersectionRule;

        /// <summary>
        /// Gets or sets a value indicating whether antialiasing should be applied.
        /// <para/>
        /// Defaults to <see cref="IntersectionRule.EvenOdd"/>.
        /// </summary>
        public IntersectionRule IntersectionRule { get; set; } = IntersectionRule.EvenOdd;

        /// <inheritdoc/>
        public ShapeOptions DeepClone() => new(this);
    }
}
