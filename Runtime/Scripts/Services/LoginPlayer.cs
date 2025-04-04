using System.Threading.Tasks;
using UnityEngine;

namespace backendEngin
{
    public class LoginPlayer : Request
    {
        public static async Task<string> Login(string emailOrPhone, string password, bool isEmail = true)
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
            APIResponse response = await SendRequest.SendAPIRequest(RequestData);
            if (response.Data.Message == "Login successful")
            {
                // Return the player ID if successful, otherwise 0
                PlayerPrefs.SetInt("playerID",response.Data.ID);
                PlayerPrefs.SetInt("coin",response.Data.Coins);
                PlayerPrefs.SetInt("DefenseLevel",response.Data.DefenceLevel);
                PlayerPrefs.SetInt("DamageLevel",response.Data.AttackLevel);
                PlayerPrefs.SetInt("highscore",response.Data.HighScore);
                PlayerPrefs.SetInt("PlayerLevel",response.Data.PlayerLevel);
                PlayerPrefs.SetString("playerJWToken",response.Data.JwToken);
            }
            return response.Data.Message;
        }
    } 
}
