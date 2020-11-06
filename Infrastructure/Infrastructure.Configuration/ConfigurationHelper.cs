using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configuration
{
    public static class ConfigurationHelper
    {
        /// <summary>
        /// Configures the eagerly.
        /// </summary>
        /// <typeparam name="T">Type of options.</typeparam>
        /// <param name="sc">The sc.</param>
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="InvalidOperationException">When section has empty values.</exception>
        public static void ConfigureEagerly<T>(this IServiceCollection sc, IConfiguration configuration)
            where T : class
        {
            if (sc is null)
            {
                throw new ArgumentNullException(nameof(sc));
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var sectionName = typeof(T).Name;
            var section = configuration.GetSection(sectionName);

            sc
                .AddOptions<T>()
                .Bind(section);
        }
    }
}
