using System.Collections;
using Newtonsoft.Json;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AppConfig : MonoBehaviour
{
    private readonly string _adminUrl = "http://185.173.104.193:90";
    public string adminID;
    [ReadOnly] public bool serverConnected = false;

    public void Start()
    {
        if (string.IsNullOrEmpty(_adminUrl))
        {
            Debug.LogError("Please enter baseURL in Server Manager");
        }
        else if (string.IsNullOrEmpty(adminID))
        {
            Debug.LogError("Please enter a valid ID");
        }
        else
        {
            StartCoroutine(LoginCoroutine(adminID));
        }
    }

    private IEnumerator LoginCoroutine(string ID)
    {
        var admin = new AdminPanel { ID = ID };
        string json = JsonConvert.SerializeObject(admin);

        using (UnityWebRequest www = new UnityWebRequest(_adminUrl + "/loginAdmin/inAppLogin", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            www.certificateHandler = new BypassCertificate();

            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    print(www.downloadHandler.text);
                    LoginResponse response = JsonConvert.DeserializeObject<LoginResponse>(www.downloadHandler.text);
                    admin = response.user;
                    serverConnected = true;
                    PlayerPrefs.SetString("schemaID", admin.schemaID);
                    PlayerPrefs.SetString("baseURL", admin.baseURL);

                    Debug.Log("Login successful. SchemaID: " + admin.schemaID);
                    Debug.Log("Login successful. BaseURL: " + admin.baseURL);
                }
                catch (JsonException jsonEx)
                {
                    Debug.LogError("JSON Parsing Error: " + jsonEx.Message);
                }
            }
            else
            {
                Debug.LogError("Login failed: " + www.error);
            }
        }
    }

    [System.Serializable]
    public class AdminPanel
    {
        public string ID;
        public string schemaID;
        public string baseURL;
    }

    [System.Serializable]
    public class LoginResponse
    {
        public AdminPanel user;
    }

    private class BypassCertificate : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
}
