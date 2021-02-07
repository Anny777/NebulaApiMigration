namespace NebulaMigration
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry point.
        /// </summary>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Create host builder.
        /// </summary>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host
                    .CreateDefaultBuilder(args)
                    .ConfigureAppConfiguration(ConfigureAppConfiguration)
                    .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
        }

        private static void ConfigureAppConfiguration(HostBuilderContext hostBuilderContext, IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.AddJsonFile("./configs.json", true);
        }
    }
}
