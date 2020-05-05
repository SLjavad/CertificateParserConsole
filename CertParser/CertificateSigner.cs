using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace CertParser
{
    public class CertificateSigner
    {
        private static byte[] Base64ToBinary(string path)
        {
            var datastr = File.ReadAllText(path);
            var strsplit = datastr.Split(Environment.NewLine);
            string base64 = string.Empty;
            for (int i = 1; i < strsplit.Length; i++)
            {
                if (strsplit[i].Contains("-----END"))
                {
                    break;
                }
                base64 += strsplit[i];
            }

            return Convert.FromBase64String(base64);
        }

        public static void SignCSR(string csrPath , string myCertPath , string myCertKeyPath)
        {

            var csrFilename = new FileInfo(csrPath).Name;
            csrFilename = csrFilename.Substring(0, csrFilename.LastIndexOf('.'));
            var certFileName = new FileInfo(myCertPath).Name;
            certFileName = certFileName.Substring(0, certFileName.LastIndexOf('.'));

            var path = FileHandler.CheckAndCreatePath("SignedCerts", $"SignedCert_{csrFilename}_{certFileName}");

            string signedCertPath = path.Item1 + $@"\{csrFilename}_{certFileName}.crt";

            RunCmd($"openssl x509 -req -in {csrPath} -CA {myCertPath} -CAkey {myCertKeyPath} -CAcreateserial -out {signedCertPath}");
            Console.WriteLine();
            Console.WriteLine("Created !!!");
        }


        private static void RunCmd(string command)
        {
            Process cmd = new Process();
            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.Arguments = $"/C {command}";
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                command = command.Replace("\"", "\\\"");
                cmd.StartInfo.FileName = "/bin/bash";
                cmd.StartInfo.Arguments = $"-c \"{command}\"";
            }
            
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.CreateNoWindow = true;
            
            cmd.Start();

            Console.WriteLine(cmd.StandardOutput.ReadToEnd());

            cmd.Close();
        }
    }
}
