using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CertParser
{
    public class FileHandler
    {
        public static List<byte[]> GetExtraCertRawData(string directory)
        {
            var files = Directory.GetFiles(directory);
            List<byte[]> certList = new List<byte[]>();
            for (int i = 0; i < files.Length; i++)
            {
                certList.Add(File.ReadAllBytes(files[i]));
            }
            return certList;
        }
    }
}
