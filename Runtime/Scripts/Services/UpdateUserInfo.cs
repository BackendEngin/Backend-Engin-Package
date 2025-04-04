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

            var updateUserData = new UpdateUserData
            {
                email = email,
                coins = coins ?? 0,
                highScore = highScore ?? 0,
                attackLevel = attackLevel ?? 0,
                defenceLevel = defenceLevel ?? 0
            };

            if (coins.HasValue) updateUserData.coins = coins.Value;
            if (highScore.HasValue) updateUserData.highScore = highScore.Value;
            if (defenceLevel.HasValue) updateUserData.defenceLevel = defenceLevel.Value;
            if (attackLevel.HasValue) updateUserData.attackLevel = attackLevel.Value;

            RequestData.updateUser = updateUserData;

            APIResponse response = await SendRequest.SendAPIRequest(RequestData);
            return response.Data.Message;
        }

    }
}