namespace WebDating.DTOs
{
    public class EnumT<T> where T : Enum
    {
        public int Value { get; set; }
        public string DisplayName { get; set; }
    }
}
