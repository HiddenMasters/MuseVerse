using System;
using System.Collections;
using UnityEngine;
using API.Models;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    // private const string BaseURL = "http://museverse.kro.kr/api";
    private const string BaseURL = "http://0.0.0.0:8080/api";

    // TODO: Add Upload File Function
    public static IEnumerator PostItem(string name, string format, float price)
    {
        const string path = "/item";
        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("format", format);
        form.AddField("price", price.ToString());
        
        UnityWebRequest request = UnityWebRequest.Post(BaseURL + path, form);
        request.SetRequestHeader("Authorization", AtomManager.Token);
        request.SetRequestHeader("Content-Type", "multipart/form-data");
        request.timeout = 1;

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
    
    public static IEnumerator GetItem(int itemId)
    {
        string path = String.Format("/item/{0}", itemId);
        
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
            ItemDetailSerializer item = JsonUtility.FromJson<ItemDetailSerializer>(request.downloadHandler.text);
            // GameObject.Find("Item Info Name Text").GetComponent<Text>().text = item.name;
            // GameObject.Find("Item Info Author Text").GetComponent<Text>().text = item.author;
        }
    }

    public static IEnumerator DeleteItem(int itemId)
    {
        string path = String.Format("/item/{0}", itemId);
        
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
            // 삭제 성공 Alert
        }
    }

    public static IEnumerator GetItemImage(int itemId, SpriteRenderer renderer, Image image)
    {
        string path = String.Format("/item/{0}/image", itemId);

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(BaseURL + path);
        request.SetRequestHeader("Authorization", AtomManager.Token);
        request.timeout = 1;

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            // 특정한 Sprite & Image를 받아서 다운받은 이미지를 넣어주는 형식
            Texture2D texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 640, 480), new Vector2(0.5f, 0.5f));
            renderer.sprite = sprite;
            image.sprite = sprite;
        }
    }
}