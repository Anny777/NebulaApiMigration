namespace NebulaMigration.IntegrationTests
{
    using System;

    internal static class Environments
    {
        private static bool DotNetRunInContainer => Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER")?
            .Equals("true", StringComparison.OrdinalIgnoreCase) ?? false;
        public static string Host => DotNetRunInContainer ? "http://web-api" : "http://localhost:5001";
    }
}