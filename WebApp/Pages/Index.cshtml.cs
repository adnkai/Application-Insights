using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.ApplicationInsights;

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
          if (new Random().Next(1,3) == 1) {
            RaiseException();
          }
        }
        public void RaiseException(){
          throw(new Exception());
        }
    }
}
