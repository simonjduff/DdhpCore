using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Table;

namespace DdhpCore.FrontEnd.Extensions
{
    public static class TableResultExtensions
    {
        public static IActionResult ToActionResult(this TableResult result, ControllerBase controller)
        {
            switch ((HttpStatusCode)result.HttpStatusCode)
            {
                case HttpStatusCode.NotFound:
                    return controller.NotFound();
                default:
                    return new ObjectResult(result.Result);
            }
        }
    }
}