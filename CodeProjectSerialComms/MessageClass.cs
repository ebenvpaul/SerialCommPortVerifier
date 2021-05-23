using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeProjectSerialComms
{

    class MessageStructure
    {
        private MessageData _MessageData;
        private string _STX, _LLLL, _ETX, _LRC;
        #region string Getters & Setters
        public string STX { get { return _STX; } set { _STX = value; } }
        public string LLLL { get { return _LLLL; } set { _LLLL = value; } }
        public MessageData MessageData { get { return _MessageData; } set { _MessageData = value; } }
        public string ETX { get { return _ETX; } set { _ETX = value; } }
        public string LRC { get { return _LRC; } set { _LRC = value; } }
        #endregion
    }


    class MessageData
    {
        private TransportHeader _TransportHeader;
        private PresentationHeader _PresentationHeader;
        private FieldDataFormat _FieldDataFormat;

        #region string Getters & Setters
        public TransportHeader TransportHeader { get { return _TransportHeader; } set { _TransportHeader = value; } }
        public PresentationHeader PresentationHeader { get { return _PresentationHeader; } set { _PresentationHeader = value; } }
        public FieldDataFormat FieldDataFormat { get { return _FieldDataFormat; } set { _FieldDataFormat = value; } }
        #endregion
    }


    class TransportHeader
    {
        private string _TransportHeaderType, _TransportDestination, _TransportSource;
        #region string Getters & Setters
        public string TransportHeaderType { get { return _TransportHeaderType; } set { _TransportHeaderType = value; } }
        public string TransportDestination { get { return _TransportDestination; } set { _TransportDestination = value; } }
        public string TransportSource { get { return _TransportSource; } set { _TransportSource = value; } }
        #endregion
    }


    class PresentationHeader
    {
        private string _FormatVersion, _RequestResponseIndicator, _PresentationHeaderType, _TransactionCode, _ResponseCode, _MoreIndicator, _FieldSeparator;
        #region string Getters & Setters

        public string PresentationHeaderType { get { return _PresentationHeaderType; } set { _PresentationHeaderType = value; } }
        public string FormatVersion { get { return _FormatVersion; } set { _FormatVersion = value; } }
        public string RequestResponseIndicator { get { return _RequestResponseIndicator; } set { _RequestResponseIndicator = value; } }
        public string TransactionCode { get { return _TransactionCode; } set { _TransactionCode = value; } }
        public string ResponseCode { get { return _ResponseCode; } set { _ResponseCode = value; } }
        public string MoreIndicator { get { return _MoreIndicator; } set { _MoreIndicator = value; } }
        public string FieldSeparator { get { return _FieldSeparator; } set { _FieldSeparator = value; } }
        #endregion
    }

    class FieldDataFormat
    {
        private string  _LLLL, _Data, _FieldSeperator;
        #region string Getters & Setters
        public List<FieldTypeDefinition> FieldType { get; set; }
        public string LLLL { get { return _LLLL; } set { _LLLL = value; } }
        public string Data { get { return _Data; } set { _Data = value; } }
        public string FieldSeperator { get { return _FieldSeperator; } set { _FieldSeperator = value; } }

        #endregion
    }


    //class FieldType
    //{
    //    private FieldTypeDefinition _ApprovalCode
    //                , _ResponseText
    //                , _TransactionDate
    //                , _TransactionTime
    //                , _TerminalID
    //                , _CardNumber_PAN
    //                , _ExpirationDate
    //                , _Amount_Transaction
    //                , _Amount_Tip
    //                , _Amount_Cash_Back
    //                , _Batch_Number
    //                , _Invoice_Number
    //                , _Merchant_Name_Address
    //                , _MerchantID
    //                , _CardIssuerName
    //                , _RetrievalReferenceNumber
    //                , _CardIssuerID
    //                , _CardHolderName;

    //    #region string Getters & Setters

    //    public FieldTypeDefinition ApprovalCode { get { return _ApprovalCode; } set { _ApprovalCode = value; } }
    //    public FieldTypeDefinition ResponseText { get { return _ResponseText; } set { _ResponseText = value; } }
    //    public FieldTypeDefinition TransactionDate { get { return _TransactionDate; } set { _TransactionDate = value; } }
    //    public FieldTypeDefinition TransactionTime { get { return _TransactionTime; } set { _TransactionTime = value; } }
    //    public FieldTypeDefinition TerminalID { get { return _TerminalID; } set { _TerminalID = value; } }
    //    public FieldTypeDefinition CardNumber_PAN { get { return _CardNumber_PAN; } set { _CardNumber_PAN = value; } }
    //    public FieldTypeDefinition ExpirationDate { get { return _ExpirationDate; } set { _ExpirationDate = value; } }
    //    public FieldTypeDefinition Amount_Transaction { get { return _Amount_Transaction; } set { _Amount_Transaction = value; } }
    //    public FieldTypeDefinition Amount_Tip { get { return _Amount_Tip; } set { _Amount_Tip = value; } }
    //    public FieldTypeDefinition Amount_Cash_Back { get { return _Amount_Cash_Back; } set { _Amount_Cash_Back = value; } }
    //    public FieldTypeDefinition Batch_Number { get { return _Batch_Number; } set { _Batch_Number = value; } }
    //    public FieldTypeDefinition Invoice_Number { get { return _Invoice_Number; } set { _Invoice_Number = value; } }
    //    public FieldTypeDefinition Merchant_Name_Address { get { return _Merchant_Name_Address; } set { _Merchant_Name_Address = value; } }
    //    public FieldTypeDefinition MerchantID { get { return _MerchantID; } set { _MerchantID = value; } }
    //    public FieldTypeDefinition CardIssuerName { get { return _CardIssuerName; } set { _CardIssuerName = value; } }
    //    public FieldTypeDefinition RetrievalReferenceNumber { get { return _RetrievalReferenceNumber; } set { _RetrievalReferenceNumber = value; } }
    //    public FieldTypeDefinition CardIssuerID { get { return _CardIssuerID; } set { _CardIssuerID = value; } }
    //    public FieldTypeDefinition CardHolderName { get { return _CardHolderName; } set { _CardHolderName = value; } }
    //    #endregion

    //}

    public class FieldTypeDefinition
    {
        public string _FieldType { get; set; }
        public string _Attribute { get; set; }
        public string _Length { get; set; }
        public string _FieldData { get; set; }
    }
}



