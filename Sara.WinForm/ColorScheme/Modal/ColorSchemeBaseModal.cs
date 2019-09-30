using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Xml.Serialization;
using Sara.NETFramework.WinForm.ControlsNS;
using Sara.WinForm.ColorScheme.ControlNS;
using Sara.WinForm.ControlsNs;

namespace Sara.WinForm.ColorScheme.Modal
{
    public class ColorSchemeBaseModal : IColorSchemeModal
    {
        [Browsable(false)]
        public string Name { get; set; }

        #region Methods
        public ColorSchemeBaseModal()
        {
            SplitterWidth = 5;

            SetDarkDefault();
        }

        [Browsable(false)]
        public Color BackColor { get; set; }
        [Browsable(false)]
        public Color ForeColor { get; set; }

        public void SetDarkDefault()
        {
            BackColor = Color.FromArgb(30, 30, 30);
            ForeColor = Color.FromArgb(220, 220, 220);

            ControlBackColor = BackColor;
            ControlForeColor = ForeColor;
            MenuBackColor = Color.FromArgb(64, 64, 64);
            MenuForeColor = ForeColor;

            ApplicationBackColor = Color.FromArgb(64, 64, 64);

            SplitterBackColor = ApplicationBackColor;

            GeneralFont = "Consolas";
            GeneralFontSize = 10;
            GeneralPlusFontSize = 14;
            ButtonFont = "Consolas";
            ButtonFontSize = 8;
            ButtonBackColor = ApplicationBackColor;
            ButtonFlatStyle = FlatStyle.Flat;
            ButtonForeColor = Color.White;

            GridAlternativeBackColor = Color.Black;
            GridHeaderBackColor = Color.DimGray;
            GridHeaderForeColor = ForeColor;

            StatusBackColor = Color.Black;
            StatusForeColor = Color.FromArgb(78, 201, 176);
            StatusBorderSize = 1;
            StatusBorderColor = Color.DeepSkyBlue;

            HighlightBorderBackColor = Color.DimGray;
            ButtonSelectedBackColor = Color.DarkGray;
            ButtonSelectedForeColor = Color.Black;
        }

        public void SetLightDefault()
        {
            BackColor = Color.FromKnownColor(KnownColor.Control);
            ForeColor = Color.FromKnownColor(KnownColor.ControlText);

            SplitterBackColor = SystemColors.Control;

            ControlBackColor = BackColor;
            ControlForeColor = ForeColor;
            MenuBackColor = BackColor;
            MenuForeColor = ForeColor;

            GeneralFont = "Consolas";
            GeneralFontSize = 10;

            GridAlternativeBackColor = Color.FromArgb(224, 224, 244);
            GridHeaderBackColor = BackColor;
            GridHeaderForeColor = Color.Black;

            StatusBackColor = Color.Silver;
            StatusForeColor = Color.Black;
            StatusBorderSize = 1;
            StatusBorderColor = Color.DeepSkyBlue;

            HighlightBorderBackColor = Color.Silver;
        }

        public string ColorToString(Color value)
        {
            return value.IsNamedColor ? value.Name : $"_{value.ToArgb().ToString("x").ToUpper()}";
        }
        public Color StringToColor(string value)
        {
            if (value == null)
                return Color.Transparent;

            return value[0] == '_' ? Color.FromArgb(int.Parse(value.TrimStart('_'), NumberStyles.HexNumber)) : Color.FromName(value);
        }
        public override string ToString()
        {
            return Name;
        }

