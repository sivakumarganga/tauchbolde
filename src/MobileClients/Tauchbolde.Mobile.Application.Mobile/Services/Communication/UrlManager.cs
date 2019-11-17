using System;

namespace Tauchbolde.Mobile.Application.Services.Communication
{
    internal class UrlManager : IUrlManager
    {
        private const string baseUrl = "https://localhot:5000";
        
        public Uri GetUrl(string relativeActionUrl, params object[] queryParams)
        {
            var uriBuilder = new UriBuilder(baseUrl);
            uriBuilder.
            
            var baseUri = new Uri(baseUrl);
            var uri = new Uri(baseUri, relativeActionUrl);
            uri.Query
        }
    }
}