using FunctionLibrary.Standard;
using FunctionLibrary.Standard.Data;
using FunctionLibrary.Standard.DocumentConverter;
using FunctionLibrary.Standard.Graph;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

[assembly: FunctionsStartup(typeof(IntuneFunctions.Startup))]
namespace IntuneFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<GraphHelper>();
            builder.Services.AddScoped<AuthenticationManager>();
        }
    }
}
