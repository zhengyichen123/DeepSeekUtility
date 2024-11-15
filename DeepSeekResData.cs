using System;
using System.Collections.Generic;

namespace DeepSeekUtility
{
    [Serializable]
    public class DeepSeekResData
    {
        //public string id;
        //public int created;
        public List<Choices> choices;
        public Usage usage;
        //public string system_fingerprint;
        //public string object;
        
        [Serializable]
        public class Choices
        {
            public MsgData message;
            //public string finish_reason;
            //public int index;
        }

        [Serializable]
        public class Usage
        {
            public int prompt_tokens;
            public int completion_tokens;
            public int total_tokens;
        }

    }
}