namespace Car_Utility
{
    public static class SD
    {
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE  
        }
        public static string AccessToken = "JWTToken";
        public static string RefreshToken = "RefreshToken";
        public static string CurrentApiVersion = "v2";

        public static string Admin = "admin";
        public static string Customer = "customer";

        public enum ContentType
        {
            Json,
            MultipartFormData
        }
    }
}
