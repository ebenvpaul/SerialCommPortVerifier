using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;

//CodeProjectSerialComms program 
//23/04/2013   16:29

namespace CodeProjectSerialComms
{
    public partial class Form1 : Form
    {
        SerialPort ComPort = new SerialPort();
        DirAppend Log = new DirAppend();
        internal delegate void SerialDataReceivedEventHandlerDelegate(object sender, SerialDataReceivedEventArgs e);
        internal delegate void SerialPinChangedEventHandlerDelegate(object sender, SerialPinChangedEventArgs e);
        private SerialPinChangedEventHandler SerialPinChangedEventHandler1;
        delegate void SetTextCallback(string text);
        string InputData = String.Empty;
        System.Timers.Timer timer;
        int autoPollingTime = 2000; 
        public Form1()
        {
            InitializeComponent();
            SerialPinChangedEventHandler1 = new SerialPinChangedEventHandler(PinChanged);
            ComPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(port_DataReceived_1);
        }
     
        private void btnGetSerialPorts_Click(object sender, EventArgs e)
        {
            string[] ArrayComPortsNames = null;
            int index = -1;
            string ComPortName = null;
           
//Com Ports
            ArrayComPortsNames = SerialPort.GetPortNames();
            do
            {
                index += 1;
                cboPorts.Items.Add(ArrayComPortsNames[index]);
               
              
            } while (!((ArrayComPortsNames[index] == ComPortName) || (index == ArrayComPortsNames.GetUpperBound(0))));
            Array.Sort(ArrayComPortsNames);
           
            if (index == ArrayComPortsNames.GetUpperBound(0))
            {
                ComPortName = ArrayComPortsNames[0];
            }
            //get first item print in text
            cboPorts.Text = ArrayComPortsNames[0];
            //Baud Rate
            cboBaudRate.Items.Add(9600);
            cboBaudRate.Items.Add(300);
            cboBaudRate.Items.Add(600);
            cboBaudRate.Items.Add(1200);
            cboBaudRate.Items.Add(2400);
            cboBaudRate.Items.Add(14400);
            cboBaudRate.Items.Add(19200);
            cboBaudRate.Items.Add(38400);
            cboBaudRate.Items.Add(57600);
            cboBaudRate.Items.Add(115200);
            cboBaudRate.Items.ToString();
            //get first item print in text
            cboBaudRate.Text = cboBaudRate.Items[0].ToString(); 
//Data Bits
            
            cboDataBits.Items.Add(8);
            cboDataBits.Items.Add(7);
            //get the first item print it in the text 
            cboDataBits.Text = cboDataBits.Items[0].ToString();
           
//Stop Bits
            cboStopBits.Items.Add("One");
            cboStopBits.Items.Add("OnePointFive");
            cboStopBits.Items.Add("Two");
            //get the first item print in the text
            cboStopBits.Text = cboStopBits.Items[0].ToString();
//Parity 
            cboParity.Items.Add("None");
            cboParity.Items.Add("Even");
            cboParity.Items.Add("Mark");
            cboParity.Items.Add("Odd");
            cboParity.Items.Add("Space");
            //get the first item print in the text
            cboParity.Text = cboParity.Items[0].ToString();
//Handshake
            cboHandShaking.Items.Add("None");
            cboHandShaking.Items.Add("XOnXOff");
            cboHandShaking.Items.Add("RequestToSend");
            cboHandShaking.Items.Add("RequestToSendXOnXOff");
            //get the first item print it in the text 
            cboHandShaking.Text = cboHandShaking.Items[0].ToString();

        }

     


        private void port_DataReceived_1(object sender, SerialDataReceivedEventArgs e)
        {
            InputData = ComPort.ReadExisting();
            if (InputData != String.Empty)
            {
                this.BeginInvoke(new SetTextCallback(SetText), new object[] { InputData });
            }
            Log.LogMessage("Data Recieved  :" + InputData);
        }
        private void SetText(string text)
        {
            this.rtbIncoming.Text += text;
        }
        internal void PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            SerialPinChange SerialPinChange1 = 0;
            bool signalState = false;

            SerialPinChange1 = e.EventType;
            lblCTSStatus.BackColor = Color.Green;
            lblDSRStatus.BackColor = Color.Green;
            lblRIStatus.BackColor = Color.Green;
            lblBreakStatus.BackColor = Color.Green;
            switch (SerialPinChange1)
            {
                case SerialPinChange.Break:
                    lblBreakStatus.Visible = true;
                    lblBreakStatus.BackColor = Color.Red;

                    //MessageBox.Show("Break is Set");
                    break;
                case SerialPinChange.CDChanged:
                    signalState = ComPort.CtsHolding;
                  //  MessageBox.Show("CD = " + signalState.ToString());
                    break;
                case SerialPinChange.CtsChanged:
                    signalState = ComPort.CDHolding;
                    lblCTSStatus.Visible = true;
                    lblCTSStatus.BackColor = Color.Red;
                    //MessageBox.Show("CTS = " + signalState.ToString());
                    break;
                case SerialPinChange.DsrChanged:
                    signalState = ComPort.DsrHolding;
                    lblDSRStatus.Visible = true;
                    lblDSRStatus.BackColor = Color.Red;
                    // MessageBox.Show("DSR = " + signalState.ToString());
                    break;
                case SerialPinChange.Ring:
                    lblRIStatus.Visible = true;
                    lblRIStatus.BackColor = Color.Red;
                    //MessageBox.Show("Ring Detected");
                    break;
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            string YourTextVariable = "02032536303030303030303030313132303030301C30320040415050524F56414C20202020202031313538303000202020202020202020202020202020202020201C443000694D415942414E4B202020202020202020202020202020204D5944454249542020202020202020202020202020202052454345495054202020202020202020202020202020201C303300063137303630381C303400063131353830301C303100063131353830301C363500063030303030361C3136000831363037323833321C443100153030303032373030393030323531301C44320010564953412020202020201C333000163433363530312A2A2A2A2A2A393735371C333100042A2A2A2A1C353000063030303030311C443300123131353830303137303630381C4434000234311C443500264C494D204C4948205745492020202020202020202020202020001C0319";
            YourTextVariable = StringFromHex(YourTextVariable);

            //string YourTextVariable = "02002936303030303030303030013032363030301C363500063030303030361C032A";
            //YourTextVariable = StringFromHex(YourTextVariable);



            //SerialPinChangedEventHandler1 = new SerialPinChangedEventHandler(PinChanged);
            //ComPort.PinChanged += SerialPinChangedEventHandler1;
            //ComPort.Open();

            //ComPort.RtsEnable = true;
            //ComPort.DtrEnable = true;
            //btnTest.Enabled = false;
        }

