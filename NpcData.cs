using System.Collections.Generic;


namespace DeepSeekUtility
{
    public class NpcData : CharData
    {
        private static readonly NpcData _instance = new NpcData();
        
        public static NpcData instance => _instance;

        private NpcData()
        {
            name = "NPC";
            sex = "Male";
            age = "25";
            bwh = "170cm 65kg";
            identity = "I am a NPC";
            personality = "I am a NPC";
            description = "I am a NPC";
            mission = "I am a NPC";
            rule = "I am a NPC";
            tone = new List<ConvData>();
        }
    }
}