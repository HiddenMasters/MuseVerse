using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using API.Models;
using UnityEngine.UI;

public sealed class AtomManager : MonoBehaviour
{
    public static AtomManager Instance = null;
    private static GameObject _gameObject;

    void Awake()
    {
        if (Instance == null)
        {        
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);    
        }
        
        DefaultSetting();
        DontDestroyOnLoad(gameObject);
    }

    private void DefaultSetting()
    {
        InventoryGroup.LoadInventories();
        StartGetMyInfoProfile();
        StartGetMyInfoBuyHistory();
        StartGetMyInfoSellHistory();
    }
    

    // public static string Token { get; set; }
    public static string Token
    {
        get
        {
            return
                "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpZCI6OCwidXNlcm5hbWUiOiJKdW5zdSIsImVtYWlsIjpudWxsLCJleHAiOjE2NDYyNjg5MDN9.c9uF3kJRJHChUSvGWXvLPvz9HMJmI20Y6HrgpYOtauI";
        }
        set
        {
            
        }
    }
    public static Texture[] Images { get; set; }
    
    public static int ExhibitionItem { get; set; }
    
    public static int ExhibitionTrade { get; set; }
    public static ItemsSerializer[] Inventories { get; set; }
    public static ItemsSerializer[] ExhibitionInventories { get; set; }
    public static bool IsPanelActive { get; set; }
    public static string LastPanel { get; set; }
    public static AuthUserProfileSerializer Profile { get; set; }
    public static TradesSerializer[] BuyHistories { get; set; }
    public static TradesSerializer[] SellHistories { get; set; }
    public static int clickedHall { get; set; }
    public static int clickedNum { get; set; }
    
    
    public static int ExhibitionInventoryNumber { get; set; }
    
    // public static ExhibitionSerializer Exhibition { get; set; }
    public static void ClosePanel()
    {
        if (LastPanel != null)
        {
            _gameObject = GameObject.Find(LastPanel);
            RectTransform transform = _gameObject.GetComponent<RectTransform>();
            transform.anchoredPosition = Vector2.down * 1200;
            IsPanelActive = false;    
        }
    }

    public static void OpenPanel(string panel)
    {
        IsPanelActive = true;
        GameObject gameObject = GameObject.Find(panel);
        RectTransform transform = gameObject.GetComponent<RectTransform>();
        transform.anchoredPosition = Vector3.zero;
        LastPanel = panel;
    }
    
    // Auth
    public static void StartUserLogin(string username, string password)
    {
        username = username.Replace("\u200b", "");
        password = password.Replace("\u200b", "");
        Instance.StartCoroutine(Auth.PostLogin(username, password));
    }

    public static void StartUserRegister(string email, string username, string password, string nickname, bool gender)
    {
        email = email.Replace("\u200b", "");
        username = username.Replace("\u200b", "");
        password = password.Replace("\u200b", "");
        nickname = nickname.Replace("\u200b", "");
        Instance.StartCoroutine(Auth.PostRegister(username, password, nickname, gender, email));
    }
    
    // Accounts
    public static void StartGetUserProfile()
    {
        Instance.StartCoroutine(Account.GetUserProfile());
    }

    public static void StartPutProfileNickname(string nickname)
    {
        Instance.StartCoroutine(Account.PutProfileNickname(nickname));
    }

    public static void StartGetInventories()
    {
        Instance.StartCoroutine(Account.GetInventories());
    }

    public static void StartGetExhibitions()
    {
        Instance.StartCoroutine(Account.GetExhibitions());
    }

    public static void StartGetBuyHistories()
    {
        Instance.StartCoroutine(Account.GetBuyHistories());
    }

    public static void StartGetSellHistories()
    {
        Instance.StartCoroutine(Account.GetSellHistories());
    }

    public static void StartPostAttendances()
    {
        Instance.StartCoroutine(Account.PostAttendances());
    }

    public static void StartGetAttendances()
    {
        Instance.StartCoroutine(Account.GetAttendances());
    }
    
    // Exhibitions
    public static void StartPostExhibition(int item, int hall, int num)
    {
        Instance.StartCoroutine(Exhibition.PostExhibition(item, hall, num));
    }

    public static void StartPutExhibition(int exhibitionId)
    {
        Instance.StartCoroutine(Exhibition.PutExhibition(exhibitionId));
    }

    public static void StartDeleteExhibition(int exhibitionId)
    {
        Instance.StartCoroutine(Exhibition.DeleteExhibition(exhibitionId));
    }
    
    // Items
    // TODO
    public static void StartPostItem(string name, string format) { }

    public static void StartGetItem(int itemId)
    {
        Instance.StartCoroutine(Item.GetItem(itemId));
    }

    public static void StartDeleteItem(int itemId)
    {
        Instance.StartCoroutine(Item.DeleteItem(itemId));
    }

    public static void StartGetItemImage(int itemId, SpriteRenderer renderer, Image image)
    {
        Instance.StartCoroutine(Item.GetItemImage(itemId, renderer, image));
    }
    
    // Private
    public static void StartPostPrivate(int item, int num)
    {
        Instance.StartCoroutine(Private.PostPrivate(item, num));
    }

    public static void StartGetPrivate(int privateId)
    {
        Instance.StartCoroutine(Private.GetPrivate(privateId));
    }

    public static void StartDeletePrivate(int privateId)
    {
        Instance.StartCoroutine(Private.DeletePrivate(privateId));
    }

    public static void StartPostTrade(int item, float price)
    {
        Instance.StartCoroutine(Trade.PostTrade(item, price));
    }

    public static void StartGetTrade(int tradeId)
    {
        Instance.StartCoroutine(Trade.GetTrade(tradeId));
    }

    public static void StartDeleteTrade(int tradeId)
    {
        Instance.StartCoroutine(Trade.DeleteTrade(tradeId));
    }

    public static void StartPutTradeBuy(int tradeId)
    {
        Instance.StartCoroutine(Trade.PutTradeBuy(tradeId));
    }

    public static void StartPutTradeExtend(int tradeId)
    {
        Instance.StartCoroutine(Trade.PutTradeExtend(tradeId));
    }

    public static void StartGetTrades()
    {
        Instance.StartCoroutine(Trade.GetTrades());
    }
}
