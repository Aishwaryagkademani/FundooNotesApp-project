using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Models
{
    public class FileHelper
    {
            private static readonly string BaseDirectory = @"C:\Users\HP\Pictures\Screenshots"; // Replace with your actual base directory

            public static string GetFilePath(string fileName)
            {
                // Combine the base directory and the provided file name
                return Path.Combine(BaseDirectory, fileName);
            }
        
    }
}
