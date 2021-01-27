namespace NebulaMigration.IntegrationTests
{
    internal static class Environments
    {
#if DEBUG
        public static string Host => "http://localhost:5001";
#else
        public static string Host => "http://web-api";
#endif
    }
}