        public virtual void Apply(object control)
        {
            if (control is Control c1)
            {
                if (c1.InvokeRequired)
                {
                    c1.Invoke(new Action<object>(Apply), control);
                    return;
                }
            }

            if (control is IColorSchemeControl csc)
                csc.ApplyColorScheme();

            if (control is StatusPanel)
                return;

            if (control is ToolStripItem t)
            {
                t.BackColor = ControlBackColor;
                t.ForeColor = ControlForeColor;
                t.Font = GeneralFontObject;
            }

            if (control is Control c)
            {
                c.BackColor = ControlBackColor;
                c.ForeColor = ControlForeColor;
                c.Font = GeneralFontObject;

                if (c is SplitContainer sc)
                {
                    var bc = sc.BackColor;
                    sc.BackColor = SplitterBackColor;
                    sc.Panel1.BackColor = bc;
                    sc.Panel2.BackColor = bc;
                    sc.SplitterWidth = SplitterWidth;
                }

                if (c is RichTextBox rtb)
                {
                    rtb.ForeColor = Color.White;
                }
                if (c is TextBox tb)
                {
                    tb.BorderStyle = BorderStyle.FixedSingle;
                }
                if (c is BorderedTextBox btb)
                {
                    btb.DefaultBorderColor = BackColor;
                    btb.FocusedBorderColor = StatusBorderColor;
                }

                var bp = c as BorderPanel;
                if (bp != null)
                {
                    bp.BackColor = BackColor;
                    bp.FocusedBorderColor = StatusBorderColor;
                }
                if (c is ComboBox cb)
                {
                    cb.FlatStyle = FlatStyle.Flat;
                }

                if (c is DataGridView dgv)
                {
                    dgv.DefaultCellStyle.ForeColor = ControlForeColor;
                    dgv.DefaultCellStyle.BackColor = ControlBackColor;
                    dgv.AlternatingRowsDefaultCellStyle.BackColor = GridAlternativeBackColor;
                    dgv.ColumnHeadersDefaultCellStyle.BackColor = GridHeaderBackColor;
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = GridHeaderForeColor;
                    dgv.ColumnHeadersDefaultCellStyle.Font = new Font(dgv.ColumnHeadersDefaultCellStyle.Font, FontStyle.Bold);
                    // Setting this to false allows us to control the color of the header. - Sara
                    dgv.EnableHeadersVisualStyles = false;
                }

                if (c is Button b)
                {
                    b.Font = ButtonFontObject;
                    b.BackColor = ButtonBackColor;
                    b.FlatStyle = ButtonFlatStyle;
                    b.ForeColor = ButtonForeColor;
                    b.FlatAppearance.BorderColor = HighlightBorderBackColor;
                }

                if (c is CheckBox ck)
                    ck.Font = ButtonFontObject;

                if (c is Label lbl)
                    lbl.Font = ButtonFontObject;

                if (c.Tag != null && c.Tag.ToString() == "Highlight")
                    c.BackColor = HighlightBorderBackColor;

                foreach (Control child in c.Controls)
                    Apply(child);
            }

            if (control.GetType().Name.ToString() == "InertButton")
            {
                if (control is Control b)
                    b.BackColor = SystemColors.Control;
            }

            if (control is IColorSchemeControl csc2)
                csc2.ApplyColorScheme();
        }
        #endregion Methods

        #region General Control
        [XmlIgnore]
        [Description("General Control BackColor."), Category("General")]
        public Color ControlBackColor { get { return StringToColor(ControlBackColorXML); } set { ControlBackColorXML = ColorToString(value); } }
        [Browsable(false)]
        public string ControlBackColorXML { get; set; }

        [XmlIgnore]
        [Description("General Control ForeColor."), Category("General")]
        public Color ControlForeColor { get { return StringToColor(ControlForeColorXML); } set { ControlForeColorXML = ColorToString(value); } }
        [Browsable(false)]
        public string ControlForeColorXML { get; set; }
        #endregion Current Control

        #region Menu
        [XmlIgnore]
        [Description("General Menu BackColor."), Category("Menu")]
        public Color MenuBackColor { get { return StringToColor(MenuBackColorXML); } set { MenuBackColorXML = ColorToString(value); } }
        [Browsable(false)]
        public string MenuBackColorXML { get; set; }

        [XmlIgnore]
        [Description("General Menu ForeColor."), Category("Menu")]
        public Color MenuForeColor { get { return StringToColor(MenuForeColorXML); } set { MenuForeColorXML = ColorToString(value); } }
        [Browsable(false)]
        public string MenuForeColorXML { get; set; }
        #endregion Menu

        #region GridView
        [XmlIgnore]
        [Description("Grid Alternative BackColor."), Category("GridView")]
        public Color GridAlternativeBackColor
        {
            get { return StringToColor(GridAlternativeBackColorXML); }
            set { GridAlternativeBackColorXML = ColorToString(value); }
        }
        [Browsable(false)]
        public string GridAlternativeBackColorXML { get; set; }

        [XmlIgnore]
        [Description("Grid Header BackColor."), Category("GridView")]
        public Color GridHeaderBackColor
        {
            get { return StringToColor(GridHeaderBackColorXML); }
            set { GridHeaderBackColorXML = ColorToString(value); }
        }
        [Browsable(false)]
        public string GridHeaderBackColorXML { get; set; }

        [XmlIgnore]
        [Description("Grid Header ForeColor."), Category("GridView")]
        public Color GridHeaderForeColor
        {
            get { return StringToColor(GridHeaderForeColorXML); }
            set { GridHeaderForeColorXML = ColorToString(value); }
        }
        [Browsable(false)]
        public string GridHeaderForeColorXML { get; set; }
        #endregion GridView

