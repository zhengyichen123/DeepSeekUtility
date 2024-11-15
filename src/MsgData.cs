namespace DeepSeekUtility
{
    public class MsgData
    {
        public string role;
        public string content;
        
        public MsgData(string role, string content)
        {
            this.role = role;
            this.content = content;
        }
        public MsgData(){}
    }

}