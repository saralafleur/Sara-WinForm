namespace Sara.NETFramework.WinForm.Common
{
    /// <summary>
    /// Used by the Toolbox to show checked and unchecked items. - Sara LaFleur
    /// </summary>
    public class CheckedItem
    {
        public bool Checked { get; set; }
        public bool PriorChecked { get; set; }

        public bool Changed => Checked != PriorChecked;
    }

}
