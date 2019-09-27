using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace Sara.NETFramework.WinForm.ColorScheme.Modal
{
    public static class ColorService
    {
        private static ColorSchemeCollection<ColorSchemeBaseModal> _colorScheme;
        public static ColorSchemeCollection<ColorSchemeBaseModal> ColorScheme
        {
            get
            {
                if (_colorScheme != null) return _colorScheme;
                _colorScheme = new ColorSchemeCollection<ColorSchemeBaseModal>();
                _colorScheme.Initialize();
                return _colorScheme;
            }
            set => _colorScheme = value;
        }
        public static Type ColorSchemeType { get; set; }
        public static event Action SaveEvent;

        public static void Save()
        {
            SaveEvent?.Invoke();
            Invalidate();
        }

        /// <summary>
        /// Updates all Controls using the Color Scheme
        /// </summary>
        public static void Invalidate()
        {
            ThreadPool.QueueUserWorkItem(m =>
            {
                if (ColorScheme?.Current == null)
                    return;

                var copy = new List<Control>();
                lock (Controls)
                {
                    copy.AddRange(Controls);
                }

                foreach (var obj in copy)
                {
                    if (obj.InvokeRequired)
                    {
                        obj.Invoke(new Action(() => ColorScheme.Current.Apply(obj)));
                        continue;
                    }
                    ColorScheme.Current.Apply(obj);
                }
            });
        }

        public static void LoadCollection<TItem>(List<TItem> value) where TItem : ColorSchemeBaseModal
        {
            ColorScheme.Collection.Clear();
            foreach (var item in value)
            {
                ColorScheme.Collection.Add(item);
            }
        }

        public static void Apply(object control)
        {
            ColorScheme.Current.Apply(control);
        }

        private static List<Control> _controls;
        private static List<Control> Controls => _controls ?? (_controls = new List<Control>());

        /// <summary>
        /// Adds a Control the the Collection of Controls that consume the Color Scheme
        /// Note: Controls that are added will be updated when the Color Scheme changes
        /// </summary>
        public static void Setup(Control obj)
        {
            lock (Controls)
                Controls.Add(obj);

            ColorScheme?.Current?.Apply(obj);
        }

        internal static void ResetAll()
        {
            ColorScheme = new ColorSchemeCollection<ColorSchemeBaseModal>();
            ColorScheme.Initialize();
        }
    }
}
