using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PipeCI.Blob.FunctionalTests
{
    public class CustomFormUrlEncodedContent : ByteArrayContent
    {
        public CustomFormUrlEncodedContent(IEnumerable<KeyValuePair<string, string>> nameValueCollection)
            : base(GetContentByteArray(nameValueCollection))
        {
            Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
        }

#if PHONE || NETNative
        internal static readonly Encoding DefaultHttpEncoding = Encoding.GetEncoding("iso-8859-1");
#else
        internal static readonly Encoding DefaultHttpEncoding = Encoding.GetEncoding(28591);
#endif

        private static byte[] GetContentByteArray(IEnumerable<KeyValuePair<string, string>> nameValueCollection)
        {
            if (nameValueCollection == null)
            {
                throw new ArgumentNullException("nameValueCollection");
            }

            // Encode and concatenate data
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in nameValueCollection)
            {
                if (builder.Length > 0)
                {
                    builder.Append('&');
                }

                builder.Append(Encode(pair.Key));
                builder.Append('=');
                builder.Append(Encode(pair.Value));
            }

            return DefaultHttpEncoding.GetBytes(builder.ToString());
        }

        private static string Encode(string data)
        {
            if (String.IsNullOrEmpty(data))
            {
                return String.Empty;
            }
            // Escape spaces as '+'.
            return WebUtility.UrlEncode(data).Replace("%20", "+");
        }
    }
}