using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ApiService : MonoBehaviour
{
    private const string ApiUrl = "https://catfact.ninja/fact";

    void Start()
    {
        StartCoroutine(FetchDungeonTip());
    }

    private IEnumerator FetchDungeonTip()
    {
        UnityWebRequest request = UnityWebRequest.Get(ApiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"[ApiService] REST API call succeeded: {request.downloadHandler.text}");
        }
        else
        {
            Debug.Log($"[ApiService] REST API call failed: {request.error}");
        }
    }
}
