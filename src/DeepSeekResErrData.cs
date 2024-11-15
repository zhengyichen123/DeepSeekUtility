namespace DeepSeekUtility
{
    [System.Serializable]
    public class DeepSeekResErrData
    {
        public Error error;
                
        [System.Serializable]
        public class Error
        {
            public string message;
            public string type;
            public object param;
            public string code;    
        }
    }
}