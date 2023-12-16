using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gauss.TccUnifaat.Common.Extensions
{
    public class ControllerEnumsExtensions
    {
        public static List<SelectListItem> MontarSelectListParaEnum2<T>(T selected = default(T), bool excludeDefault = false) where T : struct, IConvertible
        {
            var items = new List<SelectListItem>();
            var enums = Enum.GetValues(typeof(T)).Cast<T>();
            foreach (var enumerador in enums)
            {
                if (excludeDefault && enumerador.Equals(default(T)))
                    continue;
                var name = enumerador.GetDisplayName();
                var item = new SelectListItem()
                {
                    Value = enumerador.ToString(),
                    Text = name,
                    Selected = selected.Equals(enumerador)
                };
                items.Add(item);
            }
            return items;
        }
    }
}
