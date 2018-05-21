using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DuedilApi.Models;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace DuedilApi.Controllers
{    
    public class HomeController : Controller
    {
        const string RESPONSE_FORMAT = "json";
        const string API_AUTH_TOKEN = "<YOUR_API_KEY_HERE>";
        const string DEFAULT_COMPANY_ID = "06999618";

        private IEssentialsApi essentialsApi;

        public HomeController() 
        {
            Configuration.ApiKey["X-AUTH-TOKEN"] = API_AUTH_TOKEN;
            essentialsApi = new EssentialsApi();
        }

        public IActionResult Index(String CompanyId)
        {
            try
            {
                string companyId = CompanyId == null ? DEFAULT_COMPANY_ID: CompanyId;

                Debug.WriteLine(CompanyId);
                Debug.WriteLine(companyId);

                // Company vitals
                CompanyResponse result = essentialsApi.CompanyCountryCodeCompanyIdFormatGet("gb", companyId, RESPONSE_FORMAT);

                ViewData["companyId"] = result.CompanyId;
                ViewData["countryCode"] = result.CountryCode;
                ViewData["name"] = result.Name;
                ViewData["status"] = result.SimplifiedStatus;
                ViewData["address"] = result.RegisteredAddress.FullAddress;

                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling EssentialsApi.CompanyCountryCodeCompanyIdFormatGet: " + e.Message);
            }

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
