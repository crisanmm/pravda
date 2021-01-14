using Stripe;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(StripeAPI.Startup))]

namespace StripeAPI
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            StripeConfiguration.ApiKey = "sk_test_xt3znWTewSUOzvv823g2WDMn00oIzLODOH";
        }
    }
}