using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace Wordle_Clone
{
    public class HandleApiCalls : MonoBehaviour
    {
        private OnlineWord _currentWord;
        public OnlineWord currentWord { get { return _currentWord; } }

        public void ResetCurrentWord()
        {
            _currentWord = null;
        }

        public void GetOnlineWord(ref string uri)
        {
            _ = StartCoroutine(GetRequest(uri));
        }

        private IEnumerator GetRequest(string uri)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                yield return webRequest.SendWebRequest();

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError($"Something Went Wrong : {webRequest.error}");
                        break;

                    case UnityWebRequest.Result.Success:
                        _currentWord = JsonConvert.DeserializeObject<OnlineWord>(webRequest.downloadHandler.text);
                        Debug.Log($"Got Word : {currentWord.word}");
                        break;
                }
            }
        }
    }

    // OnlineWord myDeserializedClass = JsonConvert.DeserializeObject<OnlineWord>(myJsonResponse);
    public class OnlineWord
    {
        public string word { get; set; }
    }
}