namespace ConsoleApp
{
    public static class ConnectionString
    {
        public static string DefaultConnection { get; } = "DefaultConnection";
        public static string PavilionConnection { get; } = "PavilionConnection";
        public static string OtherConnection { get; set; } = "OtherConnection";
    }
}
