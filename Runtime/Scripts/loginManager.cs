using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace BackendEngine
{
    public class LoginManager : MonoBehaviour
    {
        private string baseURL;

        void Start()
        {
            baseURL = PlayerPrefs.GetString("baseURL");
        }

        public void LoginUser(string email, string password)
        {
            StartCoroutine(LoginCoroutine(email, password));
        }

        private IEnumerator LoginCoroutine(string email, string password)
        {
            UserLogin userLogin = new UserLogin
            {
                email = email,
                password = password
            };

            string json = JsonConvert.SerializeObject(userLogin);
            string loginURL = baseURL + "/login";

            using (UnityWebRequest www = new UnityWebRequest(loginURL, "POST"))
            {
                www.SetRequestHeader("Content-Type", "application/json");

                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
                www.uploadHandler = new UploadHandlerRaw(bodyRaw);
                www.downloadHandler = new DownloadHandlerBuffer();

                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    ResponseData response = JsonConvert.DeserializeObject<ResponseData>(www.downloadHandler.text);

                    Debug.Log("Login successful. Player ID: " + response.user.id);
                    Debug.Log("Login successful. Player email: " + response.user.email);
                    Debug.Log("Login successful. Player location: " + response.user.location);
                    Debug.Log("Login successful. Player schemaID: " + response.user.schemaID);
                    Debug.Log("Login successful. Player phoneNumber: " + response.user.phoneNumber);
                    Debug.Log("Login successful. Player label: " + response.user.label);
                    Debug.Log("Login successful. Player tags: " + response.user.tags);
                    Debug.Log("Login successful. Player Username: " + response.user.username);
                }
                else
                {
                    Debug.LogError("Login failed: " + www.error);
                }
            }
        }

        [System.Serializable]
        public class UserLogin
        {
            public string email;
            public string password;
        }

        [System.Serializable]
        public class ResponseData
        {
            public User user;
        }

        [System.Serializable]
        public class User
        {
            public string id;
            public string email;
            public string location;
            public string schemaID;
            public string phoneNumber;
            public string label;
            public string tags;
            public string username;
        }
    }
}
