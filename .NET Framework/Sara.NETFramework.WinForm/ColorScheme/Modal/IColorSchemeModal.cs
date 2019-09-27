using System.Drawing;

namespace Sara.NETFramework.WinForm.ColorScheme.Modal
{
    public interface IColorSchemeModal
    {
        string Name { get; set; }
        Color BackColor { get; set; }
        Color ForeColor { get; set; }
        void SetDarkDefault();
        void SetLightDefault();
        void Apply(object control);
    }
}
