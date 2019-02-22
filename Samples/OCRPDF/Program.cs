#region Copyright Syncfusion Inc. 2001-2018.
// Copyright Syncfusion Inc. 2001-2018. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using Syncfusion.OCRProcessor;
using Syncfusion.Pdf.Parsing;

namespace OCRPDF
{
    class Program
    {
        static void Main(string[] args)
        {
            //Initialize the OCR processor
            using (OCRProcessor processor = new OCRProcessor(GetFullTemplatePath(@"Tesseract Binaries/")))
            {
                //Load the PDF document 
                PdfLoadedDocument lDoc = new PdfLoadedDocument("../../Data/Region.pdf");
                //Language to process the OCR
                processor.Settings.Language = Languages.English;
                //Process OCR by providing loaded PDF document, Data dictionary and language
                processor.PerformOCR(lDoc, GetFullTemplatePath(@"Tessdata/"));
                //Save the OCR processed PDF document in the disk
                lDoc.Save("Sample.pdf");
                lDoc.Close(true);

                System.Diagnostics.Process.Start("Sample.pdf");
            }
        }       
        private static string GetFullTemplatePath(string fileName)
        {
            return @"../../../../" + fileName + "3.02/";
        }    
    }
}
