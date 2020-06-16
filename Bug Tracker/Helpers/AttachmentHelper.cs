using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Bug_Tracker.Helpers
{
    public class AttachmentHelper
    {
        public static string ShowIcon(string filePath)
        {
            string iconPath;
            switch (Path.GetExtension(filePath))

            {
                case ".pdf":
                    iconPath = "/Icons/pdf.png";
                    return iconPath;
                case ".jpg":
                    iconPath = "/Icons/jpg.png";
                    return iconPath;
                case ".doc":
                    iconPath = "/Icons/doc.png";
                    return iconPath;
                case ".png":
                    iconPath = "/Icons/png.png";
                    return iconPath;
            }
            return filePath;
        }
    }
}