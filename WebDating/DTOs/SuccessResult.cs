namespace WebDating.DTOs
{
    public class SuccessResult<T> : ResultDto<T>
    {
        public SuccessResult()
        {
            IsSuccessed = true;
        }
        public SuccessResult(T resultObj)
        {
            IsSuccessed= true;
            ResultObj = resultObj;
        }
        public SuccessResult(T resultObj, string message)
        {
            IsSuccessed = true;
            ResultObj = resultObj;
            Message = message;
        }
    }
}
