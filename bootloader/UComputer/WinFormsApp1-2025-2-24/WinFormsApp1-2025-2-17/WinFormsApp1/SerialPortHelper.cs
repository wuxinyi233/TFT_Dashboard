using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.IO.Ports;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    /// <summary>
    /// 串口帮助函数
    /// </summary>
    public class SerialPortHelper:IDisposable
    {
        SerialPort _serialPort;
        /// <summary>
        /// 是否允许事件接收
        /// </summary>
        bool isReveivedEvent = true;
        /// <summary>
        /// 接收的数据存储
        /// </summary>
        private List<byte> recData = new List<byte>();
        /// <summary>
        /// 接收数据委托
        /// </summary>
        public Func<byte[],bool> DataRecAction;
        //public SerialPortHelper(SerialPort serialPort) 
        //{
        //    _serialPort = serialPort;
        //}
        public SerialPortHelper(string portName, int baudRate)
        {
            _serialPort = new SerialPort(portName, baudRate);
            _serialPort.DataReceived += _serialPort_DataReceived;
        }
        /// <summary>
        /// 串口接收事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if(isReveivedEvent)
            {
                if(!_serialPort.IsOpen)
                {
                    return;
                }
                var readLen = _serialPort.BytesToRead;
                lock(this)
                {
                    if (readLen > 0)
                    {
                        byte[] bytes = new byte[readLen];
                        _serialPort.Read(bytes, 0, readLen);
                        recData.AddRange(bytes);
                        if (DataRecAction.Invoke(recData.ToArray()))//如果接收数据false,则将当前的数据添加到list
                        {
                            recData.Clear();//如果接收数据ok,则清除当前数据
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _serialPort.Close();
            _serialPort.Dispose();
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="bytes"></param>
        public void SendData(byte[] bytes)
        {
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
            }
            _serialPort.Write(bytes, 0, bytes.Length);
        }
        /// <summary>
        /// 异步读取消息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="timeOut">超时时间</param>
        /// <returns></returns>
        public async Task<byte[]> ReadData(int readLength,int timeOut, CancellationToken token)
        {
            isReveivedEvent = false;
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
            }
            //延时取消令牌,默认timeOut 时间
            using var delayToken = new CancellationTokenSource(timeOut);
            var buffer = new byte[1024];
            var readCancelToken = CancellationTokenSource.CreateLinkedTokenSource(delayToken.Token, token);
            var readBytes = new List<byte>();
            var resultCount = 0;
            while (!delayToken.IsCancellationRequested)
            {
                resultCount = await ReadAsync(_serialPort, buffer, 0, buffer.Length, readCancelToken.Token);
                readBytes.AddRange(buffer.Take(resultCount));
                if (delayToken.IsCancellationRequested)
                {
                    break;
                }
                if (readLength == readBytes.Count)
                {
                    break;
                }
            }
            isReveivedEvent = true;
            return readBytes.ToArray();
        }
        /// <summary>
        /// 异步读取数据
        /// </summary>
        /// <param name="stream">数据流</param>
        /// <param name="buffer">缓冲区</param>
        /// <param name="index">开始读取下标</param>
        /// <param name="length">读取的长度</param>
        /// <param name="token">取消令牌</param>
        /// <returns></returns>
        protected async Task<int> ReadAsync(SerialPort port, byte[] buffer, int index, int length, CancellationToken token)
        {
            //using (var cancellationTokenSource = new CancellationTokenSource(100))
            {
                int receivedCount = 0;
                try
                {
                    port.ReadTimeout = 1000;
                    await Task.Run(() =>
                    {
                        while (!token.IsCancellationRequested)
                        {
                            if (port.BytesToRead > 0)
                            {
                                receivedCount =port.Read(buffer, index, length);
                                break;
                            }
                        }
                    }, token);
                }
                catch (TimeoutException e)
                {
                    receivedCount = -1;
                }
                catch (Exception ex)
                {
                    receivedCount = -1;
                }

                return receivedCount;
            }
        }
    }
}
