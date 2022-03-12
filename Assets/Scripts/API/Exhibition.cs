using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using API.Models;
using UnityEngine;
using UnityEngine.Networking;

public class Exhibition : MonoBehaviour
{
    // private const string BaseURL = "http://museverse.kro.kr/api";
    private const string BaseURL = "http://0.0.0.0:8080/api";
    
    public static IEnumerator PostExhibition(int item, float price, int hall, int num)
    {
        const string path = "/exhibition";

        ExhibitionCreateSerializer serializer = new ExhibitionCreateSerializer(item, price, hall, num);
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
            ExhibitionSerializer exhibition = JsonUtility.FromJson<ExhibitionSerializer>(request.downloadHandler.text);
            Func.RefreshExhibition();
            AtomManager.Refresh();
            AtomManager.OpenAlertPanel("정상적으로 전시 되었습니다");
        }
    }

    public static IEnumerator PutExhibition(int exhibitionId)
    {
        string path = String.Format("/exhibition/{0}/extend", exhibitionId);
        
        UnityWebRequest request = UnityWebRequest.Put(BaseURL + path, String.Empty);
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
            // 정상 연기 표시
            ExhibitionSerializer exhibition = JsonUtility.FromJson<ExhibitionSerializer>(request.downloadHandler.text);
            Func.RefreshExhibition();
            AtomManager.OpenAlertPanel("정상적으로 연장 되었습니다!");
        }
    }

    public static IEnumerator DeleteExhibition(int exhibitionId)
    {
        string path = String.Format("/exhibition/{0}", exhibitionId);
        
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
            // 정상 삭제 처리
        }
    }
}
