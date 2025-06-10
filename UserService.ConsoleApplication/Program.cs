using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UserService.Client;
using UserService.Configuration;
using UserService.Services;


var host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();

        logging.AddFilter("System.Net.Http.HttpClient", LogLevel.Warning);
    })
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.Configure<ApiSettings>(context.Configuration.GetSection("ApiSettings"));

        services.AddHttpClient<IReqResApiClient, ReqResApiClient>((sp, client) =>
        {
            var settings = sp.GetRequiredService<IOptions<ApiSettings>>().Value;

            client.BaseAddress = new Uri(settings.BaseUrl);
            client.DefaultRequestHeaders.Add("X-API-KEY", settings.ApiKey);
        });
        services.AddScoped<IExternalUserService, ExternalUserService>();
    })
    .Build();

var service = host.Services.GetRequiredService<IExternalUserService>();

Console.WriteLine("All Users:");
var users = service.GetAllUsersAsync().GetAwaiter().GetResult();
foreach (var user in users)
{
    Console.WriteLine($"{user.Id}: {user.FirstName} {user.LastName} ({user.Email})");
}

while (true)
{
    Console.Write("Enter user ID (or 0 to exit): ");

    if (int.TryParse(Console.ReadLine(), out int userId))
    {
        if (userId == 0)
        {
            Console.WriteLine("Exiting...");
            break;
        }

        var user = service.GetUserByIdAsync(userId).GetAwaiter().GetResult();

        if (user != null)
        {
            Console.WriteLine($"Found: {user.FirstName} {user.LastName} ({user.Email})");
        }
        else
        {
            Console.WriteLine("User not found.");
        }
    }
    else
    {
        Console.WriteLine("Invalid input. Please enter a valid numeric user ID.");
    }

    Console.WriteLine();
}