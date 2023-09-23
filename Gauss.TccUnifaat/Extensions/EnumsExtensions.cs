using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Gauss.TccUnifaat.Extensions
{
    public static class EnumsExtensions
    {
        public static string GetDisplayName<T>(this T enumerador) where T : struct, IConvertible
        {
            var name = Enum.GetName(typeof(T), enumerador);
            var display = typeof(T).GetMember(name).First().GetCustomAttribute<DisplayAttribute>();
            if (display != null)
            {
                name = display.Name;
            }
            return name;
        }
    }
}
