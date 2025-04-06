
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using WinFormsApp1.CAN;
using WinFormsApp1.CAN.Enums;
using WinFormsApp1.CAN.Extension;
using WinFormsApp1.Files;
using WinFormsApp1.ZLGCAN;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static WinFormsApp1.Files.MyHex2Bin;
using static WinFormsApp1.ZLGCAN.ZLGCanDevices;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        #region Property
        /// <summary>
        /// zlgcan 设备
        /// </summary>
        private Dictionary<string, DeviceInfo> _zlgCanDevices;
        /// <summary>
        /// 设备句柄
        /// </summary>
        IntPtr _deviceHandle;
        /// <summary>
        /// 设备句柄-通道
        /// </summary>
        IntPtr _channelHandle;
        /// <summary>
        /// 设备状态,默认无
        /// </summary>
        ZLGDeviceRunStatusEnum _zlgDeviceStatus = ZLGDeviceRunStatusEnum.None;
        const uint CAN_EFF_FLAG = 0x80000000U; /* EFF/SFF is set in the MSB */
        const uint CAN_RTR_FLAG = 0x40000000U; /* remote transmission request */
        const uint CAN_ERR_FLAG = 0x20000000U; /* error message frame */
        const uint CAN_ID_FLAG = 0x1FFFFFFFU; /* id */

        /// <summary>
        /// 保存加载到内存的hex文件信息
        /// </summary>
        List<Section> sections = new List<Section>();
        #endregion

        /// <summary>
        /// 要下发的数据
        /// </summary>
        private List<byte> data = new List<byte>();
        /// <summary>
        /// 接收状态
        /// </summary>
        private ReceivedStateEnum recState = ReceivedStateEnum.None;

        OpenFileDialog ofd = new OpenFileDialog();//用于创建文件选择对话框，是用户浏览并选择一个文件

        SynchronizationContext? _synchronizationContext;
        public Form1()
        {
            InitializeComponent();
            LoadData();
            _synchronizationContext = SynchronizationContext.Current;
            this.FormClosing += Form1_FormClosing;
        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            //关闭can设备
            try
            {
                if (_deviceHandle > 0)
                {
                    Method.ZCAN_CloseDevice(_deviceHandle);
                }
            }
            catch (Exception)
            {


            }
        }

        /// <summary>
        /// 数据加载
        /// </summary>
        private void LoadData()
        {
            //获取周立功can设备信息
            _zlgCanDevices = GetZLGCanDevices();
            //添加设备类型
            comboBox_device.Items.AddRange(_zlgCanDevices.Select(s => s.Key).ToArray());

            //添加设备索引
            comboBox_index.Items.AddRange(GetZLGCanIndexListString(0, 30));
            //文件地址文本框禁用
            tb_FilePath.Enabled = false;
            //初始化默认值0
            comboBox_device.SelectedIndex = 0;
            //初始化默认值0
            comboBox_index.SelectedIndex = 0;
        }
        /// <summary>
        /// 设备类型文本改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_device_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var selectedItem = zlgCanDevices.Where(s => s.Key.Equals(comboBox_device.SelectedItem?.ToString() ?? ""));
            var selectedItem = comboBox_device.SelectedItem?.ToString() ?? "";
            if (_zlgCanDevices.ContainsKey(selectedItem))
            {
                comboBox_channel.Items.Clear();
                comboBox_channel.Items.AddRange(GetZLGCanIndexListString(0, (int)_zlgCanDevices[selectedItem].channel_count));
                comboBox_channel.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_OpenFile_Click(object sender, EventArgs e)
        {
            ofd.Multiselect = false;//禁止多文件选择
            ofd.Filter = "hex文件|*.hex";
            //progressBar1.Maximum = 100;//进度条最大值
            var result = ofd.ShowDialog();//弹出文件对话框，让用户浏览并选择文件

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(ofd.FileName))//点击了确定并且文件不为空
            {
                var filePath = ofd.FileName;//存储用户选择文件的完整路径
                tb_FilePath.Text = filePath;

            }
        }
        /// <summary>
        /// 获取文件包对象
        /// </summary>
        /// <returns></returns>
        private FileBinModel CreateFileBinModel()
        {
            var filePath = tb_FilePath.Text;
            if (!File.Exists(filePath))
            {
                throw new Exception("请选择文件");
            }
            ////读取所有hex
            //var bytes = File.ReadAllBytes(fileName);
            sections.Clear();
            if (!MyHex2Bin.LoadHex(Encoding.Default, ref sections, out string log, filePath))
            {
                throw new Exception(log);
            }
            if (sections.Count == 0)
            {
                throw new Exception("请选择文件");
            }
            //if (sections.Count != 1)
            //{
            //    throw new Exception("文件格式不正确");
            //}
            var bytes = MyHex2Bin.WriteBinBytes(0xff, sections);
            FileBinModel binModel = new FileBinModel(bytes);
            binModel.DataPacketInit();
            return binModel;
        }
        /// <summary>
        /// 获取设备类型
        /// </summary>
        /// <returns></returns>
        private uint GetDeviceType()
        {
            var selectedDeviceType = comboBox_device.SelectedItem?.ToString();
            //获取设备类型
            return _zlgCanDevices[selectedDeviceType!].device_type;
        }
        /// <summary>
        /// 获取设备通道
        /// </summary>
        /// <returns></returns>
        private uint GetDeviceChannel()
        {
            return comboBox_channel.SelectedItem.ToUint();
        }
        /// <summary>
        /// 获取设备索引
        /// </summary>
        /// <returns></returns>
        private uint GetDeviceIndex()
        {
            //获取设备索引
            return comboBox_index.SelectedItem.ToUint();
        }
        /// <summary>
        /// 打开设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OpenDevice_Click(object sender, EventArgs e)
        {
            try
            {
                DataCheck();
                OpenDevice();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 打开设备
        /// </summary>
        private void OpenDevice()
        {
            var deviceType = GetDeviceType();
            var deviceIndex = GetDeviceIndex();
            //打开设备
            _deviceHandle = Method.ZCAN_OpenDevice(deviceType, deviceIndex, 0);
            if (ZLGDeviceStatusEnum.Error == (ZLGDeviceStatusEnum)_deviceHandle)
            {
                throw new Exception("打开设备失败,请检查设备类型和设备索引号是否正确");
            }
            //表示设备已打开
            SetZLGDeviceRunStatus(ZLGDeviceRunStatusEnum.OpenedDevice);
        }
        /// <summary>
        /// 周立功设备运行状态
        /// </summary>
        /// <param name="zLGDeviceRunStatus"></param>
        private void SetZLGDeviceRunStatus(ZLGDeviceRunStatusEnum zLGDeviceRunStatus)
        {
            switch (zLGDeviceRunStatus)
            {
                case ZLGDeviceRunStatusEnum.None:
                    btn_OpenDevice.Enabled = true;
                    btn_Connect.Enabled = true;
                    //btn_Program.Enabled = true;
                    break;
                case ZLGDeviceRunStatusEnum.OpenedDevice:
                    btn_OpenDevice.Enabled = false;
                    break;
                case ZLGDeviceRunStatusEnum.ConnectedDevice:
                    btn_OpenDevice.Enabled = false;
                    btn_Connect.Enabled = false;
                    break;
            }
            _zlgDeviceStatus = zLGDeviceRunStatus;
        }
        /// <summary>
        /// 数据检查
        /// </summary>
        /// <exception cref="Exception">异常</exception>
        private void DataCheck()
        {
            if (comboBox_device.SelectedItem == null)
            {
                throw new Exception("设备类型未选择");
            }
            if (comboBox_index.SelectedItem == null)
            {
                throw new Exception("设备索引号未选择");
            }
            if (comboBox_channel == null)
            {
                throw new Exception("设备通道未选择");
            }
        }
        /// <summary>
        /// 关闭设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CloseDevice_Click(object sender, EventArgs e)
        {
            try
            {
                Method.ZCAN_CloseDevice(_deviceHandle);
                SetZLGDeviceRunStatus(ZLGDeviceRunStatusEnum.None);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 连接设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Connect_Click(object sender, EventArgs e)
        {
            try
            {
                //can初始化
                InitCANDevice();
                StartCANDevice();
                SetZLGDeviceRunStatus(ZLGDeviceRunStatusEnum.ConnectedDevice);
                //启动线程检测
                Task.Run(async () =>
                {
                    while (_zlgDeviceStatus ==  ZLGDeviceRunStatusEnum.ConnectedDevice)
                    {
                        await Task.Delay(10);
                        CANDeviceReceived();
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void StartCANDevice()
        {
            if (Method.ZCAN_StartCAN(_channelHandle) != (uint)ZLGDeviceStatusEnum.Success)
            {
                throw new Exception("启动CAN失败");
            }
        }
        /// <summary>
        /// can初始化
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void InitCANDevice()
        {
            var channel = comboBox_channel.SelectedItem.ToInt();
            //设置波特率
            SetBaudRate(1000000, _deviceHandle, channel);
            //can配置信息
            ZCAN_CHANNEL_INIT_CONFIG config = new ZCAN_CHANNEL_INIT_CONFIG();
            config.can.mode = (byte)ZLGDeviceModeEnum.Normal;//设备模式
            config.can_type = Define.TYPE_CAN;// GetDeviceType();//设备类型
            config.can.filter = 0;//过滤器
            config.can.acc_code = 0;
            config.can.acc_mask = 0xFFFFFFFF;
            IntPtr pConfig = Marshal.AllocHGlobal(Marshal.SizeOf(config));
            Marshal.StructureToPtr(config, pConfig, true);
            _channelHandle = Method.ZCAN_InitCAN(_deviceHandle, GetDeviceChannel(), pConfig);
            Marshal.FreeHGlobal(pConfig);
            if (_channelHandle == (uint)ZLGDeviceStatusEnum.Error)
            {
                throw new Exception("初始化can失败");
            }
          
        }
        /// <summary>
        /// can设备接收数据
        /// </summary>
        private void CANDeviceReceived()
        {
            var len = Method.ZCAN_GetReceiveNum(_channelHandle, 0);//can = 0，canfd = 1

            if (len > 0)
            {
                int size = Marshal.SizeOf(typeof(ZCAN_Receive_Data));
                IntPtr ptr = Marshal.AllocHGlobal((int)100 * size);
                len = Method.ZCAN_Receive(_channelHandle, ptr, 100, 50);
                var canData = new ZCAN_Receive_Data[len];
                for (int i = 0; i < len; ++i)
                {
                    var itemData = Marshal.PtrToStructure(
                        (IntPtr)((Int64)ptr + i * size), typeof(ZCAN_Receive_Data));
                    if (itemData != null)
                    {
                        canData[i] = (ZCAN_Receive_Data)itemData;
                    }
                }
                //OnRecvCANDataEvent(can_data, len);
                Marshal.FreeHGlobal(ptr);
                var canDataStr = "";
                for (uint i = 0; i < len; ++i)
                {
                    ZCAN_Receive_Data can = canData[i];
                    uint id = canData[i].frame.can_id;
                    string eff = IsEFF(id) ? "扩展帧" : "标准帧";
                    string rtr = IsRTR(id) ? "远程帧" : "数据帧";
                    //canDataStr += String.Format("接收到CAN ID:0x{0:X8} {1:G} {2:G} 长度:{3:D1} 数据:", GetId(id), eff, rtr, can.frame.can_dlc);
                    //canDataStr += string.Join(" ", can.frame.data.Select(s => s.ToString("X2")));
                    switch (recState)
                    {
                        case ReceivedStateEnum.Ready:
                            if (can.frame.data[7] == 1)
                            {
                                recState = ReceivedStateEnum.Success;
                            }
                            else
                            {
                                recState = ReceivedStateEnum.Fail;
                            }
                            break;
                        case ReceivedStateEnum.HandshakeReady:
                            if (can.frame.data[0]==0xff&&can.frame.data[1] == 0 && can.frame.data[7] ==0x01 )
                            {
                                recState = ReceivedStateEnum.Success;
                            }
                            else
                            {
                                recState = ReceivedStateEnum.Fail;
                            }
                            break;
                    }
                
                }
                _synchronizationContext?.Post(new SendOrPostCallback((obj) =>
                {

                    AddText(canDataStr);
                }), null);
            }
        }
        /// <summary>
        /// 相应等待超时
        /// </summary>
        private async Task<bool> AckTimeOut()
        {
            //等待5s时间
            using var cancelToken = new CancellationTokenSource(50);

            while (!cancelToken.IsCancellationRequested)
            {
                if (recState == ReceivedStateEnum.Success)
                {
                    return true;
                }
                if (recState == ReceivedStateEnum.Fail)
                {
                    return false;
                }
                await Task.Delay(1);
            }
            return false;
        }
        /// <summary>
        /// 添加文本到text
        /// </summary>
        /// <param name="text"></param>
        private void AddText(string text)
        {

            textBox1.Text += text + "\r\n";
        }
        //设置波特率
        private void SetBaudRate(UInt32 baud, IntPtr deviceHandle, int channel)
        {
            string path = channel + "/baud_rate";
            string value = baud.ToString();
            //char* pathCh = (char*)System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(path).ToPointer();
            //char* valueCh = (char*)System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(value).ToPointer();
            var status = Method.ZCAN_SetValue(deviceHandle, path, Encoding.ASCII.GetBytes(value));
            if (status == (uint)ZLGDeviceStatusEnum.Error)
            {
                throw new Exception("波特率设置失败");
            }
        }
        public uint MakeCanId(uint id, int eff, int rtr, int err)//1:extend frame 0:standard frame
        {
            uint ueff = (uint)(!!(Convert.ToBoolean(eff)) ? 1 : 0);
            uint urtr = (uint)(!!(Convert.ToBoolean(rtr)) ? 1 : 0);
            uint uerr = (uint)(!!(Convert.ToBoolean(err)) ? 1 : 0);
            return id | ueff << 31 | urtr << 30 | uerr << 29;
        }

        public bool IsEFF(uint id)//1:extend frame 0:standard frame
        {
            return !!Convert.ToBoolean((id & CAN_EFF_FLAG));
        }

        public bool IsRTR(uint id)//1:remote frame 0:data frame
        {
            return !!Convert.ToBoolean((id & CAN_RTR_FLAG));
        }

        public bool IsERR(uint id)//1:error frame 0:normal frame
        {
            return !!Convert.ToBoolean((id & CAN_ERR_FLAG));
        }

        public uint GetId(uint id)
        {
            return id & CAN_ID_FLAG;
        }

        private async void btn_Program_Click(object sender, EventArgs e)
        {
            try
            {
                var id = 0x1314U;
                if (_zlgDeviceStatus == ZLGDeviceRunStatusEnum.ConnectedDevice)
                {
                    var binModel = CreateFileBinModel();
                    await ZLGDataSend(id, binModel);
                }
                else
                {
                    throw new Exception("请连接设备");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async Task ZLGDataSend(uint id, FileBinModel binModel)
        {
            uint j = 0;
            recState = ReceivedStateEnum.Ready;//准备接收状态
            var HangShakeFrame = new byte[8] { 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            bool isHandShakeOk = false;
            for (j = 0; j < 10; j++)
            {
                await Task.Delay(200);
                recState = ReceivedStateEnum.HandshakeReady;
                await ZLGDataSend(id, [HangShakeFrame]);//发送帧
                if (!await AckTimeOut())//等待响应
                {
                    //throw new Exception("传输失败");
                }
                else
                {
                    isHandShakeOk= true;
                    break;
                }
            }
            if (!isHandShakeOk)
            {
                throw new Exception("再次重试");
            }
            //发送总包-------------
            await ZLGDataSend(id, [binModel.ToBytes()]);//发送帧
            if (!await AckTimeOut())//等待响应
            {
                throw new Exception("传输失败");
            }
            int byteLen = 0;
            foreach (var item in binModel.DataPacketList)
            {
                recState = ReceivedStateEnum.Ready;//准备接收状态
                //发送包-------------
                await ZLGDataSend(id, [item.ToBytes()]);//发送帧
                if (!await AckTimeOut())//等待响应
                {
                    throw new Exception("传输失败");
                }
                foreach (var dataModel in item.DataFrameList)
                {
                    recState = ReceivedStateEnum.Ready;
                    //发送数据-------------
                    await ZLGDataSend(id, [dataModel.ToBytes()]);//发送帧
                    byteLen += 7;
                    pb_Promgram.Value = (int)(1.0 * byteLen / binModel.TotalDataLength * 100);
                }
                if (!await AckTimeOut())//等待响应
                {
                    throw new Exception("传输失败");
                }
                //if (!await AckTimeOut())//等待响应
                //{
                //    throw new Exception("传输失败");
                //}
            }
        }
        /// <summary>
        /// 数据发送
        /// </summary>
        /// <param name="bytes"></param>
        private async Task ZLGDataSend(uint id, IEnumerable<byte[]> bytes)
        {
            var dataLen = bytes.Count();
            ZCAN_Transmit_Data[] can_data = CreateZCAN_Transmit_Datas(id, bytes);
            int size = Marshal.SizeOf(typeof(ZCAN_Transmit_Data));
            IntPtr ptr = Marshal.AllocHGlobal(size * dataLen);

            for (int i = 0; i < dataLen; i++)
            {
                Marshal.StructureToPtr(can_data[i], (IntPtr)(ptr + i * size), true);
            }
            var result = await Task.Run(() => Method.ZCAN_Transmit(_channelHandle, ptr, (uint)dataLen));
            Marshal.FreeHGlobal(ptr);

            if (result != dataLen)
            {
                throw new Exception("发送失败");
            }
        }
        /// <summary>
        /// zlg数据传输
        /// </summary>
        /// <param name="id">发送的id</param>
        /// <param name="bytes">要发送的数组</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private ZCAN_Transmit_Data[] CreateZCAN_Transmit_Datas(uint id, IEnumerable<byte[]> bytes)
        {
            //扩展
            var frameType = ZLGFrameTypeEnum.Extended;
            var dataLen = bytes.Count();
            ZCAN_Transmit_Data[] can_data = new ZCAN_Transmit_Data[dataLen];
            foreach (var item in bytes)
            {
                if (item.Length != 8)
                {
                    throw new ArgumentException("帧长度必须为8");
                }
                can_data[0].frame.can_id = MakeCanId((uint)id, (int)frameType, 0, 0);
                can_data[0].frame.data = item;
                can_data[0].frame.can_dlc = 8;//一次发数据长度
                can_data[0].transmit_type = (uint)0;//正常发送
            }
            return can_data;
        }
    }
}
