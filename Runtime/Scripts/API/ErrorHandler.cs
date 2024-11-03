using UnityEngine;

public class ErrorHandler
{

    public void APIErrors(string error, string api)
    {
        Debug.LogError($"[{System.DateTime.Now}] API Error: {api} - {error}");
    }
}
