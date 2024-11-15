using System.Collections.Generic;

namespace DeepSeekUtility
{
    public class Chat
    {
        public enum Speaker{ User, Assistant }
        
        public List<MsgData> messages = new List<MsgData>();
        
        private List<int> _messageTokens = new List<int>();
        
        public Chat()
        {
            messages.Add(new MsgData("system", NpcLoad.npcPrompt));
            _messageTokens.Add(NpcLoad.npcTokens);
        }
        
        // 追加聊天内容
        public void AppendMessage(Speaker skr, string message, int tokens)
        {
            switch (skr)
			{
                case Speaker.User:
                {
                    messages.Add(new MsgData("user", message));
                    break;
                }
                case Speaker.Assistant:
                {
                    messages.Add(new MsgData("assistant", message));
                    break;
                }
			}
            if (tokens > 0)
            {
                _messageTokens.Add(tokens);
            }
        }
        
        // 移除最先前的对话内容
        public int RemoveFirConv()
		{
			int result = _messageTokens[1];
			messages.RemoveAt(1);
			messages.RemoveAt(1);
			_messageTokens.RemoveAt(1);
			return result;
		}
        
        // 删减上下文内容
        public int ReduceToken(int maxTokens, int totalTokens)
        {
            if(totalTokens <= maxTokens)  return totalTokens;
            while(totalTokens > maxTokens)
            {
                totalTokens -= RemoveFirConv();
            }
            return totalTokens;
        }

        // 导出聊天记录
    }  
  
}

