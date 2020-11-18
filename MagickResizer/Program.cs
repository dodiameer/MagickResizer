using System;
using ImageMagick;
using CommandLine;
using System.Collections.Generic;
using System.IO;

namespace MagickResizer
{
    class Program
    {
        public class Options
        {
            [Option('i', "input", Required = true, HelpText = "Directory containing all your images")]
            public string Input { get; set; }

            [Option('o', "output", Required = true, HelpText = "Directory to output resized images")]
            public string Output { get; set; }

            [Option('w', "width", Required = true, HelpText = "Image width (px)")]
            public int Width { get; set; }

            [Option('h', "height", Required = false, HelpText = "Image Height (px) | If omitted, the width will be used")]
            public int? Height { get; set; }
        }
        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args).WithParsed(StartResize).WithNotParsed(HandleError);
        }

        public static void StartResize(Options options)
        {
            var SupportedExtensions = new List<string>
            {
                ".png",
                ".jpeg",
                ".jpg",
                ".webp",
                ".gif"
            };
            Console.WriteLine($"Width: {options.Width}");
            Console.WriteLine($"Height: {options.Height ?? options.Width}");
            Console.WriteLine($"Input: {options.Input}");
            Console.WriteLine($"Output: {options.Output}");
            var inputPath = Path.Combine(Directory.GetCurrentDirectory(), options.Input);
            var outputPath = Path.Combine(Directory.GetCurrentDirectory(), options.Output);

            if (!Directory.Exists(inputPath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input directory!");
                return;
            }

            Console.WriteLine("Processing images...");
            Directory.CreateDirectory(outputPath);

            var images = Directory.GetFiles(inputPath);

            foreach (var imageFile in images)
            {
                var imageName = Path.GetFileNameWithoutExtension(imageFile);
                var fileExt = Path.GetExtension(imageFile);
                if (SupportedExtensions.Contains(fileExt))
                {
                    Console.WriteLine($"Processing image \"{imageFile}\"");
                    var image = new MagickImage(imageFile);
                    var size = new MagickGeometry(options.Width, options.Height ?? options.Width);
                    size.IgnoreAspectRatio = false;
                    image.Resize(size);
                    //Console.WriteLine("Finished processing, Writing to disk");
                    image.Write(Path.Combine(outputPath, $"{imageName}_{size.Width}x{size.Height}{fileExt}"));
                } else
                {
                    Console.WriteLine($"Skipping file \"{imageFile}\": Unsupported extention");
                }
            }
            Console.WriteLine("Finished!");
            return;
        }

        public static void HandleError(IEnumerable<Error> errors)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("An error has occured, unable to resize your images");
        }

    }
}
