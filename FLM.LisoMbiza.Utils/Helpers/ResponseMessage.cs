namespace FLM.LisoMbiza
{
    public static class ResponseMessage
    {
        /// <summary>
        /// Maps to the response helper
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        /// <param name="error"></param>
        /// <param name="stackTrace"></param>
        /// <param name="success"></param>
        /// <returns></returns>
        public static Response<TData> ToResponseHelper<TData>(TData data, string error = null, string stackTrace = null, bool success = true)
        {
            return new Response<TData>
            {
                IsSuccessful = success,
                Data = data,
                ErrorMessage = error,
                StackTrace = stackTrace,
            };
        }
    }
}
