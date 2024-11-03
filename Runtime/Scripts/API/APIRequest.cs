using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class APIRequest : MonoBehaviour
{
    private readonly ErrorHandler _errorHandler = new ErrorHandler();
    private const string ContentTypeHeader = "application/json";

    public async Task<APIResponse> SendAPIRequest(RequestData data)
    {
        string baseURL = PlayerPrefs.GetString("baseURL");
        string json = JsonUtility.ToJson(data);
        byte[] byteData = System.Text.Encoding.UTF8.GetBytes(json);
        
        using (UnityWebRequest request = CreateRequest(baseURL, data.requestURL, data.requestType, byteData))
        {
            // Send the request and await completion
            await SendRequestAsync(request);

            // Parse and handle the response
            return await HandleResponse(request);
        }
    }

    private UnityWebRequest CreateRequest(string baseURL, string requestURL, string requestType, byte[] byteData)
    {
        UnityWebRequest request = new UnityWebRequest(baseURL + requestURL, requestType)
        {
            uploadHandler = new UploadHandlerRaw(byteData),
            downloadHandler = new DownloadHandlerBuffer()
        };
        request.SetRequestHeader("Content-Type", ContentTypeHeader);
        request.certificateHandler = new BypassCertificate();

        return request;
    }

    private async Task SendRequestAsync(UnityWebRequest request)
    {
        var asyncOperation = request.SendWebRequest();
        while (!asyncOperation.isDone)
        {
            await Task.Yield();
        }
    }

    private async Task<APIResponse> HandleResponse(UnityWebRequest request)
    {
        APIResponse response = JsonConvert.DeserializeObject<APIResponse>(request.downloadHandler.text);

        // Check for errors in the request or response
        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError ||
            (response.Data != null && response.Data.Success == "false"))
        {
            _errorHandler.APIErrors(response.Data?.Message ?? "Unknown error", request.url);
            response.IsSuccess = false;
            return response;
        }

        response.IsSuccess = true;
        return response;
    }

    private class BypassCertificate : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true; // Always accept certificates
        }
    }
}
