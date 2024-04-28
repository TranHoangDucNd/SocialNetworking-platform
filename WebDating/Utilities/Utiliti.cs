using System.ComponentModel.DataAnnotations;
using System.Reflection;
using WebDating.DTOs;

namespace WebDating.Utilities
{
    public static class Utils
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var displayNameAttribute = enumValue.GetType().GetField(enumValue.ToString())!
                .GetCustomAttribute<DisplayAttribute>();

            return displayNameAttribute?.Name ?? enumValue.ToString();
        }

        public static List<EnumT<T>> GetAllAccountType<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(e => new EnumT<T>
                {
                    DisplayName = GetDisplayName(e),
                    Value = Convert.ToInt32(e)
                }).ToList();
        }
    }
}
