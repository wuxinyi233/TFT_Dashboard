using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    internal class AwaiterOperation
    {
        private readonly Awaiter _awaiter;

        public AwaiterOperation()
        {
            _awaiter = new Awaiter();
        }
        //报告完成
        public void ReportCompleted()
        {
            _awaiter.ReportCompleted();
        }
        /// <summary>
        /// 返回一个可等待对象，以便能够使用 await 关键字进行异步等待。
        /// </summary>
        public Awaiter GetAwaiter()
        {
            return _awaiter;
        }
    }
}
