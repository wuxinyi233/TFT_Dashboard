using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public class Awaiter : INotifyCompletion
    {
        private Action _continuation;
        public bool IsCompleted { get; private set; }

        public void GetResult()
        {
            // 这个函数我们暂时还没有真正实现，因为需要进行同步等待比较复杂。
            // 我们将在本文后面附的其他博客中实现。
        }

        public void OnCompleted(Action continuation)
        {
            // 当这个 Awaiter 被 await 等待的时候，此代码会被调用。
            // 每有一处 await 执行到，这里就会执行一次，所以在任务完成之前我们需要 +=。
            if (IsCompleted)
            {
                continuation?.Invoke();
            }
            else
            {
                _continuation += continuation;
            }
        }

        public void ReportCompleted()
        {
            // 由 WalterlvOperation 来通知这个任务已经完成。
            IsCompleted = true;
            var continuation = _continuation;
            _continuation = null;
            continuation?.Invoke();
        }
    }
}
