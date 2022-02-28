using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using API.Models;
using UnityEngine;
using UnityEngine.Networking;

public class Trade : MonoBehaviour
{
    private const string BaseURL = "http://museverse.kro.kr/api";
    // private const string BaseURL = "http://0.0.0.0:8080/api";
    // private static string _token;
    // private static string _token = "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpZCI6OCwidXNlcm5hbWUiOiJKdW5zdSIsImVtYWlsIjpudWxsLCJleHAiOjE2NDU3NTYzMDF9.-Or121v4BTBbgd_F0dLd-XwWeLwuwxe1x3bdX70GDt0";

    private void Start()
    {
        StartCoroutine(GetTrades());
    }

    public static IEnumerator CreateTrade(int item, float orderPrice, float immediatePrice)
    {
        const string path = "/trade";
        TradeSerializer serializer = new TradeSerializer(item, orderPrice, immediatePrice);
        string json = JsonUtility.ToJson(serializer);
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        
        UnityWebRequest request = UnityWebRequest.Post(BaseURL + path, json);
        request.uploadHandler = new UploadHandlerRaw(bytes);
        request.SetRequestHeader("Authorization", AtomManager.Token);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            
            // 이미 올려져 있는 아이템인 경우 -> 
            
            // 본인의 아이템이 아닌 경우 ->
        }
        else
        {
            
        }
    }

    public static IEnumerator GetTradeById(int itemId)
    {
        string path = string.Format("/trade/{0}", itemId);
        UnityWebRequest request = UnityWebRequest.Get(BaseURL + path);
        request.SetRequestHeader("Authorization", AtomManager.Token);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            
            // 아이템이 존재하지 않을 경우 ->
        }
        else
        {
            TradeItemSerializer tradeItem = JsonUtility.FromJson<TradeItemSerializer>(request.downloadHandler.text);
        }
    }

    public static IEnumerator DeleteTrade(int itemId)
    {
        string path = string.Format("/trade/{0}", itemId);
        UnityWebRequest request = UnityWebRequest.Delete(BaseURL + path);
        request.SetRequestHeader("Authorization", AtomManager.Token);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            
            // 본인의 아이템이 아닌 경우
        }
        else
        {
            // Trade가 정상적으로 삭제됨.
        }
    }

    public static IEnumerator ExtendTrade(int itemId, int extendDays = 14)
    {
        string path = string.Format("/trade/{0}/extend", itemId);
        
        TradeExtendSerializer serializer = new TradeExtendSerializer(extendDays);
        string json = JsonUtility.ToJson(serializer);
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        
        UnityWebRequest request = UnityWebRequest.Put(BaseURL + path, json);
        request.uploadHandler = new UploadHandlerRaw(bytes); 
        request.SetRequestHeader("Authorization", AtomManager.Token);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            
            // 본인의 아이템이 아닌 경우
        }
        else
        {
            // 정상적으로 Trade가 연장됨
        }
    }

    public static IEnumerator GetTrades()
    {
        const string path = "/trades";
        
        UnityWebRequest request = UnityWebRequest.Get(BaseURL + path);
        request.SetRequestHeader("Authorization", AtomManager.Token);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {   
            Debug.Log(request.error);
        }
        else
        {
            // TODO: Fix Trade Serializer
            TradeListSerializer tradeList = JsonUtility.FromJson<TradeListSerializer>(request.downloadHandler.text);
            Debug.Log(tradeList);
        }
    }

    public static IEnumerator GetMyTrades()
    {
        const string path = "/trades/me";
        
        UnityWebRequest request = UnityWebRequest.Get(BaseURL + path);
        request.SetRequestHeader("Authorization", AtomManager.Token);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        
        if (request.result != UnityWebRequest.Result.Success)
        {   
            Debug.Log(request.error);
        }
        else
        {
            // TODO: Fix Trade Serializer
            TradeListSerializer tradeList = JsonUtility.FromJson<TradeListSerializer>(request.downloadHandler.text);
            Debug.Log(tradeList);
        }
    }
}