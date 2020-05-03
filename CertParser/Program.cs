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
            Console.WriteLine("Press 'S' To Sign a CSR ");

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
            Console.WriteLine("Press 'M' to Back to main menu");
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
               
                bool running = true;
                while (running)
                {
                    if (!running)
                    {
                        break;
                    }

                    ParsePrintOut(true);
                    var key = Console.ReadKey();
                    Console.Clear();
                    switch (key.Key)
                    {
                        case ConsoleKey.P:
                            Console.WriteLine(CertificateParser.PrintData());
                            break;
                        case ConsoleKey.E:
                            Console.WriteLine();
                            Console.WriteLine(CertificateParser.Validate());
                            break;
                        case ConsoleKey.M:
                            running = false;
                            break;
                        case ConsoleKey.C:
                            Console.Clear();
                            break;
                        default:
                            ParsePrintOut(true);
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Certificate Load Falied :(");
            }
        }

        private static void CertSign()
        {
            Console.Clear();
            Console.WriteLine("Please Enter CSR File Path , your CA cert , your CA Key");
            
            Console.Write("CSR File Path : ");
            var csrPath = GetCleanPath();

            Console.Write("Your CA Cert Path : ");
            var caCertPath = GetCleanPath();

            Console.Write("Your CA Key : ");
            var caKey = GetCleanPath();

            CertificateSigner.SignCSR(csrPath , caCertPath , caKey);

            
        }


        static void Main(string[] args)
        {

            Console.WriteLine("Welcome To Certificate Parser V1 :D");

            while (true)
            {
                InitialPrintOut(true);
                var input = Console.ReadKey();
                switch (input.Key)
                {
                    case ConsoleKey.R:
                        {
                            string DN = CSR.RequestCertificate();
                            var res = CSR.CreateCSR(DN);
                            Console.WriteLine($"CSR Created! at : {res.Item2}");
                            break;
                        }
                    case ConsoleKey.P:
                        {
                            ParseCert();
                            break;
                        }
                    case ConsoleKey.S:
                        {
                            CertSign();
                            break;
                        }
                    case ConsoleKey.C:
                        Console.Clear();
                        break;
                    default:
                        break;
                }

            }
        }
    }
}
