namespace GRH_SENTECH.Service
{

    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public static ServiceResult<T> Ok(T data, string message = "Opération réussie")
        {
            return new ServiceResult<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static ServiceResult<T> Fail(string message)
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message
            };
        }

        public static ServiceResult<T> Fail(List<string> errors)
        {
            return new ServiceResult<T>
            {
                Success = false,
                Errors = errors,
                Message = string.Join(", ", errors)
            };
        }
    }
}
