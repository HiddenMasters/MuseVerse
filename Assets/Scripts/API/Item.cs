using System;
using System.Collections;
using UnityEngine;
using UnityEditor.Networking;
using API.Models;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    private const string BaseURL = "http://0.0.0.0:8080/api";
    private static string _token;

    public static IEnumerator CreateItem(string name, string format, float price)
    {
        const string path = "/item";
        WWWForm form = new WWWForm();
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
    
    public static IEnumerator GetItemById(int id)
    {
        string path = String.Format("/item/{0}", id);
        
        UnityWebRequest request = UnityWebRequest.Get(BaseURL + path);
        request.SetRequestHeader("Authorization", AtomManager.Token);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            ExhibitionItemSerializer serializer = JsonUtility.FromJson<ExhibitionItemSerializer>(request.downloadHandler.text);
            GameObject.Find("Item Info Name Text").GetComponent<Text>().text = serializer.name;
            GameObject.Find("Item Info Author Text").GetComponent<Text>().text = serializer.author;
        }
    }

    public static IEnumerator GetImageByItem(int id, SpriteRenderer renderer, Image image)
    {
        string path = String.Format("/item/{0}/image", id);

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(BaseURL + path);
        // request.SetRequestHeader("Authorization", AtomManager.Token);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 640, 480), new Vector2(0.5f, 0.5f));
            renderer.sprite = sprite;
            image.sprite = sprite;
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