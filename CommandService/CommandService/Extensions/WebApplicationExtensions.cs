namespace CommandService.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void PrepPopulation(this IApplicationBuilder app)
        {
            Data.PrepData.PrepPopulation(app)
                .Wait(); // Wait for the population to complete before proceeding
        }
    }
}