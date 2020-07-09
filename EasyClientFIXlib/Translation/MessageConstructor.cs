using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyClientFIXlib.Translation
{
    public class MessageConstructor
    {
        private string _host;
        private string _username;
        private string _password;
        private string _senderCompID;
        private string _senderSubID;
        private string _targetCompID;

        public MessageConstructor(string host, string username, string password, string senderCompID, string senderSubID, string targetCompID)
        {
            this._host = host;
            this._username = username;
            this._password = password;
            this._senderCompID = senderCompID;
            this._senderSubID = senderSubID;
            this._targetCompID = targetCompID;
        }

        public string LogonMessage(MessageConstructor.SessionQualifier qualifier, int messageSequenceNumber, int heartBeatSeconds, bool resetSeqNum)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("98=0|");
            stringBuilder.Append("108=" + (object)heartBeatSeconds + "|");
            if (resetSeqNum)
                stringBuilder.Append("141=Y|");
            stringBuilder.Append("553=" + this._username + "|");
            stringBuilder.Append("554=" + this._password + "|");
            string str1 = this.ConstructHeader(qualifier, this.SessionMessageCode(MessageConstructor.SessionMessageType.Logon), messageSequenceNumber, stringBuilder.ToString());
            string str2 = this.ConstructTrailer(str1 + (object)stringBuilder);
            return (str1 + (object)stringBuilder + str2).Replace("|", "\x0001");
        }

        public string HeartbeatMessage(MessageConstructor.SessionQualifier qualifier, int messageSequenceNumber)
        {
            string message = this.ConstructHeader(qualifier, this.SessionMessageCode(MessageConstructor.SessionMessageType.Heartbeat), messageSequenceNumber, string.Empty);
            string str = this.ConstructTrailer(message);
            return (message + str).Replace("|", "\x0001");
        }

        public string TestRequestMessage(MessageConstructor.SessionQualifier qualifier, int messageSequenceNumber, int testRequestID)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("112=" + (object)testRequestID + "|");
            string str1 = this.ConstructHeader(qualifier, this.SessionMessageCode(MessageConstructor.SessionMessageType.TestRequest), messageSequenceNumber, stringBuilder.ToString());
            string str2 = this.ConstructTrailer(str1 + (object)stringBuilder);
            return (str1 + (object)stringBuilder + str2).Replace("|", "\x0001");
        }

        public string LogoutMessage(MessageConstructor.SessionQualifier qualifier, int messageSequenceNumber)
        {
            string message = this.ConstructHeader(qualifier, this.SessionMessageCode(MessageConstructor.SessionMessageType.Logout), messageSequenceNumber, string.Empty);
            string str = this.ConstructTrailer(message);
            return (message + str).Replace("|", "\x0001");
        }

        public string ResendMessage(MessageConstructor.SessionQualifier qualifier, int messageSequenceNumber, int endSequenceNo)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("16=" + (object)endSequenceNo + "|");
            string str1 = this.ConstructHeader(qualifier, this.SessionMessageCode(MessageConstructor.SessionMessageType.Resend), messageSequenceNumber, stringBuilder.ToString());
            string str2 = this.ConstructTrailer(str1 + (object)stringBuilder);
            return (str1 + (object)stringBuilder + str2).Replace("|", "\x0001");
        }

        public string RejectMessage(MessageConstructor.SessionQualifier qualifier, int messageSequenceNumber, int rejectSequenceNumber)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("45=" + (object)rejectSequenceNumber + "|");
            string str1 = this.ConstructHeader(qualifier, this.SessionMessageCode(MessageConstructor.SessionMessageType.Reject), messageSequenceNumber, string.Empty);
            string str2 = this.ConstructTrailer(str1 + (object)stringBuilder);
            return (str1 + (object)stringBuilder + str2).Replace("|", "\x0001");
        }

        public string SequenceResetMessage(MessageConstructor.SessionQualifier qualifier, int messageSequenceNumber, int rejectSequenceNumber)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("36=" + (object)rejectSequenceNumber + "|");
            string str1 = this.ConstructHeader(qualifier, this.SessionMessageCode(MessageConstructor.SessionMessageType.SequenceReset), messageSequenceNumber, string.Empty);
            string str2 = this.ConstructTrailer(str1 + (object)stringBuilder);
            return (str1 + (object)stringBuilder + str2).Replace("|", "\x0001");
        }

        public string MarketDataRequestMessage(MessageConstructor.SessionQualifier qualifier, int messageSequenceNumber, string marketDataRequestID, int subscriptionRequestType, int marketDepth, int marketDataEntryType, int noRelatedSymbol, long symbol)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("262=" + marketDataRequestID + "|");
            stringBuilder.Append("263=" + (object)subscriptionRequestType + "|");
            stringBuilder.Append("264=" + (object)marketDepth + "|");
            stringBuilder.Append("265=1|");
            stringBuilder.Append("267=2|");
            stringBuilder.Append("269=0|269=1|");
            stringBuilder.Append("146=" + (object)noRelatedSymbol + "|");
            stringBuilder.Append("55=" + (object)symbol + "|");
            string str1 = this.ConstructHeader(qualifier, this.ApplicationMessageCode(MessageConstructor.ApplicationMessageType.MarketDataRequest), messageSequenceNumber, stringBuilder.ToString());
            string str2 = this.ConstructTrailer(str1 + (object)stringBuilder);
            return (str1 + (object)stringBuilder + str2).Replace("|", "\x0001");
        }

        public string MarketDataSnapshotMessage(MessageConstructor.SessionQualifier qualifier, int messageSequenceNumber, long symbol, string noMarketDataEntries, int entryType, Decimal entryPrice, string marketDataRequestID = "")
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("262=" + marketDataRequestID + "|");
            stringBuilder.Append("55=" + (object)symbol + "|");
            stringBuilder.Append("268=" + noMarketDataEntries + "|");
            stringBuilder.Append("269=" + (object)entryType + "|");
            stringBuilder.Append("270=" + (object)entryPrice + "|");
            string str1 = this.ConstructHeader(qualifier, this.ApplicationMessageCode(MessageConstructor.ApplicationMessageType.MarketDataRequest), messageSequenceNumber, stringBuilder.ToString());
            string str2 = this.ConstructTrailer(str1 + (object)stringBuilder);
            return (str1 + (object)stringBuilder + str2).Replace("|", "\x0001");
        }

        public string MarketDataIncrementalRefreshMessage(MessageConstructor.SessionQualifier qualifier, int messageSequenceNumber, string marketDataRequestID, int noMarketDataEntries, int marketDataUpdateAction, int marketDataEntryType, string marketDataEntryID, int noRelatedSymbol, Decimal marketDataEntryPrice = 0M, double marketDataEntrySize = 0.0, long symbol = 0)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("262=" + marketDataRequestID + "|");
            stringBuilder.Append("268=" + (object)noMarketDataEntries + "|");
            stringBuilder.Append("279=" + (object)marketDataUpdateAction + "|");
            stringBuilder.Append("269=" + (object)marketDataEntryType + "|");
            stringBuilder.Append("278=" + marketDataEntryID + "|");
            stringBuilder.Append("55=" + (object)symbol + "|");
            if (marketDataEntryPrice > Decimal.Zero)
                stringBuilder.Append("270=" + (object)marketDataEntryPrice + "|");
            if (marketDataEntrySize > 0.0)
                stringBuilder.Append("271=" + (object)marketDataEntrySize + "|");
            string str1 = this.ConstructHeader(qualifier, this.ApplicationMessageCode(MessageConstructor.ApplicationMessageType.MarketDataIncrementalRefresh), messageSequenceNumber, string.Empty);
            string str2 = this.ConstructTrailer(str1 + (object)stringBuilder);
            return (str1 + (object)stringBuilder + str2).Replace("|", "\x0001");
        }

        public string NewOrderSingleMessage(MessageConstructor.SessionQualifier qualifier, int messageSequenceNumber, string orderID, long symbol, int side, string transactTime, int orderQuantity, int orderType, string timeInForce, Decimal price = 0M, Decimal stopPrice = 0M, string expireTime = "", string positionID = "")
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("11=" + orderID + "|");
            stringBuilder.Append("55=" + (object)symbol + "|");
            stringBuilder.Append("54=" + (object)side + "|");
            stringBuilder.Append("60=" + transactTime + "|");
            stringBuilder.Append("38=" + (object)orderQuantity + "|");
            stringBuilder.Append("40=" + (object)orderType + "|");
            if (price != Decimal.Zero)
                stringBuilder.Append("44=" + (object)price + "|");
            if (stopPrice != Decimal.Zero)
                stringBuilder.Append("99=" + (object)stopPrice + "|");
            stringBuilder.Append("59=" + timeInForce + "|");
            if (expireTime != string.Empty)
                stringBuilder.Append("126=" + expireTime + "|");
            if (positionID != string.Empty)
                stringBuilder.Append("721=" + positionID + "|");
            string str1 = this.ConstructHeader(qualifier, this.ApplicationMessageCode(MessageConstructor.ApplicationMessageType.NewOrderSingle), messageSequenceNumber, stringBuilder.ToString());
            string str2 = this.ConstructTrailer(str1 + (object)stringBuilder);
            return (str1 + (object)stringBuilder + str2).Replace("|", "\x0001");
        }

        public string OrderStatusRequest(MessageConstructor.SessionQualifier qualifier, int messageSequenceNumber, string orderID)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("11=" + orderID + "|");
            stringBuilder.Append("54=1|");
            string str1 = this.ConstructHeader(qualifier, this.ApplicationMessageCode(MessageConstructor.ApplicationMessageType.OrderStatusRequest), messageSequenceNumber, stringBuilder.ToString());
            string str2 = this.ConstructTrailer(str1 + (object)stringBuilder);
            return (str1 + (object)stringBuilder + str2).Replace("|", "\x0001");
        }

        public string ExecutionReport(MessageConstructor.SessionQualifier qualifier, int messageSequenceNumber, string cTraderOrderID, string orderStatus, string transactTime, long symbol = 0, int side = 1, int averagePrice = 0, int orderQuantity = 0, int leavesQuantity = 0, int cumQuantity = 0, string orderID = "", string orderType = "", int price = 0, int stopPrice = 0, string timeInForce = "", string expireTime = "", string text = "", int orderRejectionReason = -1, string positionID = "")
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("37=" + cTraderOrderID + "|");
            stringBuilder.Append("11=" + orderID + "|");
            stringBuilder.Append("150=F|");
            stringBuilder.Append("39=" + orderStatus + "|");
            stringBuilder.Append("55=" + (object)symbol + "|");
            stringBuilder.Append("54=" + (object)side + "|");
            stringBuilder.Append("60=" + transactTime + "|");
            if (averagePrice > 0)
                stringBuilder.Append("6=" + (object)averagePrice + "|");
            if (orderQuantity > 0)
                stringBuilder.Append("38=" + (object)orderQuantity + "|");
            if (leavesQuantity > 0)
                stringBuilder.Append("151=" + (object)leavesQuantity + "|");
            if (cumQuantity > 0)
                stringBuilder.Append("14=" + (object)cumQuantity + "|");
            if (orderType != string.Empty)
                stringBuilder.Append("40=" + orderType + "|");
            if (price > 0)
                stringBuilder.Append("44=" + (object)price + "|");
            if (stopPrice > 0)
                stringBuilder.Append("99=" + (object)stopPrice + "|");
            if (timeInForce != string.Empty)
                stringBuilder.Append("59=" + timeInForce + "|");
            if (expireTime != string.Empty)
                stringBuilder.Append("126=" + expireTime + "|");
            if (text != string.Empty)
                stringBuilder.Append("58=" + text + "|");
            if (orderRejectionReason != -1)
                stringBuilder.Append("103=" + (object)orderRejectionReason + "|");
            if (positionID != string.Empty)
                stringBuilder.Append("721=" + positionID + "|");
            string str1 = this.ConstructHeader(qualifier, this.ApplicationMessageCode(MessageConstructor.ApplicationMessageType.OrderStatusRequest), messageSequenceNumber, stringBuilder.ToString());
            string str2 = this.ConstructTrailer(str1 + (object)stringBuilder);
            return (str1 + (object)stringBuilder + str2).Replace("|", "\x0001");
        }

        public string BusinessMessageReject(MessageConstructor.SessionQualifier qualifier, int messageSequenceNumber, int referenceSequenceNum = 0, string referenceMessageType = "", string businessRejectRefID = "", int businessRejectReason = -1, string text = "")
        {
            StringBuilder stringBuilder = new StringBuilder();
            if ((uint)referenceSequenceNum > 0U)
                stringBuilder.Append("45=" + (object)referenceSequenceNum + "|");
            if (referenceMessageType != string.Empty)
                stringBuilder.Append("372=" + referenceMessageType + "|");
            if (businessRejectRefID != string.Empty)
                stringBuilder.Append("379=" + businessRejectRefID + "|");
            if (businessRejectReason != -1)
                stringBuilder.Append("380=" + (object)businessRejectReason + "|");
            if (text != string.Empty)
                stringBuilder.Append("58=" + text + "|");
            string str1 = this.ConstructHeader(qualifier, this.ApplicationMessageCode(MessageConstructor.ApplicationMessageType.BusinessMessageReject), messageSequenceNumber, stringBuilder.ToString());
            string str2 = this.ConstructTrailer(str1 + (object)stringBuilder);
            return (str1 + (object)stringBuilder + str2).Replace("|", "\x0001");
        }

        public string RequestForPositions(MessageConstructor.SessionQualifier qualifier, int messageSequenceNumber, string requestID, string positionRequestID = "")
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("710=" + requestID + "|");
            if (positionRequestID != string.Empty)
                stringBuilder.Append("721=" + positionRequestID + "|");
            string str1 = this.ConstructHeader(qualifier, this.ApplicationMessageCode(MessageConstructor.ApplicationMessageType.RequestForPosition), messageSequenceNumber, stringBuilder.ToString());
            string str2 = this.ConstructTrailer(str1 + (object)stringBuilder);
            return (str1 + (object)stringBuilder + str2).Replace("|", "\x0001");
        }

        public string PositionReport(MessageConstructor.SessionQualifier qualifier, int messageSequenceNumber, string requestID, string totalNumberOfPositionReports, string positionRequestResult, string positionID = "", string symbol = "", string noOfPositions = "", string longQuantity = "", string shortQuantity = "", string settlementPrice = "")
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("710=" + requestID + "|");
            if (positionID != string.Empty)
                stringBuilder.Append("721=" + positionID + "|");
            stringBuilder.Append("727=" + totalNumberOfPositionReports + "|");
            stringBuilder.Append("728=" + positionRequestResult + "|");
            if (symbol != string.Empty)
                stringBuilder.Append("55=" + symbol + "|");
            if (noOfPositions != string.Empty)
                stringBuilder.Append("702=" + noOfPositions + "|");
            if (longQuantity != string.Empty)
                stringBuilder.Append("704=" + requestID + "|");
            if (shortQuantity != string.Empty)
                stringBuilder.Append("705=" + shortQuantity + "|");
            if (settlementPrice != string.Empty)
                stringBuilder.Append("730=" + settlementPrice + "|");
            string str1 = this.ConstructHeader(qualifier, this.ApplicationMessageCode(MessageConstructor.ApplicationMessageType.PositionReport), messageSequenceNumber, string.Empty);
            string str2 = this.ConstructTrailer(str1 + (object)stringBuilder);
            return (str1 + (object)stringBuilder + str2).Replace("|", "\x0001");
        }

        private string ConstructHeader(MessageConstructor.SessionQualifier qualifier, string type, int messageSequenceNumber, string bodyMessage)
        {
            StringBuilder stringBuilder1 = new StringBuilder();
            stringBuilder1.Append("8=FIX.4.4|");
            StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder2.Append("35=" + type + "|");
            stringBuilder2.Append("49=" + this._senderCompID + "|");
            stringBuilder2.Append("56=" + this._targetCompID + "|");
            stringBuilder2.Append("57=" + qualifier.ToString() + "|");
            stringBuilder2.Append("50=" + this._senderSubID + "|");
            stringBuilder2.Append("34=" + (object)messageSequenceNumber + "|");
            stringBuilder2.Append("52=" + DateTime.UtcNow.ToString("yyyyMMdd-HH:mm:ss") + "|");
            int num = stringBuilder2.Length + bodyMessage.Length;
            stringBuilder1.Append("9=" + (object)num + "|");
            stringBuilder1.Append((object)stringBuilder2);
            return stringBuilder1.ToString();
        }

        private string ConstructTrailer(string message)
        {
            return "10=" + this.CalculateChecksum(message.Replace("|", "\x0001").ToString()).ToString().PadLeft(3, '0') + "|";
        }

        private int CalculateChecksum(string dataToCalculate)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(dataToCalculate);
            int num1 = 0;
            foreach (byte num2 in bytes)
                num1 += (int)num2;
            return num1 % 256;
        }

        private string SessionMessageCode(MessageConstructor.SessionMessageType type)
        {
            switch (type)
            {
                case MessageConstructor.SessionMessageType.Logon:
                    return "A";
                case MessageConstructor.SessionMessageType.Logout:
                    return "5";
                case MessageConstructor.SessionMessageType.Heartbeat:
                    return "0";
                case MessageConstructor.SessionMessageType.TestRequest:
                    return "1";
                case MessageConstructor.SessionMessageType.Resend:
                    return "2";
                case MessageConstructor.SessionMessageType.Reject:
                    return "3";
                case MessageConstructor.SessionMessageType.SequenceReset:
                    return "4";
                default:
                    return "0";
            }
        }

        private string ApplicationMessageCode(MessageConstructor.ApplicationMessageType type)
        {
            switch (type)
            {
                case MessageConstructor.ApplicationMessageType.MarketDataRequest:
                    return "V";
                case MessageConstructor.ApplicationMessageType.MarketDataIncrementalRefresh:
                    return "X";
                case MessageConstructor.ApplicationMessageType.NewOrderSingle:
                    return "D";
                case MessageConstructor.ApplicationMessageType.OrderStatusRequest:
                    return "H";
                case MessageConstructor.ApplicationMessageType.ExecutionReport:
                    return "8";
                case MessageConstructor.ApplicationMessageType.BusinessMessageReject:
                    return "j";
                case MessageConstructor.ApplicationMessageType.RequestForPosition:
                    return "AN";
                case MessageConstructor.ApplicationMessageType.PositionReport:
                    return "AP";
                default:
                    return "0";
            }
        }

        public enum SessionMessageType
        {
            Logon,
            Logout,
            Heartbeat,
            TestRequest,
            Resend,
            Reject,
            SequenceReset,
        }

        public enum ApplicationMessageType
        {
            MarketDataRequest,
            MarketDataIncrementalRefresh,
            NewOrderSingle,
            OrderStatusRequest,
            ExecutionReport,
            BusinessMessageReject,
            RequestForPosition,
            PositionReport,
        }

        public enum SessionQualifier
        {
            QUOTE,
            TRADE,
        }
    }
}
