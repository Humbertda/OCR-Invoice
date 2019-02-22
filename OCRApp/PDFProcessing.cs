using AForge.Imaging.Filters;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRApp
{
    class PDFProcessing
    {
        public static void ConvertPDF (string PDFDirectory)
        {

            var ext = new List<string> { ".pdf" };
            MagickReadSettings settings = new MagickReadSettings();
            // Settings the density to 300 dpi will create an image with a better quality
            settings.Density = new Density(300, 300);
           
             DirectoryInfo dr2 = new DirectoryInfo(@"C:\OCR\EnhancedImage\");
            var fileEntries = Directory.GetFiles(PDFDirectory, "*.*", SearchOption.AllDirectories)
                 .Where(s => ext.Any(e => s.EndsWith(e)));
            foreach (string fileName in fileEntries)
            {
                Console.WriteLine("PDF  file '{0}'.", fileName);
                using (MagickImageCollection images = new MagickImageCollection())
                {
                    // Add all the pages of the pdf file to the collection
                    images.Read(fileName, settings);
                    images.Write(dr2 + Path.GetFileName(fileName)
                        + ".jpg");


                }

            }
           

           
           

            

        
        }
        public void bradley(ref Bitmap tmp)
        {
            BradleyLocalThresholding filter = new BradleyLocalThresholding();
           // filter.AssertCanApply(tmp.PixelFormat);
            filter.ApplyInPlace(tmp);
        }
    }
}
