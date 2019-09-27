using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Sara.NETFramework.WinForm.Common;

namespace Sara.NETFramework.WinForm.ControlsNS
{
    public class AutoCompleteTextBox : TextBox
    {
        private ListBox _listBox;
        private bool _isAdded;
        private string _formerValue = string.Empty;
        private int _lastIndex;
        private Form _parentForm;
        public List<LookupValue> Values { get; set; }
        /// <summary>
        /// When True a Tab will insert 2 spaces.
        /// </summary>
        public bool TabAs2Spaces { get; set; }
        public AutoCompleteTextBox()
        {
            InitializeComponent();
            ResetListBox();
            TabAs2Spaces = true;
            Multiline = true;
            AcceptsTab = true;
        }

        public sealed override bool Multiline
        {
            get => base.Multiline;
            set => base.Multiline = value;
        }

        private void InitializeComponent()
        {
            _listBox = new ListBox();
            SuspendLayout();
            // 
            // _listBox
            // 
            _listBox.Location = new Point(0, 0);
            _listBox.Name = "_listBox";
            _listBox.Size = new Size(120, 95);
            _listBox.TabIndex = 0;
            // 
            // AutoCompleteTextBox
            // 
            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
            Leave += AutoCompleteTextBox_Leave;
            ResumeLayout(false);

        }
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (_listBox.Visible || 
                (e.KeyCode != Keys.Up &&
                 e.KeyCode != Keys.Down &&
                 e.KeyCode != Keys.Left &&
                 e.KeyCode != Keys.Right))
            UpdateListBox();
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    ResetListBox();
                    break;
                case Keys.Enter:
                case Keys.Tab:
                    if (_listBox.Visible)
                    {

                        var start = GetStartOfCurrentWord();
                        var end = GetEndOfCurrentWord();

                        if (_listBox.SelectedItem is LookupValue item)
                            switch (item.Type)
                            {
                                case ValueType.Keyword:
                                    InsertText(start, end, $"{item}");
                                    break;
                                case ValueType.FileType:
                                    if (item.Name.Contains(" "))
                                        InsertText(start, end, $@"""{item}""");
                                    else
                                        InsertText(start, end, $"{item}");
                                    break;
                                case ValueType.Event:
                                    InsertText(start, end, $@"""{item}""");
                                    break;
                                case ValueType.Value:
                                    break;
                            }

                        _formerValue = Text;
                        ResetListBox();
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                        break;
                    }
                    if (e.KeyCode == Keys.Tab && TabAs2Spaces)
                    {
                        InsertText(SelectionStart, SelectionStart, "  ");

                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                    break;
                case Keys.Down:
                    if (_listBox.Visible)
                    {
                        if ((_listBox.SelectedIndex < _listBox.Items.Count - 1))
                            _listBox.SelectedIndex++;
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                    break;
                case Keys.Up:
                    if (_listBox.Visible)
                    {
                        if (_listBox.SelectedIndex > 0)
                            _listBox.SelectedIndex--;
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                    break;
            }
        }
        private int FindStartOfWord()
        {
            var i = SelectionStart;
            while (Text[i] != '\n' && Text[i] != ' ' && i > 0)
                i--;
            return i;
        }
        /// <summary>
        /// Returns the Index where the Word starts based on SelectionStart
        /// </summary>
        private void InsertText(int start, int end, string value)
        {
            int horzPos, vertPos;
            ScrollApIs.GetScrollPosition(Handle, out horzPos, out vertPos);
            ScrollApIs.LockWindowUpdate(Handle);

            Text = Text.Substring(0, start) +
                   value +
                   Text.Substring(end, Text.Length - end);
            SelectionStart = start + value.Length;

            ScrollApIs.SetScrollPosition(Handle, horzPos, vertPos);
            ScrollApIs.LockWindowUpdate(IntPtr.Zero);
        }
        private int GetStartOfCurrentWord()
        {
           var index = SelectionStart;

            // Walk to the beginning of the word
            while (index >= 1
                   && Text[index - 1] != ' '
                   && Text[index - 1] != '('
                   && Text[index - 1] != '['
                   && Text[index - 1] != '{'
                   && Text[index - 1] != '\n')
                index--;

            return index;
        }
        private int GetEndOfCurrentWord()
        {
            var index = SelectionStart;

            if (index == 0) return 0;

            // Walk to the beginning of the word
            while (index <= Text.Length
                   && Text[index - 1] != ' '
                   && Text[index - 1] != '\r')
                index++;

            return index - 1;
        }
        private string GetWord()
        {
            if (Text.Length == 0)
                return string.Empty;

            var start = GetStartOfCurrentWord();

            var value = Text.Substring(start, TextLength - start);
            var match = Regex.Match(value, @"^([^\s]+)", RegexOptions.IgnoreCase);
            var word = match.Value;
            return word.Trim('"');
        }
        private void UpdateListBox()
        {
            if (Text == _formerValue && SelectionStart == _lastIndex)
                return;

            _formerValue = Text;
            _lastIndex = SelectionStart;
            var word = GetWord();

            if (Values == null || word.Length == 0)
            {
                ResetListBox();
                return;
            }

            var matches = Values.Where(n => (n.Name.ToLower().Contains(word.ToLower())));

            var lookupValues = matches.ToList();
            if (lookupValues.Count() == 0)
            {
                ResetListBox();
                return;
            }

            if (lookupValues.Count() == 1 && lookupValues.First().Name == word)
            {
                ResetListBox();
                return;
            }

            ShowListBox();
            _listBox.BeginUpdate();
            _listBox.Items.Clear();

            foreach (var item in lookupValues.OrderBy(n => n.Name).ToList())
            {
                _listBox.Items.Add(item);
            }

            _listBox.SelectedIndex = 0;
            _listBox.Height = 0;
            _listBox.Width = 0;
            Focus();
            using (var graphics = _listBox.CreateGraphics())
            {
                var width = 0;
                for (var i = 0; i < _listBox.Items.Count; i++)
                {
                    if (i < 20)
                        _listBox.Height += _listBox.GetItemHeight(i);
                    var itemWidth = (int)graphics.MeasureString(_listBox.Items[i] + "_", _listBox.Font).Width;
                    if (itemWidth > width)
                        width = itemWidth;
                }
                _listBox.Width = width;
            }
            _listBox.EndUpdate();
        }
        private void ShowListBox()
        {
            if (!_isAdded)
            {
                _parentForm = FindForm(); // new line added
                _parentForm?.Controls.Add(_listBox); // adds it to the form
                //Parent.Controls.Add(_listBox); // adds it to the form
                _isAdded = true;
            }

            if (!_listBox.Visible)
            {

                Point pt;
                using (Graphics.FromHwnd(Handle))
                {
                    var start = GetStartOfCurrentWord();
                    pt = GetPositionFromCharIndex(start);

                    if (SelectionStart == 0) pt.X = 0;
                }

                _listBox.Left = pt.X + ParentLeft(this);
                _listBox.Top = pt.Y + ParentTop(this); // + Top + 50;//(Font.Height*2) + 6;
            }

            _listBox.Visible = true;
            _listBox.BringToFront();
        }
        private int ParentLeft(Control control)
        {
            if (control.Parent != null && control != _parentForm)
                return control.Left + ParentLeft(control.Parent);
            return control.Left;
        }
        private int ParentTop(Control control)
        {
            if (control.Parent != null && control != _parentForm)
                return control.Top + ParentTop(control.Parent);
            return control.Top;
        }
        private void ResetListBox()
        {
            _listBox.Visible = false;
        }
        private void AutoCompleteTextBox_Leave(object sender, EventArgs e)
        {
            ResetListBox();
        }
    }

    public enum ValueType
    {
        Keyword,
        Event,
        Value,
        FileType
    }

    public class LookupValue
    {
        public ValueType Type { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
