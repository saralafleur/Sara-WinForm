using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Sara.WinForm.MVVM;

namespace Sara.WinForm.CRUD
{
    public static class CrudUiService
    {
        public static void Add<TWindowClass, TCrudModel, TCrudValue>(Action<TCrudModel> saveEvent)
            where TWindowClass : class, IView<TCrudModel, TCrudValue> 
            where TCrudModel : class, ICrudModel<TCrudModel, TCrudValue>
            where TCrudValue : class
        {
            var window = (TWindowClass)Activator.CreateInstance(typeof(TWindowClass));
            var model = (TCrudModel) Activator.CreateInstance(typeof (TCrudModel));
            model.Item = (TCrudValue) Activator.CreateInstance(typeof (TCrudValue));
            model.Mode = InputMode.Add;
            model.SaveEvent = saveEvent;
            window.Render(model);
            window.ShowDialog();
        }

        public static void Edit<TWindowClass, TCrudModel, TCrudValue>(ListBox lb, string selectWarning, Action<TCrudModel> saveEvent)
            where TWindowClass : class, IView<TCrudModel, TCrudValue>
            where TCrudModel : class, ICrudModel<TCrudModel, TCrudValue>
            where TCrudValue : class
        {

            if (lb.SelectedItem == null)
            {
                MessageBox.Show(selectWarning);
                return;
            }

            var window = (TWindowClass)Activator.CreateInstance(typeof(TWindowClass));
            var model = (TCrudModel)Activator.CreateInstance(typeof(TCrudModel));
            model.Item = lb.SelectedItem as TCrudValue;
            model.Mode = InputMode.Edit;
            model.SaveEvent = saveEvent;
            window.Render(model);
            window.ShowDialog();
        }

        public static void Delete(ListBox lb, string selectWarning)
        {
            if (lb.SelectedItem == null)
            {
                MessageBox.Show(selectWarning);
                return;
            }

            if (lb.SelectedItem != null) lb.Items.Remove(lb.SelectedItem);

        }

        /// <summary>
        /// Forces the re-evaulation of ToString()
        /// Calling Refresh will not force the re-evaluate ToString()
        /// </summary>
        /// <param name="listBox"></param>
        private static void UpdateToString(ListBox listBox)
        {
            var count = listBox.Items.Count;
            listBox.SuspendLayout();
            for (var i = 0; i < count; i++)
            {
                listBox.Items[i] = listBox.Items[i];
            }
            listBox.ResumeLayout();
        }

        public static void Save<TCrudModel,TCrudValue>(ListBox lb, ICrudModel<TCrudModel, TCrudValue> model )
        {
            switch (model.Mode)
            {
                case InputMode.Add:
                    lb.Items.Add(model.Item);
                    break;
                case InputMode.Edit:
                    lb.SelectedItem = model.Item;
                    UpdateToString(lb);
                    break;
            }
        }

        public static void SaveList<TValue>(List<TValue> list, ListBox lb)
        {
            list.Clear();
            list.AddRange(lb.Items.Cast<TValue>());
        }

        public static void RenderList<TValue>(List<TValue> list, ListBox lb)
        {
            lb.Items.Clear();
            foreach (var value in list)
            {
                lb.Items.Add(value);
            }

        }

        public static void RenderEnumList<TEnum>(ComboBox cb, object value)
        {
            cb.Items.Clear();
            foreach (var item in Enum.GetNames(typeof(TEnum)).ToList())
            {
                cb.Items.Add(item);
            }
            cb.Text = value.ToString();
        }

        public static T GetEnumValue<T>(ComboBox cb)
        {
            return (T)Enum.Parse(typeof(T), cb.Text, true);
        }
    }
}
