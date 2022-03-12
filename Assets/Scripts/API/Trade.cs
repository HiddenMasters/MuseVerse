using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using API.Models;
using UnityEngine;
using UnityEngine.Networking;

public class Trade : MonoBehaviour
{
    // private const string BaseURL = "http://museverse.kro.kr/api";
    private const string BaseURL = "http://0.0.0.0:8080/api";

    private void Start()
    {
        StartCoroutine(GetTrades());
    }

    public static IEnumerator PostTrade(int item, float price)
    {
        const string path = "/trade";

        TradeCreateSerializer serializer = new TradeCreateSerializer(item, price);
        string json = JsonUtility.ToJson(serializer);
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        
        UnityWebRequest request = UnityWebRequest.Post(BaseURL + path, json);
        request.uploadHandler = new UploadHandlerRaw(bytes);
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
            // Trade 정상 등록 Alert
        }
    }

    public static IEnumerator GetTrade(int tradeId)
    {
        string path = string.Format("/trade/{0}", tradeId);
        
        UnityWebRequest request = UnityWebRequest.Get(BaseURL + path);
        request.SetRequestHeader("Authorization", AtomManager.Token);
        request.SetRequestHeader("Content-Type", "application/json");
        request.timeout = 1;

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            
            // 아이템이 존재하지 않을 경우 ->
        }
        else
        {
            TradeDetailSerializer trade = JsonUtility.FromJson<TradeDetailSerializer>(request.downloadHandler.text);
        }
    }

    public static IEnumerator DeleteTrade(int tradeId)
    {
        string path = string.Format("/trade/{0}", tradeId);
        
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
            // Trade가 정상적으로 삭제 Alert
        }
    }

    public static IEnumerator PutTradeBuy(int tradeId)
    {
        string path = string.Format("/trade/{0}/buy", tradeId);
        
        UnityWebRequest request = UnityWebRequest.Put(BaseURL + path, String.Empty);
        request.SetRequestHeader("Authorization", AtomManager.Token);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            AtomManager.Refresh();
            AtomManager.OpenAlertPanel("정상적으로 구매하였습니다!");
        }
    }

    public static IEnumerator PutTradeExtend(int tradeId)
    {
        string path = string.Format("/trade/{0}/extend", tradeId);
        
        UnityWebRequest request = UnityWebRequest.Put(BaseURL + path, String.Empty);
        request.SetRequestHeader("Authorization", AtomManager.Token);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            // 정상적으로 Trade가 연장 Alert
        }
    }
    
    public static IEnumerator PutTradePrice(int tradeId, float price)
    {
        string path = string.Format("/trade/{0}/price", tradeId);

        TradeChangePriceSerializer serializer = new TradeChangePriceSerializer(price);
        string json = JsonUtility.ToJson(serializer);
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        
        UnityWebRequest request = UnityWebRequest.Put(BaseURL + path, json);
        request.uploadHandler = new UploadHandlerRaw(bytes);
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
            AtomManager.Refresh();
            AtomManager.OpenAlertPanel("가격이 정상적으로 수정되었습니다!");
        }
    }

    public static IEnumerator GetTrades()
    {
        const string path = "/trades";
        
        UnityWebRequest request = UnityWebRequest.Get(BaseURL + path);
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
            TradesSerializer trades = JsonUtility.FromJson<TradesSerializer>(request.downloadHandler.text);
        }
    }
}