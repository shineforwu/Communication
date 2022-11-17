using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Modbus.Device;
using System.Net.Sockets;
using System.Threading;
using System.IO.Ports;
using System.Drawing.Text;
using System.Windows.Forms.VisualStyles;
using System.Timers;
using System.CodeDom.Compiler;

namespace ModbusRtuMaster
{
    public partial class Form1 : Form
    {
        #region 参数配置
        private static IModbusMaster master;
        private static SerialPort port;
        //写线圈或写寄存器数组
        private bool[] coilsBuffer;
        private ushort[] registerBuffer;
        //功能码
        private string functionCode;
        //功能码序号
        private int functionOder;
        //参数(分别为从站地址,起始地址,长度)
        private byte slaveAddress;
        private ushort startAddress;
        private ushort numberOfPoints;
        //串口参数
        private string portName;
        private int baudRate;
        private Parity parity;
        private int dataBits;
        private StopBits stopBits;
        //自动测试标志位
        private bool AutoFlag = false;
        //获取当前时间
        private System.DateTime Current_time;

        //定时器初始化
        private System.Timers.Timer t = new System.Timers.Timer(1000);
        
        private const int WM_DEVICE_CHANGE = 0x219;            //设备改变           
        private const int DBT_DEVICEARRIVAL = 0x8000;          //设备插入
        private const int DBT_DEVICE_REMOVE_COMPLETE = 0x8004; //设备移除

        #endregion


        public Form1()
        {
            InitializeComponent();
            GetSerialLstTb1();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //界面初始化
            //cmb_portname.SelectedIndex = 0;
            cmb_baud.SelectedIndex = 5;
            cmb_parity.SelectedIndex = 2;
            cmb_databBits.SelectedIndex = 1;
            cmb_stopBits.SelectedIndex = 0;

        }

        #region 定时器
        //定时器初始化,失能状态
        private void init_Timer()
        {
            t.Elapsed += new System.Timers.ElapsedEventHandler(Execute);
            t.AutoReset = true;//设置false定时器执行一次，设置true定时器一直执行
            t.Enabled = false;//定时器使能true，失能false
            //t.Start();
        }

        private void Execute(object source,System.Timers.ElapsedEventArgs e)
        {
            //停止定时器后再打开定时器，避免重复打开
            t.Stop();
            //ExecuteFunction();可添加执行操作
            t.Start();
        }
        #endregion

        #region 串口配置
        /// <summary>
        /// 串口参数获取
        /// </summary>
        /// <returns></返回串口配置参数>
        private SerialPort InitSerialPortParameter()
        {
            if (cmb_portname.SelectedIndex < 0 || cmb_baud.SelectedIndex < 0 || cmb_parity.SelectedIndex < 0 || cmb_databBits.SelectedIndex < 0 || cmb_stopBits.SelectedIndex < 0)
            {
                MessageBox.Show("请选择串口参数");
                return null;
            }
            else
            {
                portName = cmb_portname.SelectedItem.ToString();
                baudRate = int.Parse(cmb_baud.SelectedItem.ToString());

                switch (cmb_parity.SelectedItem.ToString())
                {
                    case "奇":
                        parity = Parity.Odd;
                        break;
                    case "偶":
                        parity = Parity.Even;
                        break;
                    case "无":
                        parity = Parity.None;
                        break;
                    default:
                        break;
                }
                dataBits = int.Parse(cmb_databBits.SelectedItem.ToString());
                switch (cmb_stopBits.SelectedItem.ToString())
                {
                    case "1":
                        stopBits = StopBits.One;
                        break;
                    case "2":
                        stopBits = StopBits.Two;
                        break;
                    default:
                        break;
                }

                port = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
                return port;

            }
        }
        #endregion

