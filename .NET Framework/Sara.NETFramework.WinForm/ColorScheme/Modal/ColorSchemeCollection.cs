using System;
using System.Collections.Generic;
using System.Linq;

namespace Sara.NETFramework.WinForm.ColorScheme.Modal
{

    public class ColorSchemeCollection<T> where T : class, IColorSchemeModal, new()
    {
        public List<T> Collection { get; set; }
        public string ActiveColorScheme { get; set; }
        public T Current
        {
            get
            {
                if (Collection.Count == 0)
                {
                    var c = new T();
                    c.SetDarkDefault();
                    return c;
                }

                var cs = (Collection.FirstOrDefault(n => n.Name == ActiveColorScheme) ?? Collection.FirstOrDefault(n => n.Name == "Dark")) ??
                         Collection[0];

                return cs;
            }
        }
        public ColorSchemeCollection()
        {
            Collection = new List<T>();
        }

        public void Initialize()
        {
            var t = ColorService.ColorSchemeType ?? typeof(T);

            if (!(Activator.CreateInstance(t) is T dark)) return;
            dark.Name = "Dark";
            dark.SetDarkDefault();

            if (!(Activator.CreateInstance(t) is T light)) return;
            light.Name = "Light";
            light.SetLightDefault();

            Collection.Add(dark);
            Collection.Add(light);
        }
    }
}
