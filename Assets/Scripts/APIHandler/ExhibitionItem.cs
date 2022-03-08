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
    private GameObject _tradeGroup;
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
        _tradeGroup = GameObject.Find("Trade Group");
        StartCoroutine(GetExhibition());
        StartCoroutine(GetExhibitionImage());
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
                
                GameObject.Find("Trade Painting Image").GetComponent<SpriteRenderer>().sprite = null;
                GameObject.Find("Trade Painting Image").GetComponent<Image>().sprite = null;
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
                GameObject.Find("Trade Group").transform.Find("Trade").Find("Painting Description").GetChild(2).GetChild(1).GetComponent<Text>().text = _exhibition.trade.seller;
                GameObject.Find("Trade Group").transform.Find("Trade").Find("Description").GetChild(0).GetChild(1).GetComponent<Text>().text = _exhibition.trade.price.ToString("N1");
                GameObject.Find("Trade Group").transform.Find("Trade").Find("Description").GetChild(1).GetChild(1).GetComponent<Text>().text = _exhibition.expire.ToString("yy-MM-dd");
            }
            AtomManager.clickedHall = hall;
            AtomManager.clickedNum = num;
            Debug.Log("Hall: " + hall + ", Num: " + num);
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
        request.timeout = 1;

        yield return request.SendWebRequest();
        
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            ExhibitionDetailSerializer exhibition =
                JsonUtility.FromJson<ExhibitionDetailSerializer>(request.downloadHandler.text);
            // Exhibition 데이터 채우기
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