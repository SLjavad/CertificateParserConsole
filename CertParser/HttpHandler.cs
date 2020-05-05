using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CertParser
{
    class HttpHandler
    {
        public static async Task<byte[]> GetCertOfURL(string url)
        {
            X509Chain chiann;
            X509Certificate2 certt = new X509Certificate2();
            using (HttpClient httpClient = new HttpClient(new HttpClientHandler {

                UseDefaultCredentials = true,
                ServerCertificateCustomValidationCallback = (sender, cert, chain, error) =>
                {
                    chiann = chain;
                    certt = new X509Certificate2(cert);
                    return true;
                }
                
            }))
            {
                await httpClient.GetAsync(url);
                return certt.RawData;
            }
        }
    }
}
