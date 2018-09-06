using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageMagick;

namespace OCRApp.TextCleaner
{
     class TextCleanerScript
    {
        private void AdaptiveBlurImage(IMagickImage image)
        {
            if (AdaptiveBlur == 0.0)
                return;

            image.AdaptiveBlur(AdaptiveBlur, 1.0);
        }

        private void CheckSettings()
        {
            if (AdaptiveBlur < 0.0)
                throw new InvalidOperationException("Invalid adaptive blur specified, value must be zero or higher.");

            if (!CropOffset.IsValid)
                throw new InvalidOperationException("Invalid crop offset specified, values must be zero or higher.");

            if (FilterSize < 0)
                throw new InvalidOperationException("Invalid filter size specified, value must be zero or higher.");

            if (Padding < 0)
                throw new InvalidOperationException("Invalid padding specified, value must be zero or higher.");

            if (Sharpen < 0.0)
                throw new InvalidOperationException("Invalid sharpen specified, value must be zero or higher.");

            if (Saturation.ToDouble() < 0.0)
                throw new InvalidOperationException("Invalid saturation specified, value must be zero or higher.");

            if (SmoothingThreshold != null)
            {
                double textSmoothingThreshold = SmoothingThreshold.Value.ToDouble();
                if (textSmoothingThreshold < 0 || textSmoothingThreshold > 100)
                    throw new InvalidOperationException("Invalid smoothing threshold specified, value must be between zero and 100.");
            }
        }

        private void ConvertToGrayscale(IMagickImage image)
        {
            if (!MakeGray)
                return;
            
            image.ColorSpace = ColorSpace.Gray;
            image.ColorType = ColorType.Grayscale;
        }

        private void CropImage(IMagickImage image)
        {
            if (!CropOffset.IsSet)
                return;

            int width = image.Width - (CropOffset.Left + CropOffset.Right);
            int height = image.Height - (CropOffset.Top + CropOffset.Bottom);

            image.Crop(new MagickGeometry(CropOffset.Left, CropOffset.Top, width, height));
        }
        private void unperspective(IMagickImage image)
        {
            
        }
        private void distor(IMagickImage image)
        {

            
          image.Distort(DistortMethod.Perspective, new double[] {0,0, 0,0 , 1300,0 ,1300,0,  0,720, 0,720 ,1300,720, 1300,770});


        }

        private void EnhanceImage(IMagickImage image)
        {
            if (Enhance == TextCleanerEnhance.Stretch)
                image.ContrastStretch((Percentage)0);
            else if (Enhance == TextCleanerEnhance.Normalize)
                image.Normalize();
        }

        private void PadImage(IMagickImage image)
        {
            if (Padding == 0)
                return;

            image.Compose = CompositeOperator.Over;
            image.BorderColor = BackgroundColor;
            image.Border(Padding);
        }
       
        private void RemoveNoise(IMagickImage image)
        {
            using (IMagickImage second = image.Clone())
            {
                second.ColorSpace = ColorSpace.Gray;
                second.Negate();
                second.AdaptiveThreshold(FilterSize, FilterSize, FilterOffset);
                second.ContrastStretch((Percentage)0);

                if (SmoothingThreshold != null)
                {
                    second.Blur(SmoothingThreshold.Value.ToDouble() / 100, Quantum.Max);
                    second.Level(SmoothingThreshold.Value, new Percentage(100));
                }

                image.Composite(second, CompositeOperator.CopyAlpha);
            }

            image.Opaque(MagickColors.Transparent, BackgroundColor);
            image.Alpha(AlphaOption.Off);
        }

        private void RotateImage(IMagickImage image)
        {
            if ((Layout == TextCleanerLayout.Portrait && image.Height < image.Width) ||
              (Layout == TextCleanerLayout.Landscape && image.Height > image.Width))
            {
                if (Rotation == TextCleanerRotation.Counterclockwise)
                    image.Rotate(-90);
                else
                    image.Rotate(90);
            }
        }

        private void SaturateImage(IMagickImage image)
        {
            if (Saturation == (Percentage)100)
                return;
            
            image.Modulate((Percentage)100, Saturation, (Percentage)100);
        }

        private void SharpenImage(IMagickImage image)
        {
            if (Sharpen == 0.0)
                return;

            image.Sharpen(0.0, Sharpen);
        }

        private void TrimImage(IMagickImage result)
        {
            if (!Trim)
                return;

            result.Trim();
            result.RePage();
        }

        private void UnrotateImage(IMagickImage image)
        {
            if (!Unrotate)
                return;

            image.BackgroundColor = BackgroundColor;
            image.Deskew((Percentage)40);
        }

