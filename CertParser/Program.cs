using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CertParser
{
    class Program
    {
        static byte[] rawCert;
        private static void InitialPrintOut(bool clear)
        {
            Console.WriteLine("==========================================");
            Console.WriteLine("Press 'R' To Create CSR ");
            Console.WriteLine("Press 'P' To Parse a Certificate ");
            if (clear)
            {
                Console.WriteLine("Press 'C' to Clear Console");
            }
        }

        private static void ParsePrintOut(bool clear)
        {
            Console.WriteLine("===================================");
            Console.WriteLine("Press 'P' to print cert");
            Console.WriteLine("Press 'E' to Evaluate cert");
            if (clear)
            {
                Console.WriteLine("Press 'C' to Clear Console");
            }
        }

        private static string GetCleanPath()
        {
            string certAddr = Console.ReadLine();
            if (certAddr[0] == '"' && certAddr[^1] == '"')
            {
                certAddr = certAddr[1..^1];
            }
            return certAddr;
        }

        private static void ParseCert()
        {
            Console.Clear();
            Console.WriteLine("Now Please Enter Address of Certificate you want to parse:\r\n");
            Console.Write("Address : ");

            string certAddr = GetCleanPath();

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
                Console.WriteLine("Do you have Any Extra Certificates ?(Y/N)");
                var inputExt = Console.ReadKey();
                if (inputExt.Key == ConsoleKey.Y)
                {
                    Console.WriteLine("Enter The Path Of Directory Containing Extra Certificates :");
                    string extAddr = GetCleanPath();
                    CertificateParser.AddExtraCerts(FileHandler.GetExtraCertRawData(extAddr));
                }

                ParsePrintOut(false);

                while (true)
                {
                    var key = Console.ReadKey();
                    Console.Clear();
                    //Console.WriteLine();
                    //Console.WriteLine("===================================");
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
                            ParsePrintOut(true);
                            break;
                    }

                    ParsePrintOut(true);

                }
            }
            else
            {
                Console.WriteLine("Certificate Load Falied :(");
            }

            Console.ReadKey();
        }



        static void Main(string[] args)
        {

            Console.WriteLine("Welcome To Certificate Parser V1 :D");
            InitialPrintOut(false);

            while (true)
            {
                var input = Console.ReadKey();
                switch (input.Key)
                {
                    case ConsoleKey.R:
                        {
                            string DN = CSR.RequestCertificate();
                            string CSRstring = CSR.CreateCSR(DN);
                            
                            Console.WriteLine($"CSR Created! at : {AppDomain.CurrentDomain.BaseDirectory + "\\SLjavad.csr"}");
                            InitialPrintOut(true);
                            break;
                        }
                    case ConsoleKey.P:
                        {
                            ParseCert();
                            break;
                        }
                    case ConsoleKey.C:
                        Console.Clear();
                        break;
                    default:
                        InitialPrintOut(true);
                        break;
                }

            }
        }
    }
}
