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
    public GameObject nameText;
    public GameObject authorText;
    public GameObject ownerText;
    public GameObject priceText;
    public GameObject expireText;
    public GameObject painting;
    
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
        GetTexts();
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
                painting.GetComponent<SpriteRenderer>().sprite = _sprite;
                painting.GetComponent<Image>().sprite = _sprite;

                AtomManager.ExhibitionItem = _exhibition.item.id;
                AtomManager.ExhibitionTrade = _exhibition.trade.id;
            
                _name.text = _exhibition.item.name;
                _author.text = _exhibition.item.author;
                _owner.text = _exhibition.trade.owner;
                _price.text = _exhibition.item.price.ToString("N1");
                _expire.text = _exhibition.expire.ToString("yy-MM-dd");
            }   
        }
    }

    private void ShowTradeGroup()
    {
        AtomManager.OpenPanel("Trade Group");
    }
    
    private void GetTexts()
    {
        _name = nameText.GetComponent<Text>();
        _author = authorText.GetComponent<Text>();
        _owner = ownerText.GetComponent<Text>();
        _price = priceText.GetComponent<Text>();
        _expire = expireText.GetComponent<Text>();
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
