# DueDil API V4 C# Client Integration

Prerequisites
-------------
1. brew install mono
2. Intsall [VisualStudio](https://docs.microsoft.com/en-us/visualstudio/mac/installation)

Build new API client
--------------------

1.  Go to API docs page [https://www.duedil.com/api/docs](https://www.duedil.com/api/docs)
2.  Click the top button **"Generate DueDil V4 API client"**
3.  From the dropdown menu select you desired language/platform, for this tutorial **"C# .NET 2"** and download the ZIP package

Preparing the client DLL
------------------------

1.  This is for Mac/Linux users only
2.  Extract the archive with the generated client
3.  Compile the client code  
      
```bash
/bin/bash compile-mono.sh
```      
    
4.  The client should be located under **bin/IO.Swagger.dll**

Create new C# project and import client
---------------------------------------

1.  Open VisualStudio and create new MVC project
2.  In the root of the project create new folder **"libs/"**
3.  Copy the DLL file of the Swagger client there
4.  From the top menu select **"Project > Edit Preferences > .Net Assembly (tab)"**
5.  Browse and add the Swagger client to the build of the project **"./libs/IO.Swagger.dll"**
6.  From the top menu select **"Project > Add NuGet Packages ... > Search for "RestSharp.Net2" and install"**
7.  Build the project

Updating the code to work with the API
--------------------------------------
```csharp
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
        const string API\_AUTH\_TOKEN = "<YOUR\_API\_KEY_HERE>";
        const string DEFAULT\_COMPANY\_ID = "06999618";

        private IEssentialsApi essentialsApi;

        public HomeController() 
        {
            Configuration.ApiKey\["X-AUTH-TOKEN"\] = API\_AUTH\_TOKEN;
            essentialsApi = new EssentialsApi();
        }

        public IActionResult Index(String CompanyId)
        {
            try
            {
                string companyId = CompanyId == null ? DEFAULT\_COMPANY\_ID: CompanyId;
            
                Debug.WriteLine(CompanyId);
                Debug.WriteLine(companyId);

                // Company vitals
                CompanyResponse result = essentialsApi.CompanyCountryCodeCompanyIdFormatGet("gb", companyId, RESPONSE_FORMAT);

                ViewData\["companyId"\] = result.CompanyId;
                ViewData\["countryCode"\] = result.CountryCode;
                ViewData\["name"\] = result.Name;
                ViewData\["status"\] = result.SimplifiedStatus;
                ViewData\["address"\] = result.RegisteredAddress.FullAddress;

                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling EssentialsApi.CompanyCountryCodeCompanyIdFormatGet: " + e.Message);
            }

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
```
