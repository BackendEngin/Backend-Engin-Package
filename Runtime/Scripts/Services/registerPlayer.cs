using System.Threading.Tasks;
using UnityEngine;

namespace backendEngin
{
    public class RegisterPlayer : Request
    {
        public static async Task<string> Register(string email, string password, string location, string username,
            string phoneNumber,
            string label, string tags)
        {
            // Set up the request data
            RequestData.requestURL = "/register";
            RequestData.requestType = "POST";
            RequestData.schemaID = PlayerPrefs.GetString("schemaID");

            // Create and assign user data
            RequestData.user = new User
            {
                email = email,
                password = password,
                location = location,
                username = username,
                phoneNumber = phoneNumber,
                label = label,
                tags = tags
            };
            APIResponse response = await SendRequest.SendAPIRequest(RequestData);
            if (response.Data.Message == "Registered Successfully!")
            {
                PlayerPrefs.SetInt("playerID", response.Data.ID);
            }
            return response.Data.Message;
        }
    }
}


