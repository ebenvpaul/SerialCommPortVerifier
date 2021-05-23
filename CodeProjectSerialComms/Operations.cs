using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeProjectSerialComms
{
    public class Operations


    {
        public string _FieldType = "";
        public string _Length = "";
        public const string _Seperator = "1C";
        public const string _STX = "02";
        public const string _salt = "3";
        public const string DemoECRtoTerminal = "02003536303030303030303030013032303030301C343000123030303030303030313030301C0324";
        public const string DemoTerminaltoECR = "02032536303030303030303030313132303030301C30320040415050524F56414C20202020202031313538303000" + "202020202020202020202020202020202020201C443000694D415942414E4B202020202020202020202020202020204D59444" + "54249542020202020202020202020202020202052454345495054202020202020202020202020202020201C30330006313730363038" + "1C303400063131353830301C303100063131353830301C363500063030303030361C313600083136303732383332" + "1C443100153030303032373030393030323531301C44320010564953412020202020201C333000163433363530312A2A2A2A2A2A39373537" + "1C333100042A2A2A2A1C353000063030303030311C443300123131353830303137303630381C4434000234311C443500264C494D204C4948205745" + "492020202020202020202020202020001C0319";
        public const string VoidECRtoTerminal = "02002936303030303030303030013032363030301C363500063030303030361C032A";
        public const string VoidTerminaltoECR = "02032536303030303030303030313132363030301C30320040415050524F56414C20202020202056493133303300202020202020202020202020202020202020201C303100065649313330331C363500063030303030361C443000694D415942414E4B202020202020202020202020202020204D5944454249542020202020202020202020202020202052454345495054202020202020202020202020202020201C3136000831363037323833321C443100153030303032373030393030323531301C44320010564953412020202020201C333000163433363530312A2A2A2A2A2A393735371C333100042A2A2A2A1C353000063030303030311C303300063137303630381C303400063132303035311C443300123630303731353030303032351C4434000234311C443500264C494D204C4948205745492020202020202020202020202020001C0312";
        public const string RefundECRtoTerminal = "02003536303030303030303030013032373030301C343000123030303030303030313030301C0323";
        public const string RefundTerminaltoECR = "02032536303030303030303030313132373030301C30320040415050524F56414C20202020202031323032303900202020202020202020202020202020202020201C303100063132303230391C363500063030303030371C443000694D415942414E4B202020202020202020202020202020204D5944454249542020202020202020202020202020202052454345495054202020202020202020202020202020201C3136000831363037323833321C443100153030303032373030393030323531301C44320010564953412020202020201C333000163433363530312A2A2A2A2A2A393735371C333100042A2A2A2A1C353000063030303030311C303300063137303630381C303400063132303230391C443300123132303230393137303630381C4434000234311C443500264C494D204C4948205745492020202020202020202020202020001C031F";


        public string SaltString(string sentence, string seperator)
        {
            string saltedStr = "";

            //char[] charArr = null;

            //if (sentence.Length > 0)
            //{
            //    charArr = sentence.ToCharArray();

            //    foreach (char ch in charArr)
            //        saltedStr += seperator + ch;
            //}

            saltedStr = HexString(sentence);
            return saltedStr;
        }






        public string PadString(string sentence, int Len)
        {
            string PadString = "";
            try
            {
                if (sentence.Length > 0)
                    PadString = sentence.PadLeft(Len, '0');
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return PadString;
        }

        public string PadStringRight(string sentence, int Len)
        {
            string PadString = "";

            try
            {
                if (sentence.Length > 0)
                    PadString = sentence.PadRight(Len, ' ');
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return PadString;
        }

        public string HexString(string sentence)
        {
            string HexString = "";

            try
            {
                byte[] ba = Encoding.Default.GetBytes(sentence);
                HexString = BitConverter.ToString(ba);
                HexString = HexString.Replace("-", "");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return HexString;
        }

        public List<FieldTypeDefinition> addfieldTypeDefinitions()
        {
            List<FieldTypeDefinition> fieldTypeDefinitions = new List<FieldTypeDefinition>();

            try
            {
                fieldTypeDefinitions.Add(new FieldTypeDefinition()
                {
                    _FieldType = "01",
                    _Attribute = "ANS",
                    _Length = "6",
                    _FieldData = "Approval Code"
                });
                fieldTypeDefinitions.Add(new FieldTypeDefinition()
                {
                    _FieldType = "02",
                    _Attribute = "ANS",
                    _Length = "40",
                    _FieldData = "Response Text"
                });
                fieldTypeDefinitions.Add(new FieldTypeDefinition()
                {
                    _FieldType = "03",
                    _Attribute = "N",
                    _Length = "6",
                    _FieldData = "Transaction Date(YYMMDD)"
                });
                fieldTypeDefinitions.Add(new FieldTypeDefinition()
                {
                    _FieldType = "04",
                    _Attribute = "N",
                    _Length = "6",
                    _FieldData = "Transaction Time(HHMMSS)"
                });
                fieldTypeDefinitions.Add(new FieldTypeDefinition()
                {
                    _FieldType = "16",
                    _Attribute = "N",
                    _Length = "8",
                    _FieldData = "Terminal ID(TID)"
                });
                fieldTypeDefinitions.Add(new FieldTypeDefinition()
                {
                    _FieldType = "30",
                    _Attribute = "N",
                    _Length = "19",
                    _FieldData = "Card Number/ PAN"
                });
                fieldTypeDefinitions.Add(new FieldTypeDefinition()
                {
                    _FieldType = "31",
                    _Attribute = "N",
                    _Length = "4",
                    _FieldData = "Expiration Date(YYMM)"
                });
                fieldTypeDefinitions.Add(new FieldTypeDefinition()
                {
                    _FieldType = "40",
                    _Attribute = "$",
                    _Length = "12",
                    _FieldData = "Amount, Transaction"
                });
                fieldTypeDefinitions.Add(new FieldTypeDefinition()
                {
                    _FieldType = "41",
                    _Attribute = "$",
                    _Length = "12",
                    _FieldData = "Amount, Tip"
                });
                fieldTypeDefinitions.Add(new FieldTypeDefinition()
                {
                    _FieldType = "42",
                    _Attribute = "$",
                    _Length = "12",
                    _FieldData = "Amount, Cash Back"
                });
                fieldTypeDefinitions.Add(new FieldTypeDefinition()
                {
                    _FieldType = "50",
                    _Attribute = "N",
                    _Length = "6",
                    _FieldData = "Batch Number"
                });
                fieldTypeDefinitions.Add(new FieldTypeDefinition()
                {
                    _FieldType = "65",
                    _Attribute = "ANS",
                    _Length = "6",
                    _FieldData = "Invoice Number"
                });
                fieldTypeDefinitions.Add(new FieldTypeDefinition()
                {
                    _FieldType = "D0",
                    _Attribute = "ANS",
                    _Length = "69",
                    _FieldData = "Merchant Name and Address"
                });
                fieldTypeDefinitions.Add(new FieldTypeDefinition()
                {
                    _FieldType = "D1",
                    _Attribute = "ANS",
                    _Length = "15",
                    _FieldData = "Merchant ID(MID)"
                });
                fieldTypeDefinitions.Add(new FieldTypeDefinition()
                {
                    _FieldType = "D2",
                    _Attribute = "ANS",
                    _Length = "10",
                    _FieldData = "Card Issuer Name"
                });
                fieldTypeDefinitions.Add(new FieldTypeDefinition()
                {
                    _FieldType = "D3",
                    _Attribute = "ANS",
                    _Length = "12",
                    _FieldData = "Retrieval Reference Number(RRN)"
                });
                fieldTypeDefinitions.Add(new FieldTypeDefinition()
                {
                    _FieldType = "D4",
                    _Attribute = "N",
                    _Length = "2",
                    _FieldData = "Card Issuer ID – Refer to Card Issuer ID Table"
                });
                fieldTypeDefinitions.Add(new FieldTypeDefinition()
                {
                    _FieldType = "D5",
                    _Attribute = "ANS",
                    _Length = "26",
                    _FieldData = "Card Holder Name"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return fieldTypeDefinitions;
        }

        public string getLengthfromFeildType(List<FieldTypeDefinition> fieldTypeDefinitions, string _FieldType)
        {
            string getLengthfromFeildType = "";
            int index = fieldTypeDefinitions.FindIndex(p => p._FieldType == _FieldType);

            try
            {
                getLengthfromFeildType = fieldTypeDefinitions[index]._Length;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return getLengthfromFeildType;
        }


        public FieldTypeDefinition getFeildType(List<FieldTypeDefinition> fieldTypeDefinitions, string _FieldType)
        {
            FieldTypeDefinition getFieldTypeDefinition = new FieldTypeDefinition();
            int index = fieldTypeDefinitions.FindIndex(p => p._FieldType == _FieldType);
            try
            {
                getFieldTypeDefinition = fieldTypeDefinitions[index];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return getFieldTypeDefinition;
        }





        public static  string RemoveGenericSalt(string sentence)
        {
            string newname = "";
            try
            {
                sentence = Reverse(sentence);// string reversed inorder to select charcters from 0th position
                char[] namechar = sentence.ToArray();
                for (int i = 0; i < namechar.Count(); i++)
                {
                    if ((i % 2) == 0) // Check character position is odd or even and select odd position inorder to remove salt
                    {
                        newname += namechar[i].ToString().ToUpper();
                    }
                }
                newname= Reverse(newname);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return newname;

        }

        public static string RemovePadding(string sentence)
        {
            try
            {
                sentence=sentence.TrimStart(new Char[] { '0' });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return sentence;
        }



        public static string Reverse(string s)
        {
            string newname = "";
            try
            {
                char[] charArray = s.ToCharArray();
                Array.Reverse(charArray);
                 newname =  new string(charArray);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return newname;


        }



        public static string GetStringFromHexaDecimalString(string s)
        {
            string newname ="";
            string strHex = "";
            try
            {
                newname = String.Join(" ", s.ToCharArray().Aggregate("", (result, c) => result += ((!string.IsNullOrEmpty(result) && (result.Length + 1) % 3 == 0) ? " " : "") + c.ToString()).Split(' ').ToList().Select( x => x.Length == 1 ? string.Format("{0}{1}", Int32.Parse(x) - 1, x): x).ToArray());
                string[] hexValuesSplit = newname.Split(' ');
                foreach (String hex in hexValuesSplit)
                {
                    // Convert the number expressed in base-16 to an integer.
                    int value = Convert.ToInt32(hex, 16);
                    // Get the character corresponding to the integral value.
                    string stringValue = Char.ConvertFromUtf32(value);
                    char charValue = (char)value;
                    strHex += charValue.ToString();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return strHex;
        }



        private static  byte[] Str2Array(string str)
        {
            byte[] array = System.Text.Encoding.Unicode.GetBytes(str);
            return array;
        }

        public static string HexFormated(string hex)
        {
            hex = Regex.Replace(hex, @"[^\u0000-\u007F]+", string.Empty);
            hex = Regex.Replace(hex, @"[\p{C}-[\t\r\n]]+", string.Empty);
            return hex;
        }

        public static string[] SplitString(string inputstr, string seperator)
        {
            return inputstr.Split(new string[] { seperator }, StringSplitOptions.None);
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

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }


        public static string StringFromHex(string hex)
        {
            //byte[] data = FromHex(hex);
            byte[] data = StringToByteArray(hex);
            string s = Encoding.ASCII.GetString(data); // GatewayServer
                                                       //  raw = Regex.Replace(s, @"[\p{C}-[\t\r\n]]+", "");
            return s;
        }

        public enum TransactionCode
        {
            Sale = 20,
            Void = 26,
            Refund = 27,
            Settlement = 50
        }
    }
}