        #region 串口收/发
        private async void ExecuteFunction()
        {
            Current_time = System.DateTime.Now;
            try
            {
                
                if (port.IsOpen == false)
                {
                    port.Open();
                }
                if (functionCode != null)
                {
                    switch (functionCode)
                    {
                        case "01 Read Coils"://读取单个线圈
                            SetReadParameters();
                            try
                            {
                                coilsBuffer = master.ReadCoils(slaveAddress, startAddress, numberOfPoints);
                            }
                            catch(Exception)
                            {
                                MessageBox.Show("参数配置错误");
                                //MessageBox.Show(e.Message);
                                AutoFlag = false;
                                break;
                            }
                            SetMsg("[" + Current_time.ToString("yyyy-MM-dd HH:mm:ss" + "]" + " "));
                            for (int i = 0; i < coilsBuffer.Length; i++)
                            {
                                SetMsg(coilsBuffer[i] + " ");
                            }
                            SetMsg("\r\n");
                            break;
                        case "02 Read DisCrete Inputs"://读取输入线圈/离散量线圈
                            SetReadParameters();
                            try
                            {
                                coilsBuffer = master.ReadInputs(slaveAddress, startAddress, numberOfPoints);
                            }
                            catch(Exception)
                            {
                                MessageBox.Show("参数配置错误");
                                AutoFlag = false;
                                break;
                            }
                            SetMsg("[" + Current_time.ToString("yyyy-MM-dd HH:mm:ss" + "]" + " "));
                            for (int i = 0; i < coilsBuffer.Length; i++)
                            {
                                SetMsg(coilsBuffer[i] + " ");
                            }
                            SetMsg("\r\n");
                            break;
                        case "03 Read Holding Registers"://读取保持寄存器
                            SetReadParameters();
                            try
                            {
                                registerBuffer = master.ReadHoldingRegisters(slaveAddress, startAddress, numberOfPoints);
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("参数配置错误");
                                AutoFlag = false;
                                break;
                            }
                            SetMsg("[" + Current_time.ToString("yyyy-MM-dd HH:mm:ss" + "]" + " "));
                            for (int i = 0; i < registerBuffer.Length; i++)
                            {
                                SetMsg(registerBuffer[i] + " ");
                            }
                            SetMsg("\r\n");
                            break;
                        case "04 Read Input Registers"://读取输入寄存器
                            SetReadParameters();
                            try
                            {
                                registerBuffer = master.ReadInputRegisters(slaveAddress, startAddress, numberOfPoints);
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("参数配置错误");
                                AutoFlag = false;
                                break;
                            }
                            SetMsg("[" + Current_time.ToString("yyyy-MM-dd HH:mm:ss" + "]" + " "));
                            for (int i = 0; i < registerBuffer.Length; i++)
                            {
                                SetMsg(registerBuffer[i] + " ");
                            }
                            SetMsg("\r\n");
                            break;
                        case "05 Write Single Coil"://写单个线圈
                            SetWriteParametes();
                            await master.WriteSingleCoilAsync(slaveAddress, startAddress, coilsBuffer[0]);
                            break;
                        case "06 Write Single Registers"://写单个输入线圈/离散量线圈
                            SetWriteParametes();
                            await master.WriteSingleRegisterAsync(slaveAddress, startAddress, registerBuffer[0]);
                            break;
                        case "0F Write Multiple Coils"://写一组线圈
                            SetWriteParametes();
                            await master.WriteMultipleCoilsAsync(slaveAddress, startAddress, coilsBuffer);
                            break;
                        case "10 Write Multiple Registers"://写一组保持寄存器
                            SetWriteParametes();
                            await master.WriteMultipleRegistersAsync(slaveAddress, startAddress, registerBuffer);
                            break;
                        default:
                            break;
                    }

                }
                else
                {
                    MessageBox.Show("请选择功能码!");
                }
                port.Close();
            }
            catch (Exception ex)
            {
                port.Close();
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 设置读参数
        /// </summary>
        private void SetReadParameters()
        {
            if (txt_startAddr1.Text == "" || txt_slave1.Text == "" || txt_length.Text == "")
            {
                MessageBox.Show("请填写读参数!");
            }
            else
            {
                slaveAddress = byte.Parse(txt_slave1.Text);
                startAddress = ushort.Parse(txt_startAddr1.Text);
                numberOfPoints = ushort.Parse(txt_length.Text);
            }
        }

        /// <summary>
        /// 设置写参数
        /// </summary>
        private void SetWriteParametes()
        {
            if (txt_startAddr2.Text == "" || txt_slave2.Text == "" || txt_data.Text == "")
            {
                MessageBox.Show("请填写写参数!");
            }
            else
            {
                slaveAddress = byte.Parse(txt_slave2.Text);
                startAddress = ushort.Parse(txt_startAddr2.Text);
                //判断是否写线圈
                if (functionOder == 4 || functionOder == 6)
                {
                    string[] strarr = txt_data.Text.Split(' ');
                    coilsBuffer = new bool[strarr.Length];
                    //转化为bool数组
                    for (int i = 0; i < strarr.Length; i++)
                    {
                        // strarr[i] == "0" ? coilsBuffer[i] = false : coilsBuffer[i] = true;
                        if (strarr[i] == "0")
                        {
                            coilsBuffer[i] = false;
                        }
                        else
                        {
                            coilsBuffer[i] = true;
                        }
                    }
                }
                else
                {
                    //转化ushort数组
                    string[] strarr = txt_data.Text.Split(' ');
                    registerBuffer = new ushort[strarr.Length];
                    for (int i = 0; i < strarr.Length; i++)
                    {
                        registerBuffer[i] = ushort.Parse(strarr[i]);
                    }
                }
            }
        }

        /// <summary>
        /// 创建委托，打印日志
        /// </summary>
        /// <param name="msg"></param>
        public void SetMsg(string msg)
        {
            richTextBox1.Invoke(new Action(() => { richTextBox1.AppendText(msg); }));
        }

        /// <summary>
        /// 清空日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        /// <summary>
        /// 单击button1事件，串口完成一次读/写操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //AutoFlag = false;
            //button_AutomaticTest.Enabled = true;

            try
            {
                //初始化串口参数
                InitSerialPortParameter();
            
                master = ModbusSerialMaster.CreateRtu(port);
            
            
                ExecuteFunction();
            
            }
            catch (Exception)
            {
                MessageBox.Show("初始化异常");
            }
        }

        /// <summary>
        /// 自动测试初始化
        /// </summary>
        private void AutomaticTest()
        {
            AutoFlag = true;
            button1.Enabled = false;

            InitSerialPortParameter();
            master = ModbusSerialMaster.CreateRtu(port);

            Task.Factory.StartNew(() =>
            {
                //初始化串口参数
                
                while (AutoFlag)
                {
                    
                    try
                    {

                        ExecuteFunction();
                    
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("初始化异常");
                    }
                    Thread.Sleep(500);
                }
            });
        }

        /// <summary>
        /// 读取数据时，失能写数据；写数据时，失能读数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 4)
            {
                groupBox2.Enabled = true;
                groupBox1.Enabled = false;
            }
            else
            {
                groupBox1.Enabled = true;
                groupBox2.Enabled = false;
            }
            //委托事件,在主线程中创建的控件，在子线程中读取设置控件的属性会出现异常，使用Invoke方法可以解决
            comboBox1.Invoke(new Action(() => { functionCode = comboBox1.SelectedItem.ToString(); functionOder = comboBox1.SelectedIndex; }));
        }

