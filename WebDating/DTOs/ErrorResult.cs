namespace WebDating.DTOs
{
    public class ErrorResult<T> : ResultDto<T>
    {
        public ErrorResult()
        {
            IsSuccessed = false;
        }
        public ErrorResult(string message)
        {
            IsSuccessed = false;
            Message = message;   
        }
    }
}
