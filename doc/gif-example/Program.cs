namespace Example
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Linq;
    using FluentArgs;

    class Program
    {
        public static void Main(string[] args)
        {
            FluentArgsBuilder.New()
                .DefaultConfigsWithAppDescription("An app to convert png images to jpeg images with a defined quality")
                .Parameter("-i", "--input")
                    .WithDescription("Input file (PNG)")
                    .IsRequired()
                .Parameter("-o", "--output")
                    .WithDescription("Output file (JPEG)")
                    .IsRequired()
                .Parameter<int>("-q", "--quality")
                    .WithDescription("The jpeg encoding quality (from 0 to 100)")
                    .WithValidation(q => q >= 0 && q <= 100)
                    .IsOptionalWithDefault(100)
                .Flag("-v", "--verbose")
                    .WithDescription("Verbosity flag: If enabled, output more runtime infos")
                .Call(verbose => quality => output => input => ConvertPngToJpeg(input, output, quality, verbose))
                .Parse(args);
        }

        private static void ConvertPngToJpeg(string inputFile, string outputFile, int quality, bool verbose)
        {
            if (verbose)
            {
                Console.WriteLine($"Convert {inputFile} to {outputFile} (quality: {quality})");
            }

            /* Application implementation */
            var img = Image.FromFile(inputFile);
            using (var encoderParameters = new EncoderParameters(1))
            {
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);
                img.Save(outputFile, ImageCodecInfo.GetImageEncoders().First(e => e.MimeType == "image/jpeg"), encoderParameters);
            }

            if (verbose)
            {
                Console.WriteLine("Conversion done");
            }
        }
    }
}
