using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MyTools.Common
{
    public class CommonConstant
    {
        public static CoreWebView2Environment? _webView2Environment { get; private set; }

        public static async Task<CoreWebView2Environment> GetWebView2EnvironmentAsync()
        {
            if(_webView2Environment != null)
                return _webView2Environment;

            // Environment.SetEnvironmentVariable("WEBVIEW2_ADDITIONAL_BROWSER_ARGUMENTS", "--disable-web-security");
            var op = new CoreWebView2EnvironmentOptions("--disable-web-security");
            var userFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WebView2.Cache");
            _webView2Environment = await CoreWebView2Environment.CreateAsync(null, userFolder, op);
            return _webView2Environment;
        }
    }
}
