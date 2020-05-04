using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CertParser
{
    class CertificateParser
    {

        public static X509Certificate2 certificate;
        private static X509Certificate2Collection x509Certificate2Collection = new X509Certificate2Collection();

        public static void AddExtraCerts(List<byte[]> extraCerts)
        {
            extraCerts.ForEach((cert) =>
            {
                x509Certificate2Collection.Add(new X509Certificate2(cert));
            });
        }

        private static X509ChainStatus[] BuildChain(X509Certificate2 cert)
        {
            X509Chain chain = new X509Chain();
            try
            {
                if (x509Certificate2Collection.Count > 0)
                {
                    chain.ChainPolicy.ExtraStore.AddRange(x509Certificate2Collection);
                }
                chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
                var chainBuilt = chain.Build(cert);
                return chain.ChainStatus;
            }
            catch (Exception ex)
            {
            }
            return null;
        }
        public static bool Load(byte[] rawData)
        {
            try
            {
                certificate = new X509Certificate2(rawData);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTTION");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }

        public static string PrintData()
        {
            return certificate.ToString(true);
        }

        public static string Validate()
        {

            if (certificate.Verify())
            {
                return "Certificate is valid\n\n" + certificate.ToString();    
            }
            else
            {
                X509ChainStatus[] x509ChainStatuses = BuildChain(certificate);
                if (x509ChainStatuses.Length == 0)
                {
                    return "Certificate is valid for errors 13, 14, 7 but for something else is not valid\n\n" + certificate.ToString();
                }
                string result = string.Empty;
                foreach (X509ChainStatus item in x509ChainStatuses)
                {
                    result += $"chain status : {item.Status} ::: info : {item.StatusInformation}\n===============\n";
                }
                return result;
            }
        }

        public static string FingerPrintCheck(X509Certificate2 certificate1 , X509Certificate2 certificate2)
        {
            //var finger1 = certificate1.Thumbprint;
            //var finger2 = certificate2.Thumbprint;

            var fr1_sha256 = certificate1.GetCertHashString(HashAlgorithmName.SHA256);
            var fr2_sha256 = certificate2.GetCertHashString(HashAlgorithmName.SHA256);

            var fr1_sha1 = certificate1.GetCertHashString(HashAlgorithmName.SHA1);
            var fr2_sha1 = certificate2.GetCertHashString(HashAlgorithmName.SHA1);

            var fr1_md5 = certificate1.GetCertHashString(HashAlgorithmName.MD5);
            var fr2_md5 = certificate2.GetCertHashString(HashAlgorithmName.MD5);

            string result = $"Computed Fingerprints : \n\nfirst certificate sha256: {fr1_sha256}\n" +
                $"second certificate sha256: {fr2_sha256}\n\n" +
                $"first certificate sha1: {fr1_sha1}\n" +
                $"second certificate sha1: {fr2_sha1}\n\n" +
                $"first certificate md5: {fr1_md5}\n" +
                $"second certificate md5: {fr2_md5}";


            return result;
        }
    }
}
