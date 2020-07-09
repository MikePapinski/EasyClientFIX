using EasyClientFIXlib.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyClientFIXlib
{
    class SessionManager
    {
        public string priceHostName { get; set; }
        public int pricePort { get; set; }
        public string pricePassword { get; set; }
        public string priceSenderCompID { get; set; }
        public string priceTargetCompID { get; set; }
        public string priceSenderSubID { get; set; }
        public string tradeHostName { get; set; }
        public int tradePort { get; set; }
        public string tradePassword { get; set; }
        public string tradeSenderCompID { get; set; }
        public string tradeTargetCompID { get; set; }
        public string tradeSenderSubID { get; set; }

        public SessionQUOTE sessionQuote;
        public SessionTRADE sessionTrade;


        public SessionManager(string _priceHostName, int _pricePort, string _pricePassword, string _priceSenderCompID, string _priceTargetCompID, string _priceSenderSubID,
                              string _tradeHostName, int _tradePort, string _tradePassword, string _tradeSenderCompID, string _tradeTargetCompID, string _tradeSenderSubID)
        {
            priceHostName = _priceHostName;
            pricePort = _pricePort;
            pricePassword = _pricePassword;
            priceSenderCompID = _priceSenderCompID;
            priceTargetCompID = _priceTargetCompID;
            priceSenderSubID = _priceSenderSubID;
            tradeHostName = _tradeHostName;
            tradePort = _tradePort;
            tradePassword = _tradePassword;
            tradeSenderCompID = _tradeSenderCompID;
            tradeTargetCompID = _tradeTargetCompID;
            tradeSenderSubID = _tradeSenderSubID;
            sessionQuote = new SessionQUOTE(priceHostName, pricePort, pricePassword, priceSenderCompID, priceTargetCompID, priceSenderSubID);
            sessionTrade = new SessionTRADE(tradeHostName, tradePort, tradePassword, tradeSenderCompID, tradeTargetCompID, tradeSenderSubID);
        }

        public void Connect()
        {
            sessionQuote.Connect();
            sessionTrade.Connect();
            sessionQuote.Login();
            sessionTrade.Login();
            sessionQuote.StartHeartBeat();
            sessionTrade.StartHeartBeat();
        }

        public void Disconnect()
        {
            if (sessionQuote.streamEnabled)
            {
                sessionQuote.StopStreamListener();
            }
            if (sessionTrade.streamEnabled)
            {
                sessionTrade.StopStreamListener();
            }
            sessionQuote.StopHeartBeat();
            sessionTrade.StopHeartBeat();
            sessionQuote.Logout();
            sessionTrade.Logout();
            sessionQuote.Disconnect();
            sessionTrade.Disconnect();
        }

        public void SubscribeQuote(long symbol)
        {
            sessionQuote.SubscribeSymbol(symbol);
        }
        public void UnsubscribeQuote(long symbol)
        {
            sessionQuote.UnsubscribeSymbol(symbol);
        }
        public void StartStreamingQuotes()
        {
            sessionQuote.StartStreamListener();
        }



    }
}
