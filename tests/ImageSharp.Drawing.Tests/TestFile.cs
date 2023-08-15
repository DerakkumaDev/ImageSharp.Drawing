// Copyright (c) Six Labors.
// Licensed under the Six Labors Split License.

using System.Collections.Concurrent;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using IOPath = System.IO.Path;

namespace SixLabors.ImageSharp.Drawing.Tests;

/// <summary>
/// A test image file.
/// </summary>
public class TestFile
{
    /// <summary>
    /// The test file cache.
    /// </summary>
    private static readonly ConcurrentDictionary<string, TestFile> Cache = new ConcurrentDictionary<string, TestFile>();

    /// <summary>
    /// The "Formats" directory, as lazy value
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private static readonly Lazy<string> LazyInputImagesDirectory = new Lazy<string>(() => TestEnvironment.InputImagesDirectoryFullPath);

    /// <summary>
    /// The image (lazy initialized value)
    /// </summary>
    private Image<Rgba32> image;

    /// <summary>
    /// The image bytes
    /// </summary>
    private byte[] bytes;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestFile"/> class.
    /// </summary>
    /// <param name="file">The file.</param>
    private TestFile(string file)
        => this.FullPath = file;

    /// <summary>
    /// Gets the image bytes.
    /// </summary>
    public byte[] Bytes => this.bytes ??= File.ReadAllBytes(this.FullPath);

    /// <summary>
    /// Gets the full path to file.
    /// </summary>
    public string FullPath { get; }

    /// <summary>
    /// Gets the file name.
    /// </summary>
    public string FileName => IOPath.GetFileName(this.FullPath);

    /// <summary>
    /// Gets the file name without extension.
    /// </summary>
    public string FileNameWithoutExtension => IOPath.GetFileNameWithoutExtension(this.FullPath);

    /// <summary>
    /// Gets the image with lazy initialization.
    /// </summary>
    private Image<Rgba32> Image => this.image ??= ImageSharp.Image.Load<Rgba32>(this.Bytes);

    /// <summary>
    /// Gets the input image directory.
    /// </summary>
    private static string InputImagesDirectory => LazyInputImagesDirectory.Value;

    /// <summary>
    /// Gets the full qualified path to the input test file.
    /// </summary>
    /// <param name="file">The file path.</param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public static string GetInputFileFullPath(string file)
        => IOPath.Combine(InputImagesDirectory, file).Replace('\\', IOPath.DirectorySeparatorChar);

    /// <summary>
    /// Creates a new test file or returns one from the cache.
    /// </summary>
    /// <param name="file">The file path.</param>
    /// <returns>
    /// The <see cref="TestFile"/>.
    /// </returns>
    public static TestFile Create(string file)
        => Cache.GetOrAdd(file, (string fileName) => new TestFile(GetInputFileFullPath(file)));

    /// <summary>
    /// Gets the file name.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public string GetFileName(object value)
        => $"{this.FileNameWithoutExtension}-{value}{IOPath.GetExtension(this.FullPath)}";

    /// <summary>
    /// Gets the file name without extension.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public string GetFileNameWithoutExtension(object value)
        => this.FileNameWithoutExtension + "-" + value;

    /// <summary>
    /// Creates a new image.
    /// </summary>
    /// <returns>
    /// The <see cref="ImageSharp.Image"/>.
    /// </returns>
    public Image<Rgba32> CreateRgba32Image() => this.Image.Clone();

    /// <summary>
    /// Creates a new image.
    /// </summary>
    /// <returns>
    /// The <see cref="ImageSharp.Image"/>.
    /// </returns>
    public Image<Rgba32> CreateRgba32Image(IImageDecoder decoder)
        => ImageSharp.Image.Load<Rgba32>(this.Image.GetConfiguration(), this.Bytes, decoder);
}
