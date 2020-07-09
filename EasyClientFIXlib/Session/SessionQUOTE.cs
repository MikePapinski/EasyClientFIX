using EasyClientFIXlib.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyClientFIXlib.Session
{
    public class SessionQUOTE : SessionFIX
    {

        public SessionQUOTE(string getHostName, int getPort, string getPassword, string getSenderCompID, string getTargetCompID, string getSenderSubID)
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


        #region SUBSCRIBE SYMBOL
        public void SubscribeSymbol(long symbol)
        {
            SpotMarketData(symbol, 1);
        }
        public void UnsubscribeSymbol(long symbol)
        {
            SpotMarketData(symbol, 2);
        }
        #endregion


        #region FIX MESSAGES AREA
        public ResponseMsgFIX Login()
        {
            string msg = msgConstructor.LogonMessage(MessageConstructor.SessionQualifier.QUOTE, sequenceNumber, heartbeatSecondsInterval, false);
            string response = SendMessage(msg);
            return new ResponseMsgFIX(response);
        }
        public void Heartbeat(object sender, EventArgs e)
        {
            string msg = msgConstructor.HeartbeatMessage(MessageConstructor.SessionQualifier.QUOTE, sequenceNumber);
            string response = SendMessage(msg);
        }
        public ResponseMsgFIX Logout()
        {
            string msg = msgConstructor.LogoutMessage(MessageConstructor.SessionQualifier.QUOTE, sequenceNumber);
            string response = SendMessage(msg);
            sequenceNumber = 1;
            return new ResponseMsgFIX(response);
        }
        public ResponseMsgFIX ResendRequest()
        {
            string msg = msgConstructor.ResendMessage(MessageConstructor.SessionQualifier.QUOTE, sequenceNumber, sequenceNumber - 1);
            string response = SendMessage(msg);
            return new ResponseMsgFIX(response);
        }
        public ResponseMsgFIX Reject()
        {
            string msg = msgConstructor.RejectMessage(MessageConstructor.SessionQualifier.QUOTE, sequenceNumber, 0);
            string response = SendMessage(msg);
            return new ResponseMsgFIX(response);

        }
        public ResponseMsgFIX SequenceReset()
        {
            string msg = msgConstructor.SequenceResetMessage(MessageConstructor.SessionQualifier.QUOTE, sequenceNumber, 0);
            string response = SendMessage(msg);
            sequenceNumber = 1;
            return new ResponseMsgFIX(response);
        }
        public ResponseMsgFIX SpotMarketData(long symbol, int subscribeType)
        {
            string msg = msgConstructor.MarketDataRequestMessage(MessageConstructor.SessionQualifier.QUOTE, sequenceNumber, symbol.ToString(), subscribeType, 1, 0, 1, symbol);
            string response = SendMessage(msg);
            return new ResponseMsgFIX(response);
        }
        public ResponseMsgFIX MarketDataRequest(string symbol)
        {
            string msg = msgConstructor.MarketDataRequestMessage(MessageConstructor.SessionQualifier.QUOTE, sequenceNumber, symbol, 1, 1, 0, 1, long.Parse(symbol));
            string response = SendMessage(msg);
            return new ResponseMsgFIX(response);
        }
        #endregion

    }
}
