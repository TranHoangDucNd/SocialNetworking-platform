namespace WebDating.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CaculateAge(this DateOnly dob)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var age = today.Year - dob.Year;

            if (dob > today.AddYears(-age)) age--;
            return age;
        }
    }
}
