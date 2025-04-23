namespace CommandService.Models.MetaDatas
{
    public class ApiResponseBuilder
    {

        // This method is used to build a response object for single data
        public static ApiResponse<T> BuildResponse<T>(int statusCode, string message, T? data)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<object> BuildResponse(int statusCode, string message)
        {
            return new ApiResponse<object>
            {
                StatusCode = statusCode,
                Message = message,
                Data = null
            };
        }

        // This method is used to build a response object for error response
        public static ApiResponse<T> BuildErrorResponse<T>(T data, int statusCode, string message)
        {
            return new ApiResponse<T>
            {
                Data = data,
                StatusCode = statusCode,
                Message = message
            };
        }

        // This method is used to build a response object for list/pagination data
        public static ApiResponse<PagingResponse<T>> BuildPageResponse<T>(
            IEnumerable<T> items,
            int totalPages,
            int currentPage,
            int pageSize,
            long totalItems,
            string message)
        {
            var pagedResponse = new PagingResponse<T>
            {
                Items = items,
                Meta = new PaginationMeta
                {
                    TotalPages = totalPages,
                    CurrentPage = currentPage,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return new ApiResponse<PagingResponse<T>>
            {
                Data = pagedResponse,
                Message = message,
                StatusCode = 200
            };
        }
    }
}
