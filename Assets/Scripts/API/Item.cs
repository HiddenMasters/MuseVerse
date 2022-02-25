using System.Collections;
using UnityEngine;
using UnityEditor.Networking;
using API.Models;
using UnityEngine.Networking;

public class Item : MonoBehaviour
{
    private const string BaseURL = "http://0.0.0.0:8080/api";
    private static string _token;

    public static IEnumerator CreateItem(string name, string format, float price)
    {
        const string path = "/item";
        WWWForm form = new WWWForm();
        // TODO: Add File
        // form.AddField("file", );
        form.AddField("name", name);
        form.AddField("format", format);
        form.AddField("price", price.ToString());
        
        UnityWebRequest request = UnityWebRequest.Post(BaseURL + path, form);
        request.SetRequestHeader("Authorization", _token);
        request.SetRequestHeader("Content-Type", "multipart/form-data");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            ItemSerializer serializer = JsonUtility.FromJson<ItemSerializer>(request.downloadHandler.text);
        }
    }

    public static IEnumerator GetItemURL(int itemId)
    {
        string path = string.Format("/item/{0}/image", itemId);
        
        UnityWebRequest request = UnityWebRequest.Get(BaseURL + path);
        request.SetRequestHeader("Authorization", _token);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            
            // 아이템이 존재하지 않은 경우 -> 
        }
        else
        {
            
        }
    }

    public static IEnumerator GetItems()
    {
        const string path = "/items";
        
        UnityWebRequest request = UnityWebRequest.Get(BaseURL + path);
        request.SetRequestHeader("Authorization", _token);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            // TODO: List<Item> 객체를 받을 수 있게 수정
        }
    }
}