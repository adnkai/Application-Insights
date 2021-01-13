using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.ApplicationInsights;
using System.Threading;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Linq;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private TelemetryClient telemetry;
        public IndexModel(ILogger<IndexModel> logger, TelemetryClient telemetry)
        {
            _logger = logger;
            this.telemetry = telemetry;
        }

        public void OnGet()
        {
          this.telemetry.TrackEvent("HomePageRequested");
          GetBTCValue();
          if (@RouteData.Values["raise"] != null) {
            this.telemetry.TrackEvent("ManuallyRaisedException");
            RaiseException();
          }
        }
        
        public void RaiseException(){
          throw(new Exception());
        }

        public void GetBTCValue() {
          var client = new HttpClient();
          string btcData = client.GetStringAsync("https://blockchain.info/ticker").Result;
          this.telemetry.TrackEvent("FetchedData");
          ViewData["btc"] = btcData;
        }

    }
}
