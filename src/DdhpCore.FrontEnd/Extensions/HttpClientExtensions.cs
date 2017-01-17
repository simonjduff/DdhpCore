using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace DdhpCore.FrontEnd.Extensions
{
    public static class HttpClientExtensions
    {
        public static HttpClient ApiBaseAddress(this HttpClient client, HttpRequest request)
        {
            client.BaseAddress = new Uri($"{request.Scheme}://{request.Host}/api/", UriKind.Absolute);

            return client;
        }
    }
}