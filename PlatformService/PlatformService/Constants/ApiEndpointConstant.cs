namespace PlatformService.Constants
{
    public class ApiEndpointConstant
    {
        static ApiEndpointConstant()
        {
        }

        private const string RootEndpoint = "/api";
        private const string ApiVersion = "/v1";
        public const string ApiEndpoint = RootEndpoint + ApiVersion;

        public static class Platform
        {
            // Duoi co "s" danh cho nhung tac vu Create(POST) hoac GetALL (GET)
            public const string PlatformsEndpoint = ApiEndpoint + "/platforms";

            // Duoi co them {id} danh cho nhung tac vu Update(PUT) hoac GetById(GET) hoac Delete(DELETE)
            public const string PlatformById = PlatformsEndpoint + "/{id}";
        }

    }
}
