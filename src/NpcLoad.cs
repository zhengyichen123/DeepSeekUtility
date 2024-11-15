namespace DeepSeekUtility
{
    // 加载角色属性
    // 实例化该类，npcPrompt就是角色属性
    public class NpcLoad
    {
        public static NpcData npcData = NpcData.instance;

        public static string npcPrompt => FormatNpcPrompt();
        public static int npcTokens => (int)(npcPrompt.Length * 0.6);

        private static string FormatNpcPrompt()
        {
            return $"{npcData.name} is a {npcData.age} year old {npcData.sex} {npcData.identity} with {npcData.bwh} body height and {npcData.personality} personality. {npcData.description}. {npcData.mission}. {npcData.rule}";
            // 有待考量
        }
   
    }
}