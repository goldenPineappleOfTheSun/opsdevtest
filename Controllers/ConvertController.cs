using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PdfiumViewer;
using PdfToImage.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PdfToImage.Controllers
{
    public class ConvertController : Controller
    {
        public string Index()
        {
            string workingDirectory = Environment.CurrentDirectory;
            return workingDirectory;
            //return LoadPdf(FileToByteArray(Directory.GetParent(workingDirectory).FullName + "/temp.pdf"));
        }
        
        static public byte[] FileToByteArray(string fileName)
        {
            byte[] buff = null;
            FileStream fs = new FileStream(fileName,
                                           FileMode.Open,
                                           FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(fileName).Length;
            buff = br.ReadBytes((int)numBytes);
            return buff;
        }

        static public string LoadPdf(byte[] pdfBytes)
        {
            string result = "";
            var stream = new MemoryStream(pdfBytes);
            using (var document = PdfDocument.Load(stream))
            {
                byte[] bytes = null;
                for (int index = 0; index < 1; index++)
                {
                    var image = document.Render(index, 300, 300, PdfRenderFlags.CorrectFromDpi);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.Save(ms, ImageFormat.Png); // gif for example
                        bytes = ms.ToArray();
                    }
                    //image.Save(@"C:\Users\golde\Documents\test.png" + index.ToString("000") + ".png", ImageFormat.Png);
                }
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] hash = md5.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("x2").ToLower());
                }
                result = sb.ToString();
            }
            return result;
        }
    }
}