        public static string StringFromHex(string hex) {
            string raw;
            byte[] data = FromHex(hex);
            string s = Encoding.ASCII.GetString(data); // GatewayServer
           //  raw = Regex.Replace(s, @"[\p{C}-[\t\r\n]]+", "");
            return s;
        }

        public static byte[] FromHex(string hex)
        {
            hex = hex.Replace("-", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }

        private void btnPortState_Click(object sender, EventArgs e)
        {
          
            if (btnPortState.Text == "Closed")
            {
                btnPortState.Text = "Open";
                ComPort.PortName = Convert.ToString(cboPorts.Text);
                ComPort.BaudRate = Convert.ToInt32(cboBaudRate.Text);
                ComPort.DataBits = Convert.ToInt16(cboDataBits.Text);
                ComPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cboStopBits.Text);
                ComPort.Handshake = (Handshake)Enum.Parse(typeof(Handshake), cboHandShaking.Text);
                ComPort.Parity = (Parity)Enum.Parse(typeof(Parity), cboParity.Text);
                ComPort.Open();

                Log.LogMessage("ComPort.Open()");
                Log.LogMessage("ComPort.CDHolding :  " + ComPort.CDHolding);
                Log.LogMessage("ComPort.DsrHolding :  " + ComPort.DsrHolding);
                Log.LogMessage("ComPort.CtsHolding :  " + ComPort.CtsHolding );


                Log.LogMessage("ComPort.DtrEnable :  " + ComPort.DtrEnable);
                Log.LogMessage("ComPort.RtsEnable :  " + ComPort.RtsEnable );
            }
            else if (btnPortState.Text == "Open")
            {
                btnPortState.Text = "Closed";
                ComPort.Close();
                Log.LogMessage("ComPort.Close()");
            }
        }
        private void rtbOutgoing_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) // enter key  
            {
                ComPort.Write("\r\n");
                rtbOutgoing.Text = "";
            }
            else if (e.KeyChar < 32 || e.KeyChar > 126)
            {
                e.Handled = true; // ignores anything else outside printable ASCII range  
            }
            else
            {
                ComPort.Write(e.KeyChar.ToString());
            }
        }
        private void btnHello_Click(object sender, EventArgs e)
        {

            string CommandSent = txtCommand.Text;
            CommandSent= StringFromHex(CommandSent);
            txtSendData.Text = CommandSent;
            ComPort.Write(CommandSent);
            Log.LogMessage("Data in Bytes Sent : " + txtCommand.Text);
            Log.LogMessage("Data  Sent : " + CommandSent);

            Thread.Sleep(autoPollingTime);
            timer = new System.Timers.Timer();
            timer.Interval = autoPollingTime;
            if (autoPollingTime != 0)
            {
                timer.Elapsed += timer_Elapsed;
                timer.Start();
                Log.LogMessage("Timer started...");
                rtbIncoming.Text = ComPort.ReadExisting();
                if (!string.IsNullOrEmpty(ComPort.ReadExisting()))
                {
                    Log.LogMessage("DataRecieved:" + ComPort.ReadExisting());
                    rtbIncoming.Text = ComPort.ReadExisting();
                    timer.Stop();
                    Log.LogMessage("Timer Stopped");
                }

            }

        }
        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Check();
        }

        private void Check()
        {
            Log.LogMessage("Timer running...");
            if (!string.IsNullOrEmpty(ComPort.ReadExisting()))
            {
                rtbIncoming.Text = ComPort.ReadExisting();
                Log.LogMessage("DataRecieved:" + ComPort.ReadExisting());
                rtbIncoming.Text = ComPort.ReadExisting();
                timer.Stop();
                Log.LogMessage("Timer Stopped");
            }

        }
        private void btnHyperTerm_Click(object sender, EventArgs e)
        {
            string Command1 = txtCommand.Text;
            string CommandSent;
            int Length, j = 0;

            Length = Command1.Length;

            for (int i = 0; i < Length; i++)
            {
                CommandSent = Command1.Substring(j, 1);
                ComPort.Write(CommandSent);
                Log.LogMessage("Data Sent : " + CommandSent);
                j++;
            }

        }

        private byte[] Str2Array(string str)
        {
            byte[] array = System.Text.Encoding.Unicode.GetBytes(str);
            return array;
        }

        private void btnTimerStop_Click(object sender, EventArgs e)
        {
            timer.Stop();
            Log.LogMessage("Timer Stopped");
        }
    }
}
