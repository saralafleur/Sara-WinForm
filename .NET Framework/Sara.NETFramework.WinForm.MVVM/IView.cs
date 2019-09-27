using System;
using System.Windows.Forms;
using Sara.NETFramework.WinForm.Notification;
using WeifenLuo.WinFormsUI.Docking;

namespace Sara.NETFramework.WinForm.MVVM
{
    public interface IView<in TModel, in TModelItem>
        where TModel : class
        where TModelItem : class
    {
        /// <summary>
        /// WARNING: Never call Render directly, rather call the ViewModel.Render.
        /// This will ensure that the proper logic is executed on each Render. - Sara
        /// </summary>
        void Render(TModel model);
        void StatusUpdate(IStatusModel model);
        DialogResult ShowDialog();
        Cursor Cursor { get; set; }
        bool Visible { get; set; }
        bool IsDisposed { get; }
        /// <summary>
        /// Each MVVM Application has a single MainViewModel.  
        /// Return True when the MainViewModel is ready for other Views to Render.
        /// </summary>
        bool StartupReady { get; }
        bool InvokeRequired { get; }

        bool IsHandleCreated { get; }

        object Invoke(Delegate method);
        object Invoke(Delegate method, params object[] args);
        /// <summary>
        /// Used to Update the View with the current selected TModelItem
        /// </summary>
        void UpdateView(TModelItem selectedModel);
        event EventHandler Enter;
    }

    /// <summary>
    /// Used by windows that are Dock-able
    /// </summary>
    public interface IViewDock<in TModel, in TModelItem> : IView<TModel, TModelItem>
        where TModel : class
        where TModelItem : class
    {
        void Show(DockPanel dockPanel);
    }
}
