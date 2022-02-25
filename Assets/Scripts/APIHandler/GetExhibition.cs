using System;
using System.Collections;
using System.Text;
using API.Models;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class GetExhibition : MonoBehaviour
{
    public int hall;
    public int num;
    
    private SpriteRenderer _spriteRenderer;
    private ExhibitionSerializer _exhibition;
    private bool _flag;
    
    private const string BaseURL = "http://0.0.0.0:8080/api";

    void Awake()
    {
        StartCoroutine(getImage());
        _spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(getExhibition());
        _flag = false;
    }

    private IEnumerator getExhibition()
    {
        string path = String.Format("/exhibition/{0}/{1}", hall, num);
        
        UnityWebRequest request = UnityWebRequest.Get(BaseURL + path);
        // request.SetRequestHeader("Authorization", AtomManager.Token);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            _exhibition = null;
        }
        else
        {
            _exhibition = JsonUtility.FromJson<ExhibitionSerializer>(request.downloadHandler.text);
        }
    }

    private IEnumerator getImage()
    {
        string path = String.Format("/exhibition/image/{0}/{1}", hall, num);

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(BaseURL + path);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            _spriteRenderer.sprite = sprite;
        }
    }
}