        /// <summary>
        /// Creates a new instance of the TextCleanerScript class.
        /// </summary>
        public TextCleanerScript()
        {
            Reset();
        }

        /// <summary>
        ///  Applies an alternate text smoothing using and adaptive blur. Valid values are zero or
        ///  higher. The default value is zero.
        /// </summary>
        public double AdaptiveBlur
        {
            get;
            set;
        }

        /// <summary>
        ///The desired background color after it has been cleaned up. The default is white.
        /// </summary>
        public MagickColor BackgroundColor
        {
            get;
            set;
        }

        /// <summary>
        /// Image cropping offsets.
        /// </summary>
        public TextCleanerCropOffset CropOffset
        {
            get;
            set;
        }

        /// <summary>
        /// Enhance image brightness bearing cleaning, the default value is Stretch.
        /// </summary>
        public TextCleanerEnhance Enhance
        {
            get;
            set;
        }

        /// <summary>
        /// The offset threshold in percent used by the filter to eliminate noise. Valid values are
        /// zero or higher. Values too small will leave much noise and artifacts in the result. Values
        /// too large will remove too much text leaving gaps. The default value is 5.
        /// </summary>
        public Percentage FilterOffset
        {
            get;
            set;
        }

        /// <summary>
        /// The size of the filter used to clean up the background. Valid values are zero or higher.
        /// The filtersize needs to be larger than the thickness of the writing, but the smaller the
        /// better beyond this. Making it larger will increase the processing time and may lose text.
        /// The default value is 15.
        /// </summary>
        public int FilterSize
        {
            get;
            set;
        }

        /// <summary>
        /// Desired layout, the default is Portrait.
        /// </summary>
        public TextCleanerLayout Layout
        {
            get;
            set;
        }

        /// <summary>
        /// Convert document to grayscale before enhancing.
        /// </summary>
        public bool MakeGray
        {
            get;
            set;
        }

        /// <summary>
        /// Border pad amount around outer part of image. Valid values are zero or higher. The default
        /// value is 0.
        /// </summary>
        public int Padding
        {
            get;
            set;
        }

        /// <summary>
        /// Rotate image 90 degrees in direction specified if spect ratio does not match layout. The
        /// default value is no rotation.
        /// </summary>
        public TextCleanerRotation Rotation
        {
            get;
            set;
        }

        /// <summary>
        /// Color saturation. Only applicable if MakeGray is false. The default value is 200.
        /// </summary>
        public Percentage Saturation
        {
            get;
            set;
        }

        /// <summary>
        /// The amount of pixel sharpening to be applied to the resulting text. Valid values are zero
        /// or higher. If used, it should be small (suggested about 1). The default value is zero.
        /// </summary>
        public double Sharpen
        {
            get;
            set;
        }

        /// <summary>
        /// The text smoothing threshold. Valid values are between 0 and 100. Smaller values
        /// smooth/thicken the text more. Larger values thin, but can result in gaps in the text. Nominal
        /// value is in the middle at about 50. The default value is to disable smoothing.
        /// </summary>
        public Percentage? SmoothingThreshold
        {
            get;
            set;
        }

        /// <summary>
        /// Trim the border around the image. Effective only if background well-cleaned.
        /// </summary>
        public bool Trim
        {
            get;
            set;
        }

        /// <summary>
        /// Unrotate the image, the default value is false.
        /// </summary>
        public bool Unrotate
        {
            get;
            set;
        }

        /// <summary>
        /// Processses a scanned document of text to clean the text background and enhance the text.
        /// </summary>
        /// <param name="input">The image to execute the script on.</param>
        public IMagickImage Execute(IMagickImage input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            CheckSettings();

            IMagickImage output = input.Clone();
            RotateImage(output);
            
            CropImage(output);
            // distor(output);
            //unperspective need to download this script
            ConvertToGrayscale(output);
            EnhanceImage(output);
            RemoveNoise(output);
            UnrotateImage(output);
            SharpenImage(output);
            SaturateImage(output);
            AdaptiveBlurImage(output);
            TrimImage(output);
            PadImage(output);

            return output;
        }

        /// <summary>
        /// Resets the script to the default setttings.
        /// </summary>
        public void Reset()
        {
            AdaptiveBlur = 0.0;
            BackgroundColor = new MagickColor("white");
            CropOffset = new TextCleanerCropOffset();
            Enhance = TextCleanerEnhance.Stretch;
            FilterOffset = (Percentage)5;
            FilterSize = 15;
            Layout = TextCleanerLayout.Portrait;
            MakeGray = false;
            Padding = 0;
            Rotation = TextCleanerRotation.None;
            Saturation = (Percentage)200;
            Sharpen = 0.0;
            SmoothingThreshold = null;
            Trim = false;
            Unrotate = false;
        }
    }
}
