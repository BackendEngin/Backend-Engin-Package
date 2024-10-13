using System.Collections;
using System.Collections.Generic;
using BackendEngine;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class registerPlayer : MonoBehaviour, IUserManager
{
    public InputField emailInput;
    public InputField passwordInput;
    public InputField locationInput;
    public InputField usernameInput;
    public InputField phoneNumberInput;
    public InputField labelInput;
    public InputField tagsInput;

    private string baseURL;

    void Start()
    {
        baseURL = PlayerPrefs.GetString("baseURL");
    }

    public void RegisterUser(string email, string password, string location, string username, string phoneNumber,
        string label, string tags)
    {
        StartCoroutine(RegisterUserCoroutine(email, password, location, username, phoneNumber, label, tags));
    }

    public void LoginUser(string email, string password)
    {
        // This class does not handle login, so leave this method empty or throw a NotImplementedException
        throw new System.NotImplementedException();
    }

    private IEnumerator RegisterUserCoroutine(string email, string password, string location, string username,
        string phoneNumber, string label, string tags)
    {
        // Create a User object with the registration data
        User newUser = new User
        {
            email = email,
            password = password,
            location = location,
            username = username,
            phoneNumber = phoneNumber,
            label = label,
            tags = tags
        };

        // Add schemaID if needed
        newUser.schemaID = PlayerPrefs.GetString("schemaID");

        // Serialize User object to JSON
        string json = JsonConvert.SerializeObject(newUser);

        // Construct the URL for the register API endpoint
        string registerURL = baseURL + "/register";

        // Create UnityWebRequest object
        using (UnityWebRequest www = new UnityWebRequest(registerURL, "POST"))
        {
            // Set headers
            www.SetRequestHeader("Content-Type", "application/json");

            // Attach JSON data to the request
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();

            // Send the request
            yield return www.SendWebRequest();

            // Check for errors
            if (www.result == UnityWebRequest.Result.Success)
            {
                // Deserialize JSON response
                User registeredUser = JsonConvert.DeserializeObject<User>(www.downloadHandler.text);

                // Example: Store user ID in PlayerPrefs
                PlayerPrefs.SetInt("playerID", registeredUser.id);

                Debug.Log("Register successful. Player ID: " + registeredUser.id);
                Debug.Log("Register successful. Player email: " + registeredUser.email);
                // Add more logs as needed for other returned data

                // Optionally, proceed with login after successful registration
                // LoginUser(email, password);
            }
            else
            {
                Debug.LogError("Register failed: " + www.error);
            }
        }
    }

    // Define your user data structure
    [System.Serializable]
    public class User
    {
        public int id; // Assuming the ID is returned as an integer
        public string email;
        public string password; // Note: Sending passwords in clear text over HTTP is not secure
        public string location;
        public string username;
        public string phoneNumber;
        public string label;
        public string schemaID;
        public string tags;
        // Add more fields as per your registration API response
    }
}


