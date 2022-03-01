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
                "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpZCI6OCwidXNlcm5hbWUiOiJKdW5zdSIsImVtYWlsIjpudWxsLCJleHAiOjE2NDYwMzk0MTB9.LOEknC01Xs_yEh2nXzrObWin8yyVov2dgT6tEyYk9ig";
        }
        set
        {
            
        }
    }
    public static Texture[] Images { get; set; }
    
    public static int ExhibitionItem { get; set; }
    
    public static int ExhibitionTrade { get; set; }
    public static InventorySerializer[] Inventories { get; set; }
    public static ExhibitionInventorySerializer[] ExhibitionInventories { get; set; }
    public static bool IsPanelActive { get; set; }
    public static string LastPanel { get; set; }
    public static ProfileSerializer Profile { get; set; }
    public static SimpleItemSerializer[] BuyHistories { get; set; }
    public static SimpleItemSerializer[] SellHistories { get; set; }
    
    
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
    
    public static void StartUserLogin(string username, string password)
    {
        username = username.Replace("\u200b", "");
        password = password.Replace("\u200b", "");
        Instance.StartCoroutine(Auth.UserLogin(username, password));
    }

    public static void StartUserRegister(string email, string username, string password, string nickname, bool gender)
    {
        email = email.Replace("\u200b", "");
        username = username.Replace("\u200b", "");
        password = password.Replace("\u200b", "");
        nickname = nickname.Replace("\u200b", "");
        Instance.StartCoroutine(Auth.UserRegister(username, password, nickname, gender, email));
    }

    public static void StartCreateOrder(int item, int trade)
    {
        Instance.StartCoroutine(Order.CreateOrder(item, trade));
    }

    public static void StartGetInventories()
    {
        Instance.StartCoroutine(Account.GetInventories());
    }

    public static void StartGetExhibitionInventories()
    {
        Instance.StartCoroutine(Account.GetExhibitionInventories());
    }

    public static void StartGetImageByItem(int id, SpriteRenderer renderer, Image image)
    {
        Instance.StartCoroutine(Item.GetImageByItem(id, renderer, image));
    }

    public static void StartGetItemById(int id)
    {
        Instance.StartCoroutine(Item.GetItemById(id));
    }

    public static void StartExtendExhibition(int item)
    {
        Instance.StartCoroutine(Trade.ExtendTrade(item, 14));
    }

    public static void StartGetMyInfoProfile()
    {
        Instance.StartCoroutine(Account.GetMyInfoProfile());
    }

    public static void StartGetMyInfoBuyHistory()
    {
        Instance.StartCoroutine(Account.GetMyInfoBuyHistory());
    }

    public static void StartGetMyInfoSellHistory()
    {
        Instance.StartCoroutine(Account.GetMyInfoSellHistory());
    }
}
