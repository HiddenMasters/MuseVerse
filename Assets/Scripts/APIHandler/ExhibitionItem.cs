using System;
using System.Collections;
using System.Text;
using API.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;


public class ExhibitionItem : MonoBehaviour
{
    // TODO: Needs Refactoring
    public int hall;
    public int num;
    
    private SpriteRenderer _spriteRenderer;
    private Image _image;
    private ExhibitionDetailSerializer _exhibition;
    private Sprite _sprite;
    private Text _name, _author, _owner, _price, _expire;
    private bool _flag;
    
    // private const string BaseURL = "http://museverse.kro.kr/api";
    private const string BaseURL = "http://0.0.0.0:8080/api";

    void Awake()
    {
        SetImage();
    }

    public void SetImage()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _image = GetComponent<Image>();
        StartCoroutine(GetExhibitionImage());
        StartCoroutine(GetExhibition());
    }
    
    private void OnMouseDown()
    {
        AtomManager.clickedHall = hall;
        AtomManager.clickedNum = num;
        
        if (_exhibition != null)
        {
            AtomManager.OpenPanel("Trade Group");
            TradeGroup.SetImage(_sprite);
            TradeGroup.SetDetail(_exhibition);
        }
        else
        {
            AtomManager.OpenPanel("Empty Exhibition");
        }
    }

    private void ShowTradeGroup()
    {
        AtomManager.OpenPanel("Trade Group");
    }

    public void ClosePanel()
    {
        AtomManager.ClosePanel();
    }
    
    private IEnumerator GetExhibition()
    {
        string path = String.Format("/exhibition/{0}/{1}", hall, num);
        
        UnityWebRequest request = UnityWebRequest.Get(BaseURL + path);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        
        if (request.result != UnityWebRequest.Result.Success)
        {
            _exhibition = null;
        }
        else
        {
            ExhibitionDetailSerializer exhibition =
                JsonUtility.FromJson<ExhibitionDetailSerializer>(request.downloadHandler.text);
            _exhibition = exhibition;
        }
    }

    private IEnumerator GetExhibitionImage()
    {
        string path = String.Format("/exhibition/{0}/{1}/image", hall, num);

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(BaseURL + path);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
            _sprite = Sprite.Create(texture, new Rect(0, 0, 640, 480), new Vector2(0.5f, 0.5f));
            _spriteRenderer.sprite = _sprite;
            _image.sprite = _sprite;
        }
    }
}