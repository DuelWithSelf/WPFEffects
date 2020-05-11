using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WPFEffects.Core.Common
{
    public class AsynchUtils
    {
        public delegate void AsynchWorkDelegate();
        public delegate void AsynchWorkCompleteDelegate(object obj);

        private const int ThreadCount = 8;
        private static Thread[] WorkThreads; // 线程组
        private static LinkedList<AsynchWorkDelegate>[] ThreadWorkerQueues; // 线程

        static AsynchUtils()
        {
            Init();
        }

        public static void Init()
        {
            if (WorkThreads == null)
            {
                // 创建线程
                WorkThreads = new Thread[ThreadCount];
                ThreadWorkerQueues = new LinkedList<AsynchWorkDelegate>[ThreadCount];

                // 创建链表(链表数与线程数相同)
                for (int i = 0; i < WorkThreads.Length; i++)
                {
                    WorkThreads[i] = new Thread(new ParameterizedThreadStart(AsynchWorker));
                    WorkThreads[i].Name = "Worker Thread: " + i;
                    ThreadWorkerQueues[i] = new LinkedList<AsynchWorkDelegate>();
                    WorkThreads[i].Start(ThreadWorkerQueues[i]);
                }
            }
        }

        public static void Dispose()
        {
            try
            {
                if (WorkThreads != null && WorkThreads.Length > 0)
                {
                    for (int i = 0; i < WorkThreads.Length; i++)
                    {
                        if (WorkThreads[i] != null)
                            WorkThreads[i].Abort();
                    }

                    WorkThreads = null;
                    ThreadWorkerQueues = null;
                }
            }
            catch { }
        }

        // 线程从链表获取需要执行的事件或方法并执行处理
        internal static void AsynchWorker(object oLinkedList)
        {
            var workQueue = (LinkedList<AsynchWorkDelegate>)oLinkedList;
            while (true)
            {
                try
                {
                    AsynchWorkDelegate delegateToExecute = null;
                    Monitor.Enter(workQueue);
                    {
                        if (workQueue.Count == 0)
                            Monitor.Wait(workQueue);
                        delegateToExecute = workQueue.First.Value;
                        workQueue.RemoveFirst();
                    }
                    Monitor.Exit(workQueue);

                    Thread.Sleep(10);

                    if (delegateToExecute != null)
                        delegateToExecute();
                }
                catch
                {
                    break;
                }
            }
        }

        private static void AddToWorkQueue(AsynchWorkDelegate work)
        {
            try
            {
                int min = int.MaxValue;
                LinkedList<AsynchWorkDelegate> minQueue = null;

                // 获取所有链表中，挂载需要执行方法最少的链表和线程
                for (int i = 0; i < ThreadWorkerQueues.Length; i++)
                {
                    if (min > ThreadWorkerQueues[i].Count)
                    {
                        min = ThreadWorkerQueues[i].Count;
                        minQueue = ThreadWorkerQueues[i];
                    }
                }

                // 挂载方法或事件
                if (work != null)
                {
                    Monitor.Enter(minQueue);
                    {
                        minQueue.AddLast(work);
                        Monitor.Pulse(minQueue);
                    }
                    Monitor.Exit(minQueue);
                }
            }
            catch { }
        }

        public static void AsynchExecuteFunc(Dispatcher oDispatcher, AsynchWorkDelegate oEvent)
        {
            AddToWorkQueue(delegate ()
            {
                if (oDispatcher != null && oEvent != null)
                    oDispatcher.BeginInvoke(DispatcherPriority.SystemIdle, oEvent);
            });
        }

        public static void AsynchSleepExecuteFunc(Dispatcher oDispatcher,
            AsynchWorkDelegate oEvent, double dDelayDuration = 0.3)
        {
            AddToWorkQueue(delegate ()
            {
                Thread.Sleep(TimeSpan.FromSeconds(dDelayDuration));

                if (oDispatcher != null && oEvent != null)
                    oDispatcher.BeginInvoke(DispatcherPriority.SystemIdle, oEvent);
            });
        }
    }
}
