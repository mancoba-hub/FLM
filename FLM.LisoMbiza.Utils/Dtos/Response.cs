namespace FLM.LisoMbiza
{
    public class Response<TData>
    {
        public bool IsSuccessful { get; set; }

        public string ErrorMessage { get; set; }

        public string StackTrace { get; set; }

        public TData Data { get; set; }
    }
}
