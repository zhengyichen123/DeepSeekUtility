using System;
using System.Collections.Generic;
using UnityEngine;

namespace DeepSeekUtility
{
	[Serializable]
    class DeepSeekReqData
    {
		[SerializeField]
		public List<MsgData> messages;
		public DeepSeekReqData(List<MsgData> messages)
		{
			this.messages = messages;
		}

		public string model = "deepseek-chat";

		public float temperature = 0.9f;

		public int max_tokens = 350;

		public float top_p = 0.55f;

		public float frequency_penalty = 0.03f;

		public float presence_penalty = 1.03f;

		public string stop;

    }
}