using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace uEN.Core
{
    // .net 4.0 対応。 .net4.5からは async await で簡単に構築できる
    public sealed class AsyncAwaitAdapter<InT, OutT>
    {
        public AsyncAwaitAdapter(Func<AsyncAwaitNotifier<InT>, OutT> workAction, Action<object> reportAction,
            Action<Exception, OutT> completeAction, InT userState)
        {
            WorkAction = workAction;
            ReportAction = reportAction;
            CompleteAction = completeAction;
            UserState = userState;
        }
        public event EventHandler<System.EventArgs> Completed;
        public Func<AsyncAwaitNotifier<InT>, OutT> WorkAction { get; private set; }
        public Action<object> ReportAction { get; private set; }
        public Action<Exception, OutT> CompleteAction { get; private set; }
        public InT UserState { get; private set; }
        Exception taskException;
        Dictionary<string, string> dic = new Dictionary<string,string>();
        public void RunWorkerAsync()
        {
            foreach (var each in BizUtils.AdditionalInfo.Keys)
            {
                dic[each] = BizUtils.AdditionalInfo[each];
            }

            var worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += OnWorkRaw;
            worker.ProgressChanged += OnProgressChangedRaw;
            worker.RunWorkerCompleted += OnRunWorkerCompletedRaw;
            worker.RunWorkerAsync(this);
        }
        private void OnWorkRaw(object sender, DoWorkEventArgs e)
        {
            foreach (var each in dic.Keys)
            {
                BizUtils.AdditionalInfo[each] = dic[each];
            }

            var worker = (BackgroundWorker)sender;
            var adapter = (AsyncAwaitAdapter<InT, OutT>)e.Argument;
            var reporter = new AsyncAwaitNotifier<InT>(worker, adapter.UserState, adapter);

            object ret = null;
            try
            {
                ret = adapter.WorkAction(reporter);
            }
            catch (Exception ex)
            {
                adapter.taskException = ex;
            }
            e.Result = Tuple.Create<object, object>(ret, adapter);
        }
        private void OnProgressChangedRaw(object sender, ProgressChangedEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            var tuple = (Tuple<object, object>)e.UserState;
            var notifyItem = tuple.Item1;
            var adapter = (AsyncAwaitAdapter<InT, OutT>)tuple.Item2;
            adapter.ReportAction(notifyItem);
        }
        private void OnRunWorkerCompletedRaw(object sender, RunWorkerCompletedEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            var tuple = (Tuple<object, object>)e.Result;
            var userResult = (OutT)tuple.Item1;
            var adapter = (AsyncAwaitAdapter<InT, OutT>)tuple.Item2;
            adapter.CompleteAction(taskException, userResult);
            worker.Dispose();
            adapter.WorkAction = null;
            adapter.ReportAction = null;
            adapter.CompleteAction = null;

            if (Completed != null)
                Completed(this, new EventArgs());
        }
    }
    public sealed class AsyncAwaitNotifier<T>
    {
        internal AsyncAwaitNotifier(BackgroundWorker worker, T userState, object adapter)
        {
            Worker = worker;
            UserState = userState;
            Adapter = adapter;
        }
        internal BackgroundWorker Worker { get; private set; }
        internal object Adapter { get; private set; }
        public T UserState { get; private set; }
        public void Notify(object notifyItem)
        {
            Worker.ReportProgress(0, Tuple.Create<object,object>(notifyItem, Adapter));
        }
    }
}
