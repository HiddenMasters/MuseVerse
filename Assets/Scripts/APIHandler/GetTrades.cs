using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using API.Models;
using UnityEngine.Networking;

public class GetTrades : MonoBehaviour
{
    private const string BaseURL = "http://0.0.0.0:8080/api";
    
    public static IEnumerator GetImageURLs()
    {
        string token = AtomManager.Token;

        if (token == null)
        {
            yield return null;
        }

        const string path = "/trades/images";
        
        UnityWebRequest request = UnityWebRequest.Get(BaseURL + path);
        request.SetRequestHeader("Authorization", token);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            ItemsURLSerializer serializers = JsonUtility.FromJson<ItemsURLSerializer>(request.downloadHandler.text);
            Debug.Log(serializers);
        }
    }
}
