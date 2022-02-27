using System.Collections;
using System.Collections.Generic;
using API.Models;
using UnityEngine;
using UnityEngine.Networking;

public class Account : MonoBehaviour
{
    private const string BaseURL = "http://0.0.0.0:8080/api";
    
    public static IEnumerator CreateAttendance()
    {
        const string path = "accounts/attendance";

        UnityWebRequest request = UnityWebRequest.Post(BaseURL + path, "");
        request.SetRequestHeader("Authorization", AtomManager.Token);

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
        request.SetRequestHeader("Authorization", AtomManager.Token);

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
    
    public static IEnumerator GetInventories()
    {
        const string path = "/accounts/inventories";
        
        UnityWebRequest request = UnityWebRequest.Get(BaseURL + path);
        request.SetRequestHeader("Authorization", AtomManager.Token);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            InventoriesSerializer serializer = JsonUtility.FromJson<InventoriesSerializer>(request.downloadHandler.text);
            AtomManager.Inventories = serializer.inventories;
            InventoryGroup.SetInventoriesImage();
        }
    }
    
    public static IEnumerator GetExhibitionInventories()
    {
        const string path = "/accounts/inventories/exhibition";
        
        UnityWebRequest request = UnityWebRequest.Get(BaseURL + path);
        request.SetRequestHeader("Authorization", AtomManager.Token);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            ExhibitionInventoriesSerializer serializer = JsonUtility.FromJson<ExhibitionInventoriesSerializer>(request.downloadHandler.text);
            AtomManager.ExhibitionInventories = serializer.exhibitionInventories;
            InventoryGroup.SetExhibitionInventories();
        }
    }
}
