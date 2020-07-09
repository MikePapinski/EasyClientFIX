using EasyClientFIXlib.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyClientFIXlib.Session
{
    public class SessionTRADE : SessionFIX
    {
        public SessionTRADE(string getHostName, int getPort, string getPassword, string getSenderCompID, string getTargetCompID, string getSenderSubID)
        {
            hostName = getHostName;
            port = getPort;
            password = getPassword;
            senderCompID = getSenderCompID;
            targetCompID = getTargetCompID;
            senderSubID = getSenderSubID;
            string[] tempUsername = senderCompID.Split('.');
            username = tempUsername[1];
            sequenceNumber = 1;
            heartbeatSecondsInterval = 30;
            SendHeartBeat += Heartbeat;
        }


        #region TRADE METHODS
        public ResponseMsgFIX NewOrderSingle()
        {
            string msg = msgConstructor.NewOrderSingleMessage(MessageConstructor.SessionQualifier.TRADE, sequenceNumber, "1408471", 1, 1, DateTime.UtcNow.ToString("yyyyMMdd-HH:mm:ss"), 1000, 1, "1");
            string response = SendMessage(msg);
            return new ResponseMsgFIX(response);
        }

        public ResponseMsgFIX OrderStatusRequest()
        {
            string msg = msgConstructor.OrderStatusRequest(MessageConstructor.SessionQualifier.TRADE, sequenceNumber, "1408471");
            string response = SendMessage(msg);
            return new ResponseMsgFIX(response);
        }

        public ResponseMsgFIX RequestForPositions()
        {
            string msg = msgConstructor.RequestForPositions(MessageConstructor.SessionQualifier.TRADE, sequenceNumber, "1408471");
            string response = SendMessage(msg);
            return new ResponseMsgFIX(response);
        }

        public ResponseMsgFIX Login()
        {
            string msg = msgConstructor.LogonMessage(MessageConstructor.SessionQualifier.TRADE, sequenceNumber, heartbeatSecondsInterval, false);
            string response = SendMessage(msg);
            return new ResponseMsgFIX(response);
        }

        public void Heartbeat(object sender, EventArgs e)
        {
            string msg = msgConstructor.HeartbeatMessage(MessageConstructor.SessionQualifier.TRADE, sequenceNumber);
            string response = SendMessage(msg);
        }

        public ResponseMsgFIX TestRequest()
        {
            string msg = msgConstructor.TestRequestMessage(MessageConstructor.SessionQualifier.TRADE, sequenceNumber, sequenceNumber);
            string response = SendMessage(msg);
            return new ResponseMsgFIX(response);
        }

        public ResponseMsgFIX Logout()
        {
            string msg = msgConstructor.LogoutMessage(MessageConstructor.SessionQualifier.TRADE, sequenceNumber);
            string response = SendMessage(msg);
            sequenceNumber = 1;
            return new ResponseMsgFIX(response);
        }

        public ResponseMsgFIX ResendRequest()
        {
            string msg = msgConstructor.ResendMessage(MessageConstructor.SessionQualifier.TRADE, sequenceNumber, sequenceNumber - 1);
            string response = SendMessage(msg);
            return new ResponseMsgFIX(response);
        }

        public ResponseMsgFIX StopOrder()
        {
            string msg = msgConstructor.NewOrderSingleMessage(MessageConstructor.SessionQualifier.TRADE, sequenceNumber, "10", 1, 1, DateTime.UtcNow.ToString("yyyyMMdd-HH:mm:ss"), 1000, 3, "3", 0, (decimal)1.08);
            string response = SendMessage(msg);
            return new ResponseMsgFIX(response);
        }

        public ResponseMsgFIX LimitOrder()
        {
            string msg = msgConstructor.NewOrderSingleMessage(MessageConstructor.SessionQualifier.TRADE, sequenceNumber, "10", 1, 1, DateTime.UtcNow.ToString("yyyyMMdd-HH:mm:ss"), 1000, 2, "3", (decimal)1.08);
            string response = SendMessage(msg);
            return new ResponseMsgFIX(response);
        }
        #endregion


    }
}