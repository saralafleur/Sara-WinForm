using System;
using System.Reflection;
using System.Threading;
using Sara.Common.DateTimeNS;
using Sara.Logging;
using Sara.WinForm.Notification;
using WeifenLuo.WinFormsUI.Docking;

namespace Sara.WinForm.MVVM
{
    public abstract class ViewModelBase<TView, TModel, TModelItem> : IViewModelBase<TModel, TModelItem>
        where TModel : class
        where TModelItem : class
        where TView : IViewDock<TModel, TModelItem>, IDockContent
    {
        #region Properties
        private TView _view;
        public TView View { get => _view;
            set { _view = value; View.Enter += OnEnter; } }
        public IDockContent DockView => View as IDockContent;
        public bool IsClosing { get; set; }
        /// <summary>
        /// True when the View was rendered for the first time
        /// </summary>
        public bool FirstRenderComplete { get; set; }
        public abstract TModel GetModel();
        #endregion

        #region Setup

        protected ViewModelBase()
        {
        }
        #endregion

        #region Render
        public void Ready()
        {
            if (View.Visible)
                RenderDocument();
        }
        public bool IsRenderReady => (!IsClosing && !View.IsDisposed && View.StartupReady);

        /// <summary>
        /// Render will retrieve the model using a thread from the ThreadPool.
        /// Once the model is returned, then Invoke will be used to Render the View on the Main UI Thread.
        /// </summary>
        public virtual void RenderDocument()
        {
            if (!View.Visible) return;
            Render(null);
        }
        public bool IsRendering { get; set; }
        /// <summary>
        /// Render will retrieve the model using a thread from the ThreadPool.
        /// Once the model is returned, then Invoke will be used to Render the View on the Main UI Thread.
        /// If selectedItem is not null, then View.UpdateView will be called to set focus to the selected item.
        /// </summary>
        public void Render(TModelItem selectedItem)
        {
            IsRendering = true;
            // A Windows Control does not recieve a Windows Handle 'Handle' until the Control is 'Show' or the form it belongs too is 'Show'.
            // If you attempt to Invoke on a Windows Control that is not visible yet, the Invoke will fail! - Sara
            // Thus there is no need to retrieve a model and render it if the view itself is not shown by windows !
            if (!View.IsHandleCreated)
            {
                Log.WriteTrace("${GetType().Name} does not have a handle, Windows has not shown the View!", GetType().FullName, MethodBase.GetCurrentMethod().Name);
                return;
            }

            StatusUpdate(StatusModel.Update("Loading"));

            Log.WriteTrace($"Entering {GetType().Name}.Render", GetType().FullName, MethodBase.GetCurrentMethod().Name);

            if (!IsRenderReady)
            {
                IsRendering = false;
                return;
            }

            ThreadPool.QueueUserWorkItem(delegate
            {
                Log.WriteTrace($"Entering Thread for {GetType().Name}.Render", GetType().FullName, MethodBase.GetCurrentMethod().Name);
                Thread.CurrentThread.Name = "ViewModelBase.Render";
                StatusUpdate(StatusModel.Update("Getting Model"));
                StatusUpdate(StatusModel.StartStopWatch);
                var stopwatch = new Stopwatch($"Getting Model for {GetType().Name}");
                var model = GetModel();
                stopwatch.Stop(100);

                Log.WriteTrace($"Thread Invoking {GetType().Name}.Render", GetType().FullName, MethodBase.GetCurrentMethod().Name);
                InternalRender(View, model, selectedItem);
            });
        }
        private void InternalRender(TView view, TModel model, TModelItem selectedItem)
        {
            if (view.InvokeRequired)
            {
                View.Invoke(new Action<TView, TModel, TModelItem>(InternalRender), View, model, selectedItem);
                return;
            }

            Log.WriteTrace($"Inside Invoke for {GetType().Name}.Render", GetType().FullName, MethodBase.GetCurrentMethod().Name);
            view.Render(model);
            if (selectedItem != null)
                View.UpdateView(selectedItem);
            FirstRenderComplete = true;
            IsRendering = false;
        }
        #endregion

        #region Events
        private void OnEnter(object sender, EventArgs e)
        {
            if (!FirstRenderComplete & !IsRendering)
                RenderDocument();
        }

        #endregion

        #region Methods
        public void RenderView(DockPanel dockPanel)
        {
            if (View.InvokeRequired)
            {
                View.Invoke(new Action<DockPanel>(RenderView), dockPanel);
                return;
            }

            var visible = View.Visible;
            View.Show(dockPanel);
            if (!visible)
                RenderDocument();
        }
        public void StatusUpdate(IStatusModel model)
        {
            View.StatusUpdate(model);
        }
        #endregion
    }
}
