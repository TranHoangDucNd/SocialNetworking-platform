namespace WebDating.DTOs
{
    public class ResultDto<T>
    {
        public bool IsSuccessed { get; set; }
        public string Message { get; set; } = string.Empty;
        public T ResultObj { get; set; }
    }
}
