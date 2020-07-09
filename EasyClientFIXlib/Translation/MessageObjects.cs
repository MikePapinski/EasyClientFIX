using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyClientFIXlib.Translation
{
    public class ResponseMsgFIX
    {
        public NameValueCollection msgContent;
        public DateTime timeStamp;

        public ResponseMsgFIX(string rawMsg)
        {
            msgContent = new NameValueCollection();
            timeStamp = DateTime.UtcNow;
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
                }
            }


        }
    }
}
