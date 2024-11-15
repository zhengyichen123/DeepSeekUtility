using System;
using System.Text;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;


namespace DeepSeekUtility
{
    public class DeepSeekReq
    {
        private string _url = "https://api.deepseek.com/chat/completions";
        private string _apiKey = "your api key";

        public IEnumerator PostReq<T>(string json, Action<T> callback, Action<string> errorCallback/*游戏错误处理*/)
        {
            UnityWebRequest webRequest = new UnityWebRequest(_url, "POST");

            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _apiKey);

            byte[] bytes = new UTF8Encoding().GetBytes(json);

            webRequest.uploadHandler = new UploadHandlerRaw(bytes);
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            webRequest.disposeDownloadHandlerOnDispose = true;
            webRequest.disposeUploadHandlerOnDispose = true;

            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error: " + webRequest.error);
                    errorCallback?.Invoke(webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    int statusCode = (int)webRequest.responseCode;
                    string errorMessage = webRequest.error;
                    Debug.LogError($"HTTP Error ({statusCode}): {errorMessage}");

                    // 根据不同的状态码进行处理
                    switch (statusCode)
                    {
                        case 400:
                            Debug.LogError("Bad Request: " + errorMessage);
                            errorCallback?.Invoke("Bad Request: " + errorMessage);
                            break;
                        case 401:
                            Debug.LogError("Unauthorized: " + errorMessage);
                            errorCallback?.Invoke("Unauthorized: " + errorMessage);
                            break;
                        case 403:
                            Debug.LogError("Forbidden: " + errorMessage);
                            errorCallback?.Invoke("Forbidden: " + errorMessage);
                            break;
                        case 404:
                            Debug.LogError("Not Found: " + errorMessage);
                            errorCallback?.Invoke("Not Found: " + errorMessage);
                            break;
                        case 429:
                            Debug.Log("Too Many Requests, retrying...");
                            yield return PostReq(json, callback, errorCallback);
                            break;
                        case 500:
                            Debug.LogError("Internal Server Error: " + errorMessage);
                            errorCallback?.Invoke("Internal Server Error: " + errorMessage);
                            break;
                        case 502:
                            Debug.LogError("Bad Gateway: " + errorMessage);
                            errorCallback?.Invoke("Bad Gateway: " + errorMessage);
                            break;
                        case 503:
                            Debug.LogError("Service Unavailable: " + errorMessage);
                            errorCallback?.Invoke("Service Unavailable: " + errorMessage);
                            break;
                        case 504:
                            Debug.LogError("Gateway Timeout: " + errorMessage);
                            errorCallback?.Invoke("Gateway Timeout: " + errorMessage);
                            break;
                        default:
                            Debug.LogError($"HTTP Error ({statusCode}): {errorMessage}");
                            errorCallback?.Invoke($"HTTP Error ({statusCode}): {errorMessage}");
                            break;
                    }

                    // 进一步处理响应内容
                    if (_url.EndsWith("/completions"))
                    {
                        try
                        {
                            DeepSeekResErrData deepSeekResError = JsonMapper.ToObject<DeepSeekResErrData>(webRequest.downloadHandler.text);
                            Debug.LogError(deepSeekResError.error.message);
                            errorCallback?.Invoke(deepSeekResError.error.message);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError("Failed to parse error response: " + ex.Message);
                            errorCallback?.Invoke("Failed to parse error response: " + ex.Message);
                        }
                    }
                    else
                    {
                        Debug.LogError("Full Content: " + webRequest.downloadHandler.text);
                        errorCallback?.Invoke(webRequest.downloadHandler.text);
                    }
                    break;
                case UnityWebRequest.Result.Success:
                    if (webRequest.downloadHandler.text.StartsWith("{\"error\":"))
                    {
                        try
                        {
                            DeepSeekResErrData deepSeekResError = JsonMapper.ToObject<DeepSeekResErrData>(webRequest.downloadHandler.text);
                            Debug.LogError(deepSeekResError.error.message);
                            errorCallback?.Invoke(deepSeekResError.error.message);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError("Failed to parse error response: " + ex.Message);
                            errorCallback?.Invoke("Failed to parse error response: " + ex.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            Debug.Log(webRequest.downloadHandler.text);
                            T obj = JsonMapper.ToObject<T>(webRequest.downloadHandler.text);
                            callback(obj);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError("Failed to parse success response: " + ex.Message);
                            errorCallback?.Invoke("Failed to parse success response: " + ex.Message);
                        }
                    }
                    break;
            }
            webRequest.Dispose();
        }
    }
}

