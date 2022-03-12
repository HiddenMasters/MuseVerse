using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using API.Models;

public class Private : MonoBehaviour
{
    // private const string BaseURL = "http://museverse.kro.kr/api";
    private const string BaseURL = "http://0.0.0.0:8080/api";

    public static IEnumerator PostPrivate(int item, int num)
    {
        const string path = "/private";
        PrivateExhibitionCreateSerializer serializer = new PrivateExhibitionCreateSerializer(item, num);
        string json = JsonUtility.ToJson(serializer);
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        
        UnityWebRequest request = UnityWebRequest.Post(BaseURL + path, json);
        request.uploadHandler = new UploadHandlerRaw(bytes);
        request.SetRequestHeader("Authorization", AtomManager.Token);
        request.SetRequestHeader("Content-Type", "application/json");
        request.timeout = 1;

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            PrivateExhibitionSerializer privateExhibition =
                JsonUtility.FromJson<PrivateExhibitionSerializer>(request.downloadHandler.text);
            // 전시 완료 Alert
        }
    }

    public static IEnumerator GetPrivate(int privateId)
    {
        string path = String.Format("/private/{0}", privateId);
        
        UnityWebRequest request = UnityWebRequest.Get(BaseURL + path);
        request.SetRequestHeader("Authorization", AtomManager.Token);
        request.SetRequestHeader("Content-Type", "application/json");
        request.timeout = 1;

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            PrivateExhibitionDetailSerializer privateExhibition =
                JsonUtility.FromJson<PrivateExhibitionDetailSerializer>(request.downloadHandler.text);
        }
    }

    public static IEnumerator DeletePrivate(int privateId)
    {
        string path = String.Format("/private/{0}", privateId);
        
        UnityWebRequest request = UnityWebRequest.Delete(BaseURL + path);
        request.SetRequestHeader("Authorization", AtomManager.Token);
        request.SetRequestHeader("Content-Type", "application/json");
        request.timeout = 1;

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            // 정상 삭제 Alert
        }
    }
}
