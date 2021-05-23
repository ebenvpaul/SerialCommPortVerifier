using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeProjectSerialComms
{
    class ECR2Terminal
    {

        private string _LengthHdr = "35";
        private string _TransportHeader  = "60";
        private string _TransportDestination  = "0000";
        private string _TransportSource  = "0000";
        private string _PresentationHeader  = "01";
        private string _RequestIndicator  = "0";
        private string _TransCode  = "26";
        private string _ResponseCode  = "00";
        private string _MoreIndicator  = "0";
        private string _FieldType  = "65";
        private string _LengthData  = "6";
        private string _InvoiceNumber  = "000006";
        private string _Data  = "10.00";
        private string _ETX  = "03";
        private string _LRC  = "24";
        private string _ECRtoTerminal;

        public  string ECR2Terminal_Sale()
            {
                Operations operations = new Operations();
                List<FieldTypeDefinition> fieldTypeDefinitions = operations.addfieldTypeDefinitions();

            try
            {

          
                Console.WriteLine("================================  Data  ==============================================================");
                _LengthData = "12";
                decimal Debitvalue = Convert.ToDecimal(_Data);
                decimal DEBITAMT = Convert.ToDecimal(string.Format("{0:F2}", Debitvalue));
                _Data = Convert.ToString(DEBITAMT);
                Console.WriteLine("DEBITAMT :" + DEBITAMT);
                _Data = _Data.Replace(".", "");
                Console.WriteLine("Formatted Amount :" + _Data);
                _Data = operations.PadString(_Data, Convert.ToInt16(_LengthData));
                Console.WriteLine("Padded Amount :" + _Data);
                Console.WriteLine("Padded Amount Length :" + _Data.Length);
                _Data = operations.SaltString(_Data, Operations._salt);
                // Console.WriteLine("Length :" & _Data.Length)
                // Console.WriteLine("Final Amount :" & _Data)
                // Console.WriteLine("Salted Amount Length :" & _Data.Length)


                Console.WriteLine("================================ Salted Hdr =========================================================");
                string SaltedHdrMessage_ = "";
                // Dim SaltedHdrMessage_ As String = _TransportHeader & " " & _TransportDestination & " " & _TransportSource & " " & " " & _PresentationHeader & " " & _RequestIndicator & " " & _TransCode & " " & _ResponseCode & " " & _MoreIndicator

                // Console.WriteLine(SaltedHdrMessage_.Replace(" ", ""))
                // Console.WriteLine("SaltedHdrMessage Length : " & SaltedHdrMessage_.Replace(" ", "").Length)

                // Console.WriteLine(" ")

                _TransportHeader = operations.SaltString(_TransportHeader, Operations._salt);
                _TransportDestination = operations.SaltString(_TransportDestination, Operations._salt);
                _TransportSource = operations.SaltString(_TransportSource, Operations._salt);
                _RequestIndicator = operations.SaltString(_RequestIndicator, Operations._salt);
                _TransCode = operations.SaltString(_TransCode, Operations._salt);
                _ResponseCode = operations.SaltString(_ResponseCode, Operations._salt);
                _MoreIndicator = operations.SaltString(_MoreIndicator, Operations._salt);
                SaltedHdrMessage_ = _TransportHeader + " " + _TransportDestination + " " + _TransportSource + " " + " " + _PresentationHeader + " " + _RequestIndicator + " " + _TransCode + " " + _ResponseCode + " " + _MoreIndicator;

                SaltedHdrMessage_ = SaltedHdrMessage_.Replace(" ", "");
                Console.WriteLine(SaltedHdrMessage_.Replace(" ", ""));
                _LengthHdr = SaltedHdrMessage_.Length.ToString ();
                _LengthHdr = operations.PadString(_LengthHdr, 4);
                Console.WriteLine("SaltedHdrMessage Length : " + _LengthHdr);



                Console.WriteLine("=============================== ECRtoTerminal  ======================================================");
                Console.WriteLine("DemoECRtoTerminal  : " + Operations.DemoECRtoTerminal);
                Console.WriteLine("DemoECRtoTerminal Length : " + Operations.DemoECRtoTerminal.Length);
                Console.WriteLine("");




                operations._FieldType = "40";
                operations._Length = operations.getLengthfromFeildType(fieldTypeDefinitions, operations._FieldType);
                operations._FieldType = operations.SaltString(operations._FieldType, Operations._salt);
                Console.WriteLine(operations._FieldType);
                _FieldType = operations._FieldType;
                operations._Length = operations.PadString(operations._Length, 4);
                Console.WriteLine(operations._Length);
                _LengthData = operations._Length;
                _ECRtoTerminal = Operations._STX + " " + _LengthHdr + " " + SaltedHdrMessage_ + " " + Operations._Seperator + " " + _FieldType + " " + _LengthData + " " + _Data + " " + Operations._Seperator + " " + _ETX + " " + _LRC;
                Console.WriteLine("");
                Console.WriteLine("ECRtoTerminal  : " + _ECRtoTerminal.Replace(" ", ""));
              
                Console.WriteLine("ECRtoTerminal Length : " + _ECRtoTerminal.Replace(" ", "").Length);
                Console.WriteLine("=====================================================================================================");
                Console.ReadKey();
                Console.ReadKey();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return _ECRtoTerminal.Replace(" ", "");

        }

            public void ECR2Terminal_Void()
            {
                Operations operations = new Operations();
                List<FieldTypeDefinition> fieldTypeDefinitions = operations.addfieldTypeDefinitions();
                Console.WriteLine("================================  Data  ==============================================================");
                _LRC = "2A";
                _LengthHdr = "29";
                decimal Debitvalue = Convert.ToDecimal(_Data);
                decimal DEBITAMT = Convert.ToDecimal(string.Format("{0:F2}", Debitvalue));
                _Data = Convert.ToString(DEBITAMT);
                Console.WriteLine("DEBITAMT :" + DEBITAMT);
                _Data = _Data.Replace(".", "");
                Console.WriteLine("Formatted Amount :" + _Data);
                _Data = operations.PadString(_Data, Convert.ToInt16(_LengthData));
                Console.WriteLine("Padded Amount :" + _Data);
                Console.WriteLine("Padded Amount Length :" + _Data.Length);
                _Data = operations.SaltString(_Data, Operations._salt);
                Console.WriteLine("Final Amount :" + _Data);
                Console.WriteLine("Amount Length :" + _Data.Length);
                Console.WriteLine("================================ Salted Hdr =========================================================");
                string SaltedHdrMessage_ = _TransportHeader + " " + _TransportDestination + " " + _TransportSource + " " + " " + _PresentationHeader + " " + _RequestIndicator + " " + _TransCode + " " + _ResponseCode + " " + _MoreIndicator;
                Console.WriteLine(SaltedHdrMessage_.Replace(" ", ""));
                Console.WriteLine("SaltedHdrMessage Length : " + SaltedHdrMessage_.Replace(" ", "").Length);
                Console.WriteLine(" ");
                _TransportHeader = operations.SaltString(_TransportHeader, Operations._salt);
                _TransportDestination = operations.SaltString(_TransportDestination, Operations._salt);
                _TransportSource = operations.SaltString(_TransportSource, Operations._salt);
                _RequestIndicator = operations.SaltString(_RequestIndicator, Operations._salt);
                _TransCode = operations.SaltString(_TransCode, Operations._salt);
                _ResponseCode = operations.SaltString(_ResponseCode, Operations._salt);
                _MoreIndicator = operations.SaltString(_MoreIndicator, Operations._salt);
                SaltedHdrMessage_ = _TransportHeader + " " + _TransportDestination + " " + _TransportSource + " " + " " + _PresentationHeader + " " + _RequestIndicator + " " + _TransCode + " " + _ResponseCode + " " + _MoreIndicator;
                Console.WriteLine(SaltedHdrMessage_.Replace(" ", ""));
                Console.WriteLine("SaltedHdrMessage Length : " + SaltedHdrMessage_.Replace(" ", "").Length);
                Console.WriteLine("=============================== ECRtoTerminal  ======================================================");
                Console.WriteLine("DemoECRtoTerminal  : " + Operations.VoidECRtoTerminal);
                Console.WriteLine("DemoECRtoTerminal Length : " + Operations.VoidECRtoTerminal.Length);
                Console.WriteLine("");
                Console.WriteLine(SaltedHdrMessage_);
                operations._FieldType = "65";
                operations._Length = operations.getLengthfromFeildType(fieldTypeDefinitions, operations._FieldType);
                operations._FieldType = operations.SaltString(operations._FieldType, Operations._salt);
                Console.WriteLine(operations._FieldType);
                _FieldType = operations._FieldType;
                operations._Length = operations.PadString(operations._Length, 4);
                Console.WriteLine(operations._Length);
                _LengthData = operations._Length;
                _InvoiceNumber = operations.PadString(_InvoiceNumber, 6);
                _InvoiceNumber = operations.SaltString(_InvoiceNumber, Operations._salt);
                Console.WriteLine(_InvoiceNumber);
                _Data = _InvoiceNumber;
                _LengthHdr = operations.PadString(_LengthHdr, 4);
                _ECRtoTerminal = Operations._STX + " " + _LengthHdr + " " + SaltedHdrMessage_ + " " + Operations._Seperator + " " + _FieldType + " " + _LengthData + " " + _Data + " " + Operations._Seperator + " " + _ETX + " " + _LRC;
                Console.WriteLine("");
                Console.WriteLine("ECRtoTerminal  : " + _ECRtoTerminal.Replace(" ", ""));
                Console.WriteLine("ECRtoTerminal Length : " + _ECRtoTerminal.Replace(" ", "").Length);
                Console.WriteLine("=====================================================================================================");
                Console.ReadKey();
                Console.ReadKey();
            }

            public void ECR2Terminal_Refund()
            {
                Operations operations = new Operations();
                List<FieldTypeDefinition> fieldTypeDefinitions = operations.addfieldTypeDefinitions();
                Console.WriteLine("================================  Data  ==============================================================");
                _LRC = "23";
                _LengthHdr = "35";
                _LengthData = "12";
                decimal Debitvalue = Convert.ToDecimal(_Data);
                decimal DEBITAMT = Convert.ToDecimal(string.Format("{0:F2}", Debitvalue));
                _Data = Convert.ToString(DEBITAMT);
                Console.WriteLine("DEBITAMT :" + DEBITAMT);
                _Data = _Data.Replace(".", "");
                Console.WriteLine("Formatted Amount :" + _Data);
                _Data = operations.PadString(_Data, Convert.ToInt16(_LengthData));
                Console.WriteLine("Padded Amount :" + _Data);
                Console.WriteLine("Padded Amount Length :" + _Data.Length);
                _Data = operations.SaltString(_Data, Operations._salt);
                Console.WriteLine("Final Amount :" + _Data);
                Console.WriteLine("Amount Length :" + _Data.Length);
                Console.WriteLine("================================ Salted Hdr =========================================================");
                string SaltedHdrMessage_ = _TransportHeader + " " + _TransportDestination + " " + _TransportSource + " " + " " + _PresentationHeader + " " + _RequestIndicator + " " + _TransCode + " " + _ResponseCode + " " + _MoreIndicator;
                Console.WriteLine(SaltedHdrMessage_.Replace(" ", ""));
                Console.WriteLine("SaltedHdrMessage Length : " + SaltedHdrMessage_.Replace(" ", "").Length);
                Console.WriteLine(" ");
                _TransportHeader = operations.SaltString(_TransportHeader, Operations._salt);
                _TransportDestination = operations.SaltString(_TransportDestination, Operations._salt);
                _TransportSource = operations.SaltString(_TransportSource, Operations._salt);
                _RequestIndicator = operations.SaltString(_RequestIndicator, Operations._salt);
                _TransCode = operations.SaltString(_TransCode, Operations._salt);
                _ResponseCode = operations.SaltString(_ResponseCode, Operations._salt);
                _MoreIndicator = operations.SaltString(_MoreIndicator, Operations._salt);
                SaltedHdrMessage_ = _TransportHeader + " " + _TransportDestination + " " + _TransportSource + " " + " " + _PresentationHeader + " " + _RequestIndicator + " " + _TransCode + " " + _ResponseCode + " " + _MoreIndicator;
                Console.WriteLine(SaltedHdrMessage_.Replace(" ", ""));
                Console.WriteLine("SaltedHdrMessage Length : " + SaltedHdrMessage_.Replace(" ", "").Length);
                Console.WriteLine("=============================== ECRtoTerminal  ======================================================");
                Console.WriteLine("RefundECRtoTerminal  : " + Operations.RefundECRtoTerminal);
                Console.WriteLine("RefundECRtoTerminal Length : " + Operations.RefundECRtoTerminal.Length);
                Console.WriteLine("");
                Console.WriteLine(SaltedHdrMessage_);
                operations._FieldType = "40";
                operations._Length = operations.getLengthfromFeildType(fieldTypeDefinitions, operations._FieldType);
                operations._FieldType = operations.SaltString(operations._FieldType, Operations._salt);
                Console.WriteLine(operations._FieldType);
                _FieldType = operations._FieldType;
                operations._Length = operations.PadString(operations._Length, 4);
                Console.WriteLine(operations._Length);
                _LengthData = operations._Length;
                _LengthHdr = operations.PadString(_LengthHdr, 4);
                _ECRtoTerminal = Operations._STX + " " + _LengthHdr + " " + SaltedHdrMessage_ + " " + Operations._Seperator + " " + _FieldType + " " + _LengthData + " " + _Data + " " + Operations._Seperator + " " + _ETX + " " + _LRC;
                Console.WriteLine("");
                _ECRtoTerminal = _ECRtoTerminal.Replace(" ", "");
                Console.WriteLine("ECRtoTerminal  : " + _ECRtoTerminal.Replace(" ", ""));
                Console.WriteLine("ECRtoTerminal Length : " + _ECRtoTerminal.Replace(" ", "").Length);
                Console.WriteLine("=====================================================================================================");
                Console.ReadKey();
                Console.ReadKey();
            }
        }

}

