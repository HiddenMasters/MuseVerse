using System.Collections;
using System.Collections.Generic;
using System.Text;
using API.Models;
using UnityEngine;
using UnityEngine.Networking;

public class Account : MonoBehaviour
{
    // private const string BaseURL = "http://museverse.kro.kr/api";
    private const string BaseURL = "http://0.0.0.0:8080/api";

    public static IEnumerator GetUserProfile()
    {
        const string path = "/accounts/me";

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
            AuthUserProfileSerializer profile =
                JsonUtility.FromJson<AuthUserProfileSerializer>(request.downloadHandler.text);
        }
    }

    public static IEnumerator PutProfileNickname(string nickname)
    {
        const string path = "/accounts/rename";

        AuthRenameSerializer serializer = new AuthRenameSerializer(nickname);
        string json = JsonUtility.ToJson(serializer);
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        
        UnityWebRequest request = UnityWebRequest.Put(BaseURL + path, json);
        request.uploadHandler = new UploadHandlerRaw(bytes);
        request.SetRequestHeader("Authorization", AtomManager.Token);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            AtomManager.ClosePanel();
            // AtomManager.OpenConfirm("title", "message");
        }
    }

    public static IEnumerator GetInventories()
    {
        const string path = "/accounts/inventories";
        
        UnityWebRequest request = UnityWebRequest.Get(BaseURL + path);
        request.SetRequestHeader("Authorization", AtomManager.Token);
        request.timeout = 1;

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            ItemsSerializer items = JsonUtility.FromJson<ItemsSerializer>(request.downloadHandler.text);
            // AtomManager.Inventories = serializer.items;
            // InventoryGroup.SetInventoriesImage();
        }
    }
    
    public static IEnumerator GetExhibitions()
    {
        const string path = "/accounts/exhibitions";
        
        UnityWebRequest request = UnityWebRequest.Get(BaseURL + path);
        request.SetRequestHeader("Authorization", AtomManager.Token);
        request.timeout = 1;

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            ExhibitionsSerializer exhibitions = JsonUtility.FromJson<ExhibitionsSerializer>(request.downloadHandler.text);
            // AtomManager.ExhibitionInventories = serializer.exhibitionInventories;
            // InventoryGroup.SetExhibitionInventories();
        }
    }
    
    public static IEnumerator GetBuyHistories()
    {
        const string path = "/accounts/history/buy";
        
        UnityWebRequest request = UnityWebRequest.Get(BaseURL + path);
        request.SetRequestHeader("Authorization", AtomManager.Token);
        request.timeout = 1;

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            TradesSerializer trades = JsonUtility.FromJson<TradesSerializer>(request.downloadHandler.text);
            // AtomManager.BuyHistories = serializer.histories;
        }
    }
    
    public static IEnumerator GetSellHistories()
    {
        const string path = "/accounts/history/sell";
        
        UnityWebRequest request = UnityWebRequest.Get(BaseURL + path);
        request.SetRequestHeader("Authorization", AtomManager.Token);
        request.timeout = 1;

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            TradesSerializer trades = JsonUtility.FromJson<TradesSerializer>(request.downloadHandler.text);
            // AtomManager.SellHistories = serializer.histories;
        }
    }
    
    public static IEnumerator PostAttendances()
    {
        const string path = "accounts/attendance";
        
        UnityWebRequest request = UnityWebRequest.Get(BaseURL + path);
        request.SetRequestHeader("Authorization", AtomManager.Token);
        request.timeout = 1;

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            // 출석 체크 Confirm 창 확인
        }

    }
    
    public static IEnumerator GetAttendances()
    {
        const string path = "/accounts/attendances";
        
        UnityWebRequest request = UnityWebRequest.Get(BaseURL + path);
        request.SetRequestHeader("Authorization", AtomManager.Token);
        request.timeout = 1;

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            AttendancesSerializer attendances =
                JsonUtility.FromJson<AttendancesSerializer>(request.downloadHandler.text);
            
            // 출석 확인 창
        }
    }
}
