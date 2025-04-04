using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace backendEngin
{
    public class UpdateUserInfo : Request
    {
        public static async Task<string> UpdateInfo(string email, int? coins = null, int? highScore = null, int? defenceLevel = null, int? attackLevel = null)
        {
            RequestData.requestURL = "/updateUserInfo";
            RequestData.requestType = "POST";
            RequestData.schemaID = PlayerPrefs.GetString("schemaID");

            var userData = new Dictionary<string, object>
            {
                { "email", email }
            };

            if (coins.HasValue) userData["coins"] = coins.Value;
            if (highScore.HasValue) userData["highScore"] = highScore.Value;
            if (defenceLevel.HasValue) userData["defenceLevel"] = defenceLevel.Value;
            if (attackLevel.HasValue) userData["attackLevel"] = attackLevel.Value;

            RequestData.UpdateUser = userData;

            APIResponse response = await SendRequest.SendAPIRequest(RequestData);
            return response.Data.Message;
        }
    }
}