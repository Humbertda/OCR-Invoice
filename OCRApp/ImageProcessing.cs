using OCRApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRApp
{
    class ImageProcessing
    {
        public static void GetIMagesOnly (string FileDirectory)
        {
            var ext = new List<string> { ".jpg", ".gif", ".png","tif" };
            
            var fileEntries = Directory.GetFiles(FileDirectory, "*.*", SearchOption.AllDirectories)
                 .Where(s => ext.Any(e => s.EndsWith(e)));
            foreach (string fileName in fileEntries)
            { 
                Console.WriteLine("Image  file '{0}'.", fileName);
               
                //Console.WriteLine(Path.GetFileName(fileName));// used to get only file name
               // PerformingOCR.DoOCR1(fileName);
                ImagePreProcessing.EnhanceImageQuality(fileName);
                // Console.ReadLine();
            }
             fileEntries = Directory.GetFiles(@"C:\OCR\EnhancedImage", "*.*", SearchOption.AllDirectories)
                  .Where(s => ext.Any(e => s.EndsWith(e)));
            var InvoiceList = new List<Invoice>();

            foreach (string fileName in fileEntries)
            {
                Console.WriteLine("Image  file '{0}'.", fileName);

                
                var pageItem = PerformingOCR.DoOCR1(fileName);
                Identify removeSpace = new Identify();
                pageItem = removeSpace.RemoveSpace(pageItem);
                Invoice ck = PopInvoice.poplateInvoice(pageItem, fileName);
                InvoiceList.Add(ck);
                

                 
               
            }
            
            List<String> lines = new List<String>();
            foreach (Invoice invoice in InvoiceList)
            {
                lines.Add("Invocie File Name" + " " + invoice.InvoiceID);
                
                lines.Add("Invoice Date" + "     " + invoice.InvoiceDate);
                lines.Add("VendorName" + "     " + invoice.VendorName);
                lines.Add("     " +"QTY" + "        " + "  " + "Amount " +  "        "+ " " + "Item" + "     " );
                foreach (Lineitem lineitem in invoice.Lineitems) 
                {   
                    lines.Add( "     " + lineitem.ItemQty  + "        "+ "     " + lineitem.ItemAmount + "         " + lineitem.ItemName);
                }
                lines.Add("Total Amount" + "     " + invoice.TotalAmount);
                
                lines.Add("****************************************************************************");
            }
            
            // WriteAllLines creates a file, writes a collection of strings to the file,
            // and then closes the file.  You do NOT need to call Flush() or Close().
            System.IO.File.WriteAllLines(@"C:\OCR\EnhancedImage\OcrResult.txt", lines);
            Console.ReadLine();
        } 
    }
}
