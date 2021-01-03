using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Stripe;

namespace StripeExample
{
  public class Program
  {
    public static void Main(string[] args)
    {
      WebHost.CreateDefaultBuilder(args)
        .UseUrls("https://localhost:5500/")
        .UseWebRoot(".")
        .UseStartup<Startup>()
        .Build()
        .Run();
    }
  }

  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc().AddNewtonsoftJson();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      StripeConfiguration.ApiKey = "sk_test_51I0pMtC4Z1dRglPMvv09PGilfkGQfnjBJ0Iy0py75vb07sV0dpNJHjs00O4lroatKCmrDyKSa5YaQsDt6IoPe1vW00TnhS2sU8";

      if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
      app.UseRouting();
      app.UseStaticFiles();
      app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
  }

  [Route("create-payment-intent")]
  [ApiController]
  public class PaymentIntentApiController : Controller
  {
    [HttpPost]
    public ActionResult Create(PaymentIntentCreateRequest request)
    {
      var paymentIntents = new PaymentIntentService();
      var paymentIntent = paymentIntents.Create(new PaymentIntentCreateOptions
      {
        Amount = CalculateOrderAmount(request.Items),
        Currency = "usd",
      });

      return Json(new { clientSecret = paymentIntent.ClientSecret });
    }

    private int CalculateOrderAmount(Item[] items)
    {
      return 500;
    }

    public class Item
    {
      [JsonProperty("id")]
      public string Id { get; set; }
    }

    public class PaymentIntentCreateRequest
    {
      [JsonProperty("items")]
      public Item[] Items { get; set; }
    }
  }
}