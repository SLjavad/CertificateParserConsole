using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CertParser
{
    class Program
    {
        static byte[] rawCert;
        static void Main(string[] args)
        {

            Console.WriteLine("Welcome To Certificate Parser V1 :D");
            Console.WriteLine("==========================================");
            Console.WriteLine("Now Please Enter Address of Certificate you want to parse:\r\n");
            Console.Write("Address : ");

            string certAddr = Console.ReadLine();
            if (certAddr[0] == '"' && certAddr[^1] == '"')
            {
                certAddr = certAddr[1..^1];
            }
            Uri uriResult;
            bool result = Uri.TryCreate(certAddr, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (result)
            {
                rawCert = HttpHandler.GetCertOfURL(certAddr).Result;
            }
            else
            {
                rawCert = File.ReadAllBytes(certAddr);
            }


            if (CertificateParser.Load(rawCert))
            {
                Console.WriteLine("Certificate Loaded Successfully !");
                Console.WriteLine("Press 'P' to print cert");
                Console.WriteLine("Press 'E' to Evaluate cert");
                while (true)
                {
                    var key = Console.ReadKey();
                    Console.WriteLine();
                    Console.WriteLine("===================================");
                    switch (key.Key)
                    {
                        case ConsoleKey.P:
                            Console.WriteLine(CertificateParser.PrintData());
                            break;
                        case ConsoleKey.E:
                            Console.WriteLine();
                            Console.WriteLine(CertificateParser.Validate());
                            break;
                        case ConsoleKey.C:
                            Console.Clear();
                            break;
                        default:
                            Console.WriteLine("===================================");
                            Console.WriteLine("Press 'P' to print cert");
                            Console.WriteLine("Press 'E' to Evaluate cert");
                            Console.WriteLine("Press 'C' to Evaluate cert");
                            break;
                    }
                    Console.WriteLine("===================================");
                    Console.WriteLine("Press 'P' to print cert");
                    Console.WriteLine("Press 'E' to Evaluate cert");
                    Console.WriteLine("Press 'C' to Evaluate cert");

                }


            }
            else
            {
                Console.WriteLine("Certificate Load Falied :(");
            }

            Console.ReadKey();
        }
    }
}
