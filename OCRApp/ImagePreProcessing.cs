using ImageMagick;
using OCRApp.Autotrim;
using OCRApp.TextCleaner;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mime;
using AForge.Imaging;
using AForge.Imaging.Filters;
using System.Drawing.Imaging;

namespace OCRApp
{
    //this class used to pre image processing 1 DPI 2 Binarisation 3 Noise 4 Orientation / Skew  5 Borders  6 Segmentation
    class ImagePreProcessing
    {
        public static void EnhanceImageQuality(string ImageDirectory)
        {

            // do image Enhance then call Do OCR
            // convert - density 600 input.pdf output.tif;
            //System.Drawing.Image SelectedImage = new System.Drawing.Image();
            DirectoryInfo EnhancedImageDir = new DirectoryInfo(@"C:\OCR\EnhancedImage\");
            Bitmap orgimg = (Bitmap)Bitmap.FromFile(ImageDirectory);
            Bitmap bitmap = (Bitmap)Bitmap.FromFile(ImageDirectory);
            Bitmap image2 = Grayscale.CommonAlgorithms.BT709.Apply(bitmap);
            
            
            Rectangle rec = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bitmapdata = bitmap.LockBits(rec, ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            // create instance of skew checker
            DocumentSkewChecker skewChecker = new DocumentSkewChecker();
            // get documents skew angle
            double angle = skewChecker.GetSkewAngle(bitmapdata);
            // create rotation filter
            RotateBilinear rotationFilter = new RotateBilinear(-angle);
            rotationFilter.FillColor = Color.White;
            // rotate image applying the filter
            bitmap.UnlockBits(bitmapdata);

            Bitmap rotatedImage = rotationFilter.Apply(orgimg);

            rotatedImage.Save(ImageDirectory + ".tif");
           
            using (IMagickImage image = new MagickImage(ImageDirectory))
            {
                TextCleanerScript Cleaner = new TextCleanerScript();

               // Cleaner.FilterSize = 12; // filter to clean up background;
               // Cleaner.FilterOffset = (Percentage)3; //filter in percent to reduce noise
               // Cleaner.Saturation = (Percentage)200;  // high value to Saturation
               //// Cleaner.Unrotate = true;
               // var newImage1 = Cleaner.Execute(image);
               // //newImage1.CannyEdge(); selcet the inner objects in the image 
                
               // newImage1.Density = new PointD(600, 600);
               // newImage1.Write(EnhancedImageDir + Path.GetFileName(ImageDirectory)
               //     + ".tif");

                AutotrimScript WhiteB = new AutotrimScript();
               // WhiteB.InnerTrim = true;
                var a = WhiteB.GetLargestAreaP(image);
                
                WhiteboardScript AoutoTrim = new WhiteboardScript();
                //AoutoTrim.SetCoordinates(new PointD(01, 53), new PointD(313, 31),
                //new PointD(331, 218), new PointD(218, 200));
                //AoutoTrim.Enhance = WhiteboardEnhancements.Both;
                //AoutoTrim.FilterSize = 25; // filter to clean up background;
                //AoutoTrim.FilterOffset = (Percentage)3; //filter in percent to reduce noise
                //AoutoTrim.Saturation = (Percentage)200;  // high value to Saturation
                //AoutoTrim.SetCoordinates(new PointD(a[0].X, a[0].Y), new PointD(a[1].X, a[1].Y),
                //new PointD(a[2].X, a[2].Y), new PointD(a[3].X, a[3].Y));
                var new2mage2 = AoutoTrim.Execute(image);
              //  new2mage2.Deskew((Percentage)2);
                AoutoTrim.SetPointDs(new PointD(13, 3), new PointD(342, 6),
                new PointD(331, 467), new PointD(38, 482));
                AoutoTrim.Enhance = WhiteboardEnhancements.Both;
             
               
             //  new2mage2.Density= new PointD(300, 300);
				new2mage2= WhiteB.Execute(new2mage2);
                new2mage2= Cleaner.Execute(new2mage2);
                
                new2mage2.Density = new Density(600, 600);
                new2mage2.Write(EnhancedImageDir + Path.GetFileName(ImageDirectory)
                    + "2" + ".tif");
                string dir12 = (EnhancedImageDir + Path.GetFileName(ImageDirectory)
                    + "2" + ".tif");
                Bitmap imageTobeDeske = new Bitmap(dir12);
                gmseDeskew deskew = new gmseDeskew();
                deskew.New(imageTobeDeske);
                double angel=   deskew.GetSkewAngle();
                if (angel > 0) angel = angel * -1;



                WhiteB.BorderColorLocation = new PointD(10, 10);
                
               
                //AoutoTrim.SetCoordinates(ad);
                
                //WhiteB.ColorFuzz = (Percentage)20;
                //var new2mage3 = WhiteB.Execute(image);
                //new2mage3 = Cleaner.Execute(new2mage3);

                //new2mage3.Write(EnhancedImageDir + Path.GetFileName(ImageDirectory)
                //    + "3" + ".tif");
            }

        }
        private static System.Drawing.Image EnchanceDPI(System.Drawing.Image DPIImage)
        {
            //Bitmap currentImage = new Bitmap(ImageDirectory);
            //Bitmap cleanImage = CleanText.Resize(currentImage, currentImage.Width, currentImage.Height);
            //using (MagickImage img = new MagickImage(cleanImage))
            //{
            //    img.Write("ManualImage.tif");
            //}


            //  QuantizeSettings qs = new QuantizeSettings();

            // qs.ColorSpace = ColorSpace.Gray;
            // image.Quantize(qs);
            //image.ColorType = ColorType.Grayscale;
            //image.ContrastStretch(new Percentage(0), new Percentage(0));

            //QuantizeSettings qs2 = new QuantizeSettings();
            //image.Negate();
            //qs2.ColorSpace = ColorSpace.GRAY;
            //image.Quantize(qs2);

            //image.ColorType = ColorType.Grayscale;
            //image.AdaptiveThreshold(30, 30, 10);
            //  image.BackgroundColor = Color.White;
            //image.AdaptiveThreshold(15, 15, 10);
            //image.ContrastStretch(0, 0);
            //ImageOptimizer optimizer = new ImageOptimizer();
            //optimizer.LosslessCompress(image.FileName);

            // image.Negate();

            MagickNET.Initialize(@"C:\OCR\File\");
            // Read first frame of gif image
            using (IMagickImage image = new MagickImage("Snakeware.gif"))
            {
                image.Density = new Density(600, 600);
                image.AutoLevel();
                image.Negate();
                image.AdaptiveThreshold(30, 30, 10);
                image.Negate();
                image.Write("Snakeware.jpg");
            }

            // Write to stream
            MagickReadSettings settings = new MagickReadSettings();
            // Tells the xc: reader the image to create should be 800x600
            settings.Width = 800;
            settings.Height = 600;
            //settings.Density = 600;
            using (MemoryStream memStream = new MemoryStream())
            {
                // Create image that is completely purple and 800x600
                using (IMagickImage image = new MagickImage("xc:purple", settings))
                {
                    // Sets the output format to png
                    image.Format = MagickFormat.Png;
                    // Write the image to the memorystream
                    image.Write(memStream);
                }
            }
            return DPIImage;
        }
        private static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
           // g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (System.Drawing.Image)b;
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    