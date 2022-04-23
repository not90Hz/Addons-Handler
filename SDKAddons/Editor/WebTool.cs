using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

namespace SDKAddons
{
    public class WebTool
    {
        public static string ReadTextFromUrl(string url)
        {

            using (var client = new WebClient())

            using (var stream = client.OpenRead(url))

            using (var textReader = new StreamReader(stream, Encoding.UTF8, true))

            {

                return textReader.ReadToEnd();

            }

        }
    }
}