        /// <summary>
        /// 将打印日志显示到最新接收到的符号位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            this.richTextBox1.SelectionStart = int.MaxValue;
            this.richTextBox1.ScrollToCaret();
        }

        /// <summary>
        /// 自动化测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_AutomaticTest_Click(object sender, EventArgs e)
        {
            AutoFlag = false;
            button_AutomaticTest.Enabled = false; //自动收发按钮失能，避免从复开启线程
            if (AutoFlag == false)
            {
                AutomaticTest();
                
            }
            
        }

        /// <summary>
        /// 串口关闭，停止读/写
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_ClosePort_Click(object sender, EventArgs e)
        {
            AutoFlag = false;
            button1.Enabled = true;
            button_AutomaticTest.Enabled = true;
            t.Enabled = false;//失能定时器

            if (port.IsOpen)
            {
                port.Close();
            }

        }

        #region 串口下拉列表刷新
        /// <summary>
        /// 刷新下拉列表显示
        /// </summary>
        private void GetSerialLstTb1()
        {
            //清除cmb_portname显示
            cmb_portname.SelectedIndex = -1;
            cmb_portname.Items.Clear();
            //获取串口列表
            string[] serialLst = SerialPort.GetPortNames();
            if (serialLst.Length > 0)
            {
                //取串口进行排序
                Array.Sort(serialLst);
                //将串口列表输出到cmb_portname
                cmb_portname.Items.AddRange(serialLst);
                cmb_portname.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)                                  //判断消息类型
            {
                case WM_DEVICE_CHANGE:                      //设备改变消息
                    {
                        GetSerialLstTb1();                  //设备改变时重新花去串口列表
                    }
                    break;
            }
            base.WndProc(ref m);
        }
        #endregion

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void txt_slave1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void txt_startAddr1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void txt_length_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
