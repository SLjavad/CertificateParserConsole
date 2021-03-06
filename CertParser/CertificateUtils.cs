﻿using System;
using System.Text;

namespace CertParser
{
    public class CertificateUtils
    {
        public static string PemEncodeSigningRequest(string base64)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("-----BEGIN CERTIFICATE REQUEST-----");

            int offset = 0;
            const int LineLength = 64;

            while (offset < base64.Length)
            {
                int lineEnd = Math.Min(offset + LineLength, base64.Length);
                builder.AppendLine(base64.Substring(offset, lineEnd - offset));
                offset = lineEnd;
            }

            builder.AppendLine("-----END CERTIFICATE REQUEST-----");
            return builder.ToString();
        }

        public static string PemEncodeKey(string base64)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("-----BEGIN RSA PRIVATE KEY-----");

            int offset = 0;
            const int LineLength = 64;

            while (offset < base64.Length)
            {
                int lineEnd = Math.Min(offset + LineLength, base64.Length);
                builder.AppendLine(base64.Substring(offset, lineEnd - offset));
                offset = lineEnd;
            }

            builder.AppendLine("-----END RSA PRIVATE KEY-----");
            return builder.ToString();
        }
    }
}
