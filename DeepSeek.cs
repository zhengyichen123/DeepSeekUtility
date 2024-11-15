using UnityEngine.Events;
using UnityEngine;
using LitJson;
using System;


namespace DeepSeekUtility
{
    public class DeepSeek : MonoBehaviour
    {
        private Chat _chat = new Chat();

        private DeepSeekReq _req = new DeepSeekReq();

        private int _maxTokens = 3060;  //chat的最大token

        private string _deepSeekResponse;

        private int _tokens = NpcLoad.npcTokens; //记录每一轮对话的token

        public UnityEvent<string> deepSeekResponse = new UnityEvent<string>();

        public void SendtoDeepSeek(string query, Action<string> errorCallback)
        {
            Debug.Log("Sending to DeepSeek");
            _chat.AppendMessage(Chat.Speaker.User, query, 0);
            string json = JsonMapper.ToJson(new DeepSeekReqData(_chat.messages));
            Debug.Log(json);
            StartCoroutine(_req.PostReq<DeepSeekResData>(json, ResolveDeepSeek, errorCallback));

        }

        private void ResolveDeepSeek(DeepSeekResData res)
        {
            Debug.Log("Received from DeepSeek");
            _deepSeekResponse = res.choices[0].message.content;
            Debug.Log(_deepSeekResponse);
            _tokens = res.usage.completion_tokens - _tokens;
            _chat.AppendMessage(Chat.Speaker.Assistant, _deepSeekResponse, _tokens);
            _tokens = _chat.ReduceToken(_maxTokens, res.usage.total_tokens);
            deepSeekResponse.Invoke(_deepSeekResponse);
        }

        public void StopMessageRequest()
        {
            StopAllCoroutines();
        }

        public string GetResponse()
        {
            return _deepSeekResponse;
        }

        public void ClearMemory()
        {
            _chat = null;
            _chat = new Chat();
        }

        public void ExportChatHistory(string userName, string aiName)
        {
            // to do
        }

        /*test*/
        // private void Start() 
        // {
        //     SendtoDeepSeek("hello", null);
        // }

    }
}