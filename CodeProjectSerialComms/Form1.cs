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
using static CodeProjectSerialComms.Operations;

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

        public  bool DecodeRecievedHexValue(string hex)
        {
            string SplittedStr;
            try
            {
                {
                    MessageStructure messageStructure = new MessageStructure();
                    Operations operations = new Operations();
                    MessageData messageData = new MessageData();
                    TransportHeader transportHeader = new TransportHeader();
                    PresentationHeader presentationHeader = new PresentationHeader();
                    List<FieldTypeDefinition> fieldTypeList = new List<FieldTypeDefinition>();
                    List<FieldTypeDefinition> fieldTypeDefinitionslst;
                    fieldTypeDefinitionslst = operations.addfieldTypeDefinitions();
                    FieldTypeDefinition fieldTypeDefinitions = new FieldTypeDefinition();
                    FieldTypeDefinition RecievedfieldTypeDefinitions = new FieldTypeDefinition();
                  
                    string[] SplitedStrings = SplitString(hex, Operations._Seperator);

                    for (int i = 0; i < SplitedStrings.Length; i++)
                    {
                        rtbOutgoing.Text += "\n" + SplitedStrings[i];
                    }
                    // Splitting Header Lines 
                    try
                    {
                        SplittedStr = SplitedStrings[0];
                        //0203253630303030303030303031313230303030

                        operations._FieldType = SplittedStr.Substring(0, 2);
                        // 02

                        operations._Length = SplittedStr.Substring(2, 4);
                        //0325

                        operations._Length = RemoveGenericSalt(operations._Length);

                        //operations._Length = operations.GetStringFromHexaDecimalString(operations._Length);
                        // operations._Length = operations.RemovePadding(operations._Length);
                        //325

                        SplittedStr = SplittedStr.Substring(6, int.Parse(operations._Length) - 1);
                        //3630303030303030303031313230303030

                        SplittedStr = StringFromHex(SplittedStr);
                        //60000000001120000

                        SplittedStr = HexFormated(SplittedStr);
                        //60000000001120000

                        // Fetch Data from Header Response
                        //   6 0 0 0 0 0 0 0 0 0  1  1  2  0  0  0  0
                        //   0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16


                        messageStructure.STX = operations._FieldType;
                        // 02

                        messageStructure.LLLL = operations._Length;
                        //35

                        transportHeader.TransportHeaderType = SplittedStr.Substring(0, 2);
                        // 60

                        transportHeader.TransportDestination = SplittedStr.Substring(2, 4);
                        // 0000

                        transportHeader.TransportSource = SplittedStr.Substring(4, 4);
                        // 0000

                        presentationHeader.PresentationHeaderType = SplittedStr.Substring(10, 1);
                        // 1

                        presentationHeader.RequestResponseIndicator = SplittedStr.Substring(11, 1);
                        // 1

                        presentationHeader.TransactionCode = SplittedStr.Substring(12, 2);
                        // 20

                        presentationHeader.ResponseCode = SplittedStr.Substring(14, 2);
                        // 00

                        presentationHeader.MoreIndicator = SplittedStr.Substring(16, 1);
                        // 0

                        rtbIncoming.Text += "STX                         : " + messageStructure.STX + "\n";
                        rtbIncoming.Text += "LLLL                        : " + messageStructure.LLLL + "\n";
                        rtbIncoming.Text += "TransportHeaderType         : " + transportHeader.TransportHeaderType + "\n";
                        rtbIncoming.Text += "TransportDestination        : " + transportHeader.TransportDestination + "\n";
                        rtbIncoming.Text += "TransportSource             : " + transportHeader.TransportSource + "\n";
                        rtbIncoming.Text += "PresentationHeaderType      : " + presentationHeader.PresentationHeaderType + "\n";
                        rtbIncoming.Text += "RequestResponseIndicator    : " + presentationHeader.RequestResponseIndicator + "\n";
                        rtbIncoming.Text += "TransactionCode             : " + presentationHeader.TransactionCode + "\n";
                        rtbIncoming.Text += "TransactionType             : " + GetTransactionType(int.Parse(presentationHeader.TransactionCode)) + "\n";
                        rtbIncoming.Text += "ResponseCode                : " + presentationHeader.ResponseCode + "\n";
                        rtbIncoming.Text += "MoreIndicator               : " + presentationHeader.MoreIndicator + "\n";

                    }
                    catch (Exception ex)
                    {
                        rtbIncoming.Text = ex.Message;
                    }


                    for (int i = 1; i < SplitedStrings.Length - 1; i++)
                    {
                        fieldTypeDefinitions = new FieldTypeDefinition();
                        SplittedStr = "";
                        fieldTypeDefinitions._FieldData = "";
                        fieldTypeDefinitions._Length = "";
                        try
                        {

                            ////======== testing ==========
                            SplittedStr = SplitedStrings[i];

                            operations._FieldType = SplittedStr.Substring(0, 4);
                            //operations._FieldType = operations.RemoveGenericSalt(operations._FieldType);
                            operations._FieldType = GetStringFromHexaDecimalString(operations._FieldType);
                            // 3032 =>02

                            operations._Length = SplittedStr.Substring(4, 4);
                            //0040

                            operations._Length = RemovePadding(operations._Length);
                            //40

                            SplittedStr = SplittedStr.Substring(8, int.Parse(operations._Length) - 1);
                            ////==================

                            SplittedStr = StringFromHex(SplitedStrings[i]);
                            SplittedStr = HexFormated(SplittedStr);

                            RecievedfieldTypeDefinitions = new FieldTypeDefinition();
                            //  if (SplittedStr.Length > 0) { operations._FieldType = SplittedStr.Substring(0, 2); operations._Length = SplittedStr.Substring(2, 2); }
                            fieldTypeDefinitions = operations.getFeildType(fieldTypeDefinitionslst, operations._FieldType);
                            if (fieldTypeDefinitions._Length != null)

                            {
                                //if (SplittedStr.Length > 0 && SplittedStr.Length > int.Parse(fieldTypeDefinitions._Length))
                                //{
                                //    SplittedStr = SplittedStr.Substring(2, int.Parse(fieldTypeDefinitions._Length));
                                //}

                                if (SplittedStr.Length > 0 && SplittedStr.Length > int.Parse(operations._Length))
                                {
                                    SplittedStr = SplittedStr.Substring(2, int.Parse(operations._Length));
                                }

                                RecievedfieldTypeDefinitions._FieldData = SplittedStr;
                                RecievedfieldTypeDefinitions._FieldType = operations._FieldType;
                                RecievedfieldTypeDefinitions._Attribute = fieldTypeDefinitions._FieldData;
                                RecievedfieldTypeDefinitions._Length = fieldTypeDefinitions._Length;
                            }
                            fieldTypeList.Add(RecievedfieldTypeDefinitions);

                        }
                        catch (Exception)
                        {
                            rtbIncoming.Text = "";
                            rtbIncoming.Text += "\n" + i.ToString();


                        }

                        //operations._Length = SplittedStr.Substring(2, 2);

                    }


                    for (int i = 0; i < fieldTypeList.Count; i++)
                    {
                        rtbIncoming.Text += "\n" + fieldTypeList[i]._Attribute + " : " + fieldTypeList[i]._FieldType + " : " + fieldTypeList[i]._Length + " : " + fieldTypeList[i]._FieldData;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return true;

        }


        private void btnTest_Click(object sender, EventArgs e)
        {
          

            //Sale
            //string YourTextVariable = "02032536303030303030303030313132303030301C30320040415050524F56414C20202020202031313538303000202020202020202020202020202020202020201C443000694D415942414E4B202020202020202020202020202020204D5944454249542020202020202020202020202020202052454345495054202020202020202020202020202020201C303300063137303630381C303400063131353830301C303100063131353830301C363500063030303030361C3136000831363037323833321C443100153030303032373030393030323531301C44320010564953412020202020201C333000163433363530312A2A2A2A2A2A393735371C333100042A2A2A2A1C353000063030303030311C443300123131353830303137303630381C4434000234311C443500264C494D204C4948205745492020202020202020202020202020001C0319";

            //Void
            //string YourTextVariable = "02032536303030303030303030313132363030301C30320040415050524F56414C20202020202056493133303300202020202020202020202020202020202020201C303100065649313330331C363500063030303030361C443000694D415942414E4B202020202020202020202020202020204D5944454249542020202020202020202020202020202052454345495054202020202020202020202020202020201C3136000831363037323833321C443100153030303032373030393030323531301C44320010564953412020202020201C333000163433363530312A2A2A2A2A2A393735371C333100042A2A2A2A1C353000063030303030311C303300063137303630381C303400063132303035311C443300123630303731353030303032351C4434000234311C443500264C494D204C4948205745492020202020202020202020202020001C0312";

            //Refund
            string YourTextVariable = "02032536303030303030303030313132373030301C30320040415050524F56414C20202020202031323032303900202020202020202020202020202020202020201C303100063132303230391C363500063030303030371C443000694D415942414E4B202020202020202020202020202020204D5944454249542020202020202020202020202020202052454345495054202020202020202020202020202020201C3136000831363037323833321C443100153030303032373030393030323531301C44320010564953412020202020201C333000163433363530312A2A2A2A2A2A393735371C333100042A2A2A2A1C353000063030303030311C303300063137303630381C303400063132303230391C443300123132303230393137303630381C4434000234311C443500264C494D204C4948205745492020202020202020202020202020001C031F";






            ECR2Terminal eCRtoTerminal = new ECR2Terminal();
            Terminal2ECR terminaltoECR = new Terminal2ECR();


            Console.WriteLine("================================ECR2Terminal_Sale====================================================");
             rtbOutgoing.Text  = eCRtoTerminal.ECR2Terminal_Sale();


            //DecodeRecievedHexValue(YourTextVariable);
        }

         



        private static string GetTransactionType(int type)
        {
            string trtype = "";
            try
            {
                switch (type)
                {
                    case 20:
                        trtype = "SALE";
                        break;
                    case 26:
                        trtype = "Void";
                        break;
                    case 27:
                        trtype = "Refund";
                        break;
                    case 50:
                        trtype = "Settlement";
                        break;
                    default:
                        trtype = "Invalid";
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return trtype;

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
                DecodeRecievedHexValue(rtbIncoming.Text);
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


        private void btnTimerStop_Click(object sender, EventArgs e)
        {
            timer.Stop();
            Log.LogMessage("Timer Stopped");
        }

       
    }
}
