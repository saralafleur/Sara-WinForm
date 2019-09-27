namespace Sara.NETFramework.WinForm.Notification
{
    public enum StatusAction
    {
        Updating,
        Complete,
        StartBackground,
        EndBackground,
        Error,
        StartStopWatch,
        StopStopWatch,
        StartCountdown,
        StopCountdown,
        ShowEstimatedTime,
        HideEstimatedTime,
        FadeOutUpdate,
        AddPersistentDetail,
        ClearPersistentDetail,
        FullScreenOff,
        FullScreenOn
    }

    public interface IStatusModel
    {
        StatusAction Action { get; set; }
        string Status { get; set; }
        string StatusDetail { get; set; }
        /// <summary>
        /// Total number of items being processed
        /// </summary>
        int Total { get; set; }
        /// <summary>
        /// Current item being processed
        /// </summary>
        int Current { get; set; }
        /// <summary>
        /// Countdown in seconds
        /// </summary>
        int CountDown { get; set; }
    }

    public class StatusModel : IStatusModel
    {
        public StatusAction Action { get; set; }
        public string Status { get; set; }
        /// <summary>
        /// Countdown in seconds
        /// </summary>
        public int CountDown { get; set; }
        public string StatusDetail { get; set; }
        /// <summary>
        /// Total number of items being processed
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// Current item being processed
        /// </summary>
        public int Current { get; set; }

        /// <summary>
        /// Returns a Completed IStatusModel
        /// </summary>
        public static IStatusModel Completed => new StatusModel { Action = StatusAction.Complete };

        public static IStatusModel FullScreenOn => new StatusModel { Action = StatusAction.FullScreenOn };

        public static IStatusModel FullScreenOff => new StatusModel { Action = StatusAction.FullScreenOff };

        /// <summary>
        /// Increase the background thread count
        /// The system will not hide the Status Panel until the background thread count is zero by using EndBackground
        /// </summary>
        public static IStatusModel StartBackground => new StatusModel { Action = StatusAction.StartBackground };

        /// <summary>
        /// Decrease the background thread count
        /// The system will not hide the Status Panel until the background thread count is zero
        /// </summary>
        public static IStatusModel EndBackground => new StatusModel { Action = StatusAction.EndBackground };

        /// <summary>
        /// Starts the Stop Watch
        /// Displays how much time has occured form when an event started
        /// </summary>
        public static IStatusModel StartStopWatch => new StatusModel { Action = StatusAction.StartStopWatch };

        /// <summary>
        /// Starts the Stop Watch
        /// Displays how much time has occured form when an event started
        /// </summary>
        public static IStatusModel StartCountdown(int seconds)
        {
            return new StatusModel { Action = StatusAction.StartCountdown, CountDown = seconds };
        }

        /// <summary>
        /// Stops the Stop Watch
        /// </summary>
        public static IStatusModel StopStopWatch => new StatusModel { Action = StatusAction.StopStopWatch };

        public static IStatusModel StopCountdown => new StatusModel { Action = StatusAction.StopCountdown };

        public static IStatusModel ShowEstimatedTime => new StatusModel { Action = StatusAction.ShowEstimatedTime };

        public static IStatusModel HideEstimatedTime => new StatusModel { Action = StatusAction.HideEstimatedTime };

        public static IStatusModel AddPersistentDetail(string status)
        {
            return new StatusModel { Action = StatusAction.AddPersistentDetail, StatusDetail = status };
        }
        public static IStatusModel AddPersistentDetail(string status, params object[] args)
        {
            return AddPersistentDetail(string.Format(status, args));
        }
        public static IStatusModel ClearPersistentDetail => new StatusModel { Action = StatusAction.ClearPersistentDetail };

        /// <summary>
        /// Used to quickly send a status update
        /// </summary>
        public static IStatusModel Update(string status)
        {
            return Update(status, "");
        }

        public static IStatusModel UpdateDetail(string detail)
        {
            return new StatusModel
            {
                Action = StatusAction.Updating,
                Status = null,
                StatusDetail = detail,
                Total = 0,
                Current = 0
            };
        }

        /// <summary>
        /// Used to quickly send a status update with detail
        /// </summary>
        public static IStatusModel Update(string status, string statusDetail)
        {
            return Update(status, statusDetail, 0, 0);
        }
        /// <summary>
        /// Used to quickly send a status update with detail
        /// </summary>
        public static IStatusModel Update(string status, string statusDetail, int total, int current)
        {
            return Update(status, statusDetail, total, current, StatusAction.Updating);
        }
        /// <summary>
        /// Used to quickly send a status update with detail
        /// </summary>
        public static IStatusModel FadeOutUpdate(string status)
        {
            return Update(status, null, 0, 0, StatusAction.FadeOutUpdate);
        }
        /// <summary>
        /// Used to quickly send a status update with detail
        /// </summary>
        public static IStatusModel Update(string status, string statusDetail, int total, int current, StatusAction action)
        {
            //Log.Write(typeof(StatusModel).FullName, MethodBase.GetCurrentMethod().Name, LogEntryType.Trace, string.Format("{0}{1}{2}",
            //    status == null ? "" : string.Format("Status: {0}", status),
            //    status != null && statusDetail != null ? ", " : "",
            //    statusDetail == null ? "" : string.Format("Status Detail: {0}", statusDetail)));

            return new StatusModel
            {
                Action = action,
                Status = status,
                StatusDetail = statusDetail,
                Total = total,
                Current = current
            };
        }

        public static IStatusModel Error(string status)
        {
            var model = Update(status, null);
            model.Action = StatusAction.Error;
            return model;
        }
    }
}
