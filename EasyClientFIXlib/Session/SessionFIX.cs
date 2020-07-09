using System;
using System.Text;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Collections.Specialized;
using EasyClientFIXlib.Translation;

namespace EasyClientFIXlib.Session
{
    public class SessionFIX
    {
        // SESSION DETAILS
        public string hostName { get; set; }
        public int port { get; set; }
        public string password { get; set; }
        public string senderCompID { get; set; }
        public string targetCompID { get; set; }
        public string senderSubID { get; set; }
        public string username { get; set; }
        public int sequenceNumber { get; set; }
        public int heartbeatSecondsInterval { get; set; }

        // SESSION MANAGEMENT
        public TcpClient client;
        public SslStream streamSSL;
        public MessageConstructor msgConstructor;
        public bool connected;

        // SESSION THREADS
        public Thread heartbeatThread;
        public bool heartbeatEnabled;
        public Thread streamThread;
        public bool streamEnabled;

        // MANAGING EVENTS
        public ResponseMsgFIX lastMsg;
        public event EventHandler<NameValueCollection> OnNewMessage;
        public event EventHandler SendHeartBeat;


        #region CONNECT & DISCONNECT
        public void Connect()
        {
            client = new TcpClient(hostName, port);
            streamSSL = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
            streamSSL.AuthenticateAsClient(hostName);
            msgConstructor = new MessageConstructor(hostName, username, password, senderCompID, senderSubID, targetCompID);

        }
        public void Disconnect()
        {
            streamSSL.Close();
            client.Close();
        }
        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;
            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);
            return false;
        }
        #endregion

        #region READ MSG & SEND MSG
        public string SendMessage(string message)
        {
            var byteArray = Encoding.ASCII.GetBytes(message);
            streamSSL.Write(byteArray, 0, byteArray.Length);
            sequenceNumber++;
            return ReadMessage();
        }
        private string ReadMessage()
        {
            var buffer = new byte[4096];
            int counter = 0;
            try
            {
                while (!client.GetStream().DataAvailable & !streamSSL.CanRead & counter < 10)
                {
                    try
                    {
                        Thread.Sleep(100);
                        counter++;
                    }
                    catch
                    {
                        break;
                    }
                }
                if (client.GetStream().DataAvailable && streamSSL.CanRead)
                    streamSSL.Read(buffer, 0, 4096);
            }
            catch
            {
                Console.WriteLine("Something went wrong with reading FIX stream");
            }
            var returnMessage = Encoding.ASCII.GetString(buffer);
            return returnMessage;

        }
        #endregion

        #region STREAM MANAGEMENT
        public void StartHeartBeat()
        {
            heartbeatEnabled = true;
            ThreadStart start_heartbeat = delegate ()
            {
                while (heartbeatEnabled)
                {
                    try
                    {
                        SendHeartBeat(this, EventArgs.Empty);
                        Thread.Sleep(1000 * heartbeatSecondsInterval);
                    }
                    catch
                    {
                        Console.WriteLine("Sending HeartBeat failed. Aborting the loop...");
                        break;
                    }
                }

            };
            heartbeatThread = new Thread(start_heartbeat);
            heartbeatThread.IsBackground = true;
            heartbeatThread.Start();
        }
        public void StopHeartBeat()
        {
            heartbeatEnabled = false;
            if (heartbeatThread.IsAlive)
            {
                heartbeatThread.Abort();
            }
        }
        public void StartStreamListener()
        {
            streamEnabled = true;
            ThreadStart start_stream = delegate ()
            {
                while (streamEnabled)
                {
                    try
                    {
                        GenerateMessagesFromString(ReadMessage());
                        Thread.Sleep(50);
                    }
                    catch
                    {
                        Console.WriteLine("Listening to stream failed. Aborting the loop...");
                        break;
                    }
                }
            };
            streamThread = new Thread(start_stream);
            streamThread.IsBackground = true;
            streamThread.Start();
        }
        public void StopStreamListener()
        {
            if (streamThread.IsAlive)
            {
                streamThread.Abort();
            }
        }
        public void GenerateMessagesFromString(string rawMsg)
        {
            NameValueCollection msgContent = new NameValueCollection(); ;
            string msg = rawMsg.Replace("\0", string.Empty);
            string[] splitMsgs = msg.Split('\u0001');
            foreach (string msgs in splitMsgs)
            {
                if (msgs.Count(c => c == '=') > 0)
                {
                    string[] splitMsgTag = msgs.Split('=');
                    string translatedKey = TranslatorFIX.ParserFIX.Parser[int.Parse(splitMsgTag[0])];
                    string translatedVal = splitMsgTag[1];
                    if (translatedKey == "MsgType")
                    {
                        translatedVal = TranslatorFIX.ParserFIX.MsgTypeParser[translatedVal];
                    }
                    msgContent.Add(translatedKey, translatedVal);
                    if (translatedKey == "CheckSum")
                    {
                        OnNewMessage(this, msgContent);
                        msgContent = new NameValueCollection();
                    }
                }
            }
        }
        #endregion


    }
}