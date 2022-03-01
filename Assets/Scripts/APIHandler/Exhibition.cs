using System;
using System.Collections;
using System.Text;
using API.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;


public class Exhibition : MonoBehaviour
{
    // TODO: Needs Refactoring
    public int hall;
    public int num;
    
    private SpriteRenderer _spriteRenderer;
    private Image _image;
    private ExhibitionSerializer _exhibition;
    private Sprite _sprite;
    private GameObject _tradeGroup;
    private Text _name, _author, _owner, _price, _expire;
    private bool _flag;
    
    private const string BaseURL = "http://museverse.kro.kr/api";
    // private const string BaseURL = "http://0.0.0.0:8080/api";

    void Awake()
    {
        StartCoroutine(GetExhibition());
        StartCoroutine(GetImage());
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _image = GetComponent<Image>();
        _tradeGroup = GameObject.Find("Trade Group");
    }
    
    private void OnMouseDown()
    {
        if (!AtomManager.IsPanelActive)
        {
            ShowTradeGroup();
            if (_exhibition == null)
            { 
                GameObject.Find("Trade Group").transform.Find("Trade").gameObject.SetActive(false);
                GameObject.Find("Trade Group").transform.Find("Exhibition").gameObject.SetActive(true);
            }
            else
            {
                GameObject.Find("Trade Group").transform.Find("Exhibition").gameObject.SetActive(false);
                GameObject.Find("Trade Group").transform.Find("Trade").gameObject.SetActive(true);
                
                // TODO: Painting Image -> Trade Panting Image: 이름 변경
                GameObject.Find("Trade Painting Image").GetComponent<SpriteRenderer>().sprite = _sprite;
                GameObject.Find("Trade Painting Image").GetComponent<Image>().sprite = _sprite;

                AtomManager.ExhibitionItem = _exhibition.item.id;
                AtomManager.ExhibitionTrade = _exhibition.trade.id;
            
                GameObject.Find("Trade Group").transform.Find("Trade").Find("Painting Description").GetChild(0).GetChild(1).GetComponent<Text>().text = _exhibition.item.name;
                GameObject.Find("Trade Group").transform.Find("Trade").Find("Painting Description").GetChild(1).GetChild(1).GetComponent<Text>().text = _exhibition.item.author;
                GameObject.Find("Trade Group").transform.Find("Trade").Find("Painting Description").GetChild(2).GetChild(1).GetComponent<Text>().text = _exhibition.trade.owner;
                GameObject.Find("Trade Group").transform.Find("Trade").Find("Description").GetChild(0).GetChild(1).GetComponent<Text>().text = _exhibition.item.price.ToString("N1");
                GameObject.Find("Trade Group").transform.Find("Trade").Find("Description").GetChild(1).GetChild(1).GetComponent<Text>().text = _exhibition.expire.ToString("yy-MM-dd");
            }   
        }
    }

    private void ShowTradeGroup()
    {
        AtomManager.OpenPanel("Trade Group");
    }
    
    private IEnumerator GetExhibition()
    {
        string path = String.Format("/exhibition/{0}/{1}", hall, num);
        
        UnityWebRequest request = UnityWebRequest.Get(BaseURL + path);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            _exhibition = null;
        }
        else
        {
            _exhibition = JsonUtility.FromJson<ExhibitionSerializer>(request.downloadHandler.text);
        }
        Debug.Log(_exhibition);
    }

    private IEnumerator GetImage()
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
            _sprite = Sprite.Create(texture, new Rect(0, 0, 640, 480), new Vector2(0.5f, 0.5f));
            _spriteRenderer.sprite = _sprite;
            _image.sprite = _sprite;
        }
    }
}