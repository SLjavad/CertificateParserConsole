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
        
        public static string GetLastFolderName(string directory)
        {
            var directories = Directory.GetDirectories(directory);
            if (directories.Length == 0)
            {
                return "0";
            }
            string lastName = string.Empty;
            int tmp = 0;
            for (int i = 0; i < directories.Length; i++)
            {
                string name = Path.GetDirectoryName(directories[i]);
            }
            return null;
        }

        public static (string,string) CheckAndCreatePath()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\CSRs";
            new FileInfo(path).Directory.Create();
            var lastItem = GetLastFolderName(path);

            if (lastItem == "0")
            {
                path += "\\CSR_1";
            }
            else
            {
                path += $"\\CSR_{(int.Parse(lastItem) + 1)}";
            }
            new FileInfo(path).Directory.Create();

            return (path , (int.Parse(lastItem) + 1).ToString());
        }
    }
}
