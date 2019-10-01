using System;
using System.Windows.Forms;
using Sara.WinForm.ColorScheme.View;
using Sara.WinForm.ColorScheme.Modal;

namespace Sara.WinForm.ColorScheme.ViewModel
{
    public static class ColorSchemeViewModel
    {
        public static ColorSchemeWindow View { get; set; }

        public static ColorSchemeCollection<ColorSchemeBaseModal> GetModel()
        {
            return ColorService.ColorScheme;
        }
        public static void ShowColorScheme()
        {
            View = new ColorSchemeWindow();
            View.ShowDialog();
        }

        internal static void Add(string name)
        {
            if (name == string.Empty)
            {
                MessageBox.Show("Name of the Color Scheme cannot be blank!");
                return;
            }

            var m = GetModel();
            var i = Activator.CreateInstance(ColorService.ColorSchemeType) as ColorSchemeBaseModal;
            if (i != null)
            {
                i.Name = name;
                m.Collection.Add(i);
            }

            View.Render();
        }

        internal static void Add()
        {
            var view = new ColorSchemeAddEdit()
            {
                Text = "Adding a Color Scheme",
            };
            if (view.ShowDialog() != DialogResult.OK) return;
            var m = GetModel();
            if (Activator.CreateInstance(ColorService.ColorSchemeType) is ColorSchemeBaseModal i)
            {
                i.Name = view.ColorSchemeName;
                m.Collection.Add(i);
            }

            View.Render();
        }

        public static void Save()
        {
            ColorService.Save();
        }

        internal static void Delete(object model)
        {
            if (model == null)
            {
                MessageBox.Show("You must select a Color Scheme to Delete.");
                return;
            }

            var m = GetModel();
            foreach (var item in m.Collection)
            {
                if (item.Name == (model as ColorSchemeBaseModal)?.Name)
                {
                    m.Collection.Remove(item);
                    break;
                }
            }
            View.Render();
        }

        internal static void Edit(object item)
        {
            if (item == null)
            {
                MessageBox.Show("You must select a Color Scheme to Edit.");
                return;
            }

            if (!(item is ColorSchemeBaseModal model)) return;
            var view = new ColorSchemeAddEdit()
            {
                Text = "Editing a Color Scheme",
                ColorSchemeName = model.Name
            };
            if (view.ShowDialog() == DialogResult.OK)
            {
                model.Name = view.ColorSchemeName;
                View.Render();
            }
        }
    }
}
