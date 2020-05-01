using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CertParser
{
    public class CSR
    {
        public static string CreateCSR(string DN)
        {

            RSA rSA = new RSACryptoServiceProvider(2048);
            CertificateRequest certificateRequest = new CertificateRequest(
                $"{DN}", rSA, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            //certificateRequest.CertificateExtensions.Add(
            //    new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature, false));

            //certificateRequest.CertificateExtensions.Add(
            //    new X509SubjectKeyIdentifierExtension(certificateRequest.PublicKey, false));

            //certificateRequest.CertificateExtensions.Add(
            //    new X509BasicConstraintsExtension(false, false, 0, false));

            var csr = certificateRequest.CreateSigningRequest();

            return CertificateUtils.PemEncodeSigningRequest(Convert.ToBase64String(csr));
        }

        public static string RequestCertificate()
        {
            StringBuilder stringBuilder = new StringBuilder();
            Console.Clear();
            Console.WriteLine("Now Enter The Information Needed To Make CSR.");
            Console.Write("Common Name (CN): ");
            stringBuilder.Append($"CN={Console.ReadLine()};");

            Console.WriteLine();
            Console.Write("Organization (O): ");
            stringBuilder.Append($"O={Console.ReadLine()};");

            Console.WriteLine();
            Console.Write("Organization Unit (OU): ");
            stringBuilder.Append($"OU={Console.ReadLine()};");

            Console.WriteLine();
            Console.Write("Locality (L): ");
            stringBuilder.Append($"L={Console.ReadLine()};");

            Console.WriteLine();
            Console.Write("State (ST): ");
            stringBuilder.Append($"ST={Console.ReadLine()};");

            Console.WriteLine();
            Console.Write("Country (C): ");
            stringBuilder.Append($"C={Console.ReadLine()};");

            return stringBuilder.ToString();
        }
    }
}
