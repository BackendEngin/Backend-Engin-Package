using System;
using System.Threading.Tasks;
using UnityEngine;

public class TriggerEvent : Request
{
    public static async Task<APIResponse> Trigger(string eventName, string eventType)
    {
        RequestData.requestURL = "/eventManager/logEvent";
        RequestData.requestType = "POST";
        RequestData.schemaID = PlayerPrefs.GetString("schemaID");
        RequestData.jwToken = PlayerPrefs.GetString("playerJWToken");
        RequestData.user.id = PlayerPrefs.GetInt("playerID");
        RequestData.events = new Events
        {
            eventName = eventName,
            eventType = eventType
        };

        APIResponse response = await SendRequest.SendAPIRequest(RequestData);
        return response;
    }

}
