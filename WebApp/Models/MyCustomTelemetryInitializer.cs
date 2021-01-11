using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Channel;

namespace WebApp.Models {
  public class MyCustomTelemetryInitializer : ITelemetryInitializer
  {
      public MyCustomTelemetryInitializer()
      {
      }

      public void Initialize(ITelemetry telemetry)
      {
          if (telemetry == null)
          {
              return;
          }

          telemetry.Context.GlobalProperties["SomePropertyName"] = "SomePropertyValue";
      }
      public string fieldName {get; set;}
  }
}