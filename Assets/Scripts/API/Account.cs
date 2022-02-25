using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Account : MonoBehaviour
{
    private const string BaseURL = "http://0.0.0.0:8080/api";
    private static string _token;
    
    public static IEnumerator CreateAttendance()
    {
        const string path = "accounts/attendance";

        UnityWebRequest request = UnityWebRequest.Post(BaseURL + path, "");
        request.SetRequestHeader("Authorization", _token);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
        }
    }

    public static IEnumerator GetAttendances()
    {
        const string path = "accounts/attendance";
        
        UnityWebRequest request = UnityWebRequest.Get(BaseURL + path);
        request.SetRequestHeader("Authorization", _token);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            // TODO: List<Attendance> 생성
        }

    }
}