        #region General Font
        [Description("General Font")]
        public string GeneralFont { get; set; }
        [Description("General Font Size")]
        public int GeneralFontSize { get; set; }
        [Description("General Plus Font Size")]
        public int GeneralPlusFontSize { get; set; }
        [Browsable(false)]
        public Font GeneralFontObject { get { return new Font(GeneralFont, GeneralFontSize); } }
        [Browsable(false)]
        public Font GeneralPlusFontObject { get { return new Font(GeneralFont, GeneralPlusFontSize); } }
        #endregion General Font

        #region Application
        [XmlIgnore]
        [Description("Application BackColor."), Category("General")]
        public Color ApplicationBackColor { get { return StringToColor(ApplicationBackColorXML); } set { ApplicationBackColorXML = ColorToString(value); } }
        [Browsable(false)]
        public string ApplicationBackColorXML { get; set; }
        #endregion Application

        #region Button
        [XmlIgnore]
        [Description("Button Color."), Category("Button")]
        public Color ButtonBackColor { get { return StringToColor(ButtonBackColorXML); } set { ButtonBackColorXML = ColorToString(value); } }
        [Browsable(false)]
        public string ButtonBackColorXML { get; set; }

        [Description("Button Font"), Category("Button")]
        public string ButtonFont { get; set; }
        [Description("Button Font Size"), Category("Button")]
        public int ButtonFontSize { get; set; }
        [Browsable(false)]
        public Font ButtonFontObject { get { return new Font(ButtonFont, ButtonFontSize); } }

        [Description("Button Flat Style"), Category("Button")]
        public FlatStyle ButtonFlatStyle { get; set; }


        [XmlIgnore]
        [Description("Button ForeColor."), Category("Button")]
        public Color ButtonForeColor { get { return StringToColor(ButtonForeColorXML); } set { ButtonForeColorXML = ColorToString(value); } }
        [Browsable(false)]
        public string ButtonForeColorXML { get; set; }
        #endregion

        #region Status
        [XmlIgnore]
        [Description("Status BackColor."), Category("Status")]
        public Color StatusBackColor { get { return StringToColor(StatusBackColorXML); } set { StatusBackColorXML = ColorToString(value); } }
        [Browsable(false)]
        public string StatusBackColorXML { get; set; }

        [XmlIgnore]
        [Description("Status Border Color."), Category("Status")]
        public Color StatusBorderColor { get { return StringToColor(StatusBorderColorXML); } set { StatusBorderColorXML = ColorToString(value); } }
        [Browsable(false)]
        public string StatusBorderColorXML { get; set; }

        [XmlIgnore]
        [Description("Status ForeColor."), Category("Status")]
        public Color StatusForeColor { get { return StringToColor(StatusForeColorXML); } set { StatusForeColorXML = ColorToString(value); } }
        [Browsable(false)]
        public string StatusForeColorXML { get; set; }

        [Description("Status border size."), Category("Status")]
        public int StatusBorderSize { get; set; }
        #endregion Status

        #region Splitter
        [XmlIgnore]
        [Description("Splitter Color."), Category("Splitter")]
        public Color SplitterBackColor { get { return StringToColor(SplitterBackColorXML); } set { SplitterBackColorXML = ColorToString(value); } }
        [Browsable(false)]
        public string SplitterBackColorXML { get; set; }

        [XmlIgnore]
        [Description("Splitter Width."), Category("Splitter")]
        public int SplitterWidth { get; set; }

        [XmlIgnore]
        [Description("Highlight Border Color."), Category("Splitter")]
        public Color HighlightBorderBackColor { get { return StringToColor(HighlightBorderBackColorXML); } set { HighlightBorderBackColorXML = ColorToString(value); } }
        [Browsable(false)]
        public string HighlightBorderBackColorXML { get; set; }


        [XmlIgnore]
        [Description("Selected Button BackColor."), Category("Splitter")]
        public Color ButtonSelectedBackColor { get { return StringToColor(ButtonSelectedBackColorXML); } set { ButtonSelectedBackColorXML = ColorToString(value); } }
        [Browsable(false)]
        public string ButtonSelectedBackColorXML { get; set; }
        #endregion Splitter

        [XmlIgnore]
        [Description("Selected Button ForeColor."), Category("Splitter")]
        public Color ButtonSelectedForeColor { get { return StringToColor(ButtonSelectedForeColorXML); } set { ButtonSelectedForeColorXML = ColorToString(value); } }
        [Browsable(false)]
        public string ButtonSelectedForeColorXML { get; set; }
    }
}
