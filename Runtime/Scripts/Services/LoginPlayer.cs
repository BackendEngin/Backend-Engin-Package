using System.Threading.Tasks;
using UnityEngine;

public class LoginPlayer : Request
{
    public static async Task<int> Login(string emailOrPhone, string password, bool isEmail = true)
    {
        // Set up the request data
        RequestData.requestURL = "/login";
        RequestData.requestType = "POST";
        RequestData.schemaID = PlayerPrefs.GetString("schemaID");

        // Create and assign user data based on login type
        RequestData.user = new User
        {
            password = password
        };

        // Assign the provided login identifier to either email or phoneNumber field
        if (isEmail)
        {
            RequestData.user.email = emailOrPhone;
        }
        else
        {
            RequestData.user.phoneNumber = emailOrPhone;
        }

        // Send the request and get the response
        APIResponse response = await RendRequest.SendAPIRequest(RequestData);
        
        // Return the player ID if successful, otherwise 0
        return response.IsSuccess ? response.Data.ID : 0;
    }
}