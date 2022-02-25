using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using API.Models;

public class Order : MonoBehaviour
{
    private const string BaseURL = "http://0.0.0.0:8080/api";
    private static string _token;

    public static IEnumerator CreateOrder(int item, float price, int trade, string status = "order")
    {
        const string path = "/order";

        OrderCreateSerializer serializer = new OrderCreateSerializer(item, price, trade, status);
        string json = JsonUtility.ToJson(serializer);
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        
        UnityWebRequest request = UnityWebRequest.Post(BaseURL + path, json);
        request.uploadHandler = new UploadHandlerRaw(bytes);
        request.SetRequestHeader("Authorization", _token);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            
            // 본인이 아이템의 주인이 아니라면 ->
            
            // 가격이 0원 이하라면 
        }
        else
        {
            OrderSerializer order = JsonUtility.FromJson<OrderSerializer>(request.downloadHandler.text);
        }
    }

    public static IEnumerator DeleteOrder(int orderId)
    {
        string path = String.Format("/order/{0}", orderId);
        
        UnityWebRequest request = UnityWebRequest.Delete(BaseURL + path);
        request.SetRequestHeader("Authorization", _token);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            
            // 아이템 주인은 주문 금지 ->
            
            // 존재하지 않은 주문 -> 
        }
        else
        {
            // 삭제 처리
        }
    }

    public static IEnumerator GetOrders()
    {
        const string path = "/orders";
        
        UnityWebRequest request = UnityWebRequest.Get(BaseURL + path);
        request.SetRequestHeader("Authorization", _token);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            // TODO: List<OrderSerializer> 생성
        }
    }

    public static IEnumerator GetOrderById(int tradeId)
    {
        string path = string.Format("/orders/{0}", tradeId);
        
        UnityWebRequest request = UnityWebRequest.Get(BaseURL + path);
        request.SetRequestHeader("Authorization", _token);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            
            // Trade가 존재하지 않음 ->
        }
        else
        {
            // TODO: List<OrderSerializer> 생성
        }
    }
}
