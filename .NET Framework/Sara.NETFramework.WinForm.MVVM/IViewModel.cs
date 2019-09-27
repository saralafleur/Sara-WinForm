using Sara.NETFramework.WinForm.Notification;
using WeifenLuo.WinFormsUI.Docking;

namespace Sara.NETFramework.WinForm.MVVM
{
    public interface IViewModelBase<out TModel, in TModelItem>
        where TModel : class
        where TModelItem : class
    {
        void RenderView(DockPanel dockPanel);
        /// <summary>
        /// True when the Application is Closing
        /// </summary>
        bool IsClosing { get; set; }
        /// <summary>
        ///  Called when the system has completed loading and is ready to display
        /// </summary>
        void Ready();
        void RenderDocument();
        void Render(TModelItem selectedItem);
        void StatusUpdate(IStatusModel model);
        TModel GetModel();
    }

    public interface IViewModelBaseNonGeneric
    {
        void RenderDocument();
        /// <summary>
        /// True when the Application is Closing
        /// </summary>
        bool IsClosing { get; set; }
        IDockContent DockView { get; }
    }
}
