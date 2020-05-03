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
        
        public static int GetLastFolderNameIndex(string directory)
        {
            var directories = Directory.GetDirectories(directory);
            if (directories.Length == 0)
            {
                return 0;
            }

            int tmp = 0;
            for (int i = 0; i < directories.Length; i++)
            {
                string name = new DirectoryInfo(directories[i]).Name;
                var splitted = name.Split("_");
                int index = int.Parse(splitted[splitted.Length - 1]);
                if (index > tmp)
                {
                    tmp = index;
                }
            }
            return tmp;
        }

        public static (string,string) CheckAndCreatePath(string baseFolderName , string subfolderName)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + $@"\{baseFolderName}";
            new DirectoryInfo(path).Create();
            var lastItem = GetLastFolderNameIndex(path);

            if (lastItem == 0)
            {
                path += $@"\{subfolderName}_1";
            }
            else
            {
                path += $@"\{subfolderName}_{(lastItem + 1)}";
            }
            new DirectoryInfo(path).Create();
            //new FileInfo(path).Directory.Create();

            return (path , (lastItem + 1).ToString());
        }
    }
}
