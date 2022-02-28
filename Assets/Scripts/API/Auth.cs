using System;
using System.Collections;
using System.Text;
using API.Models;
using UnityEngine;
using UnityEngine.Networking;

public class Auth : MonoBehaviour
{
    private const string BaseURL = "http://museverse.kro.kr/api";
    // private const string BaseURL = "http://0.0.0.0:8080/api";
    private static string _token;

    public static IEnumerator UserRegister(string username, string password, string nickname, bool gender,
        string email = null)
    {
        const string path = "/auth/register";

        RegisterSerializer register = new RegisterSerializer(username, password, nickname, gender, email);
        string json = JsonUtility.ToJson(register);
        byte[] bytes = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = UnityWebRequest.Post(BaseURL + path, json);
        request.uploadHandler = new UploadHandlerRaw(bytes);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            // 축하 메시지 혹은 정상 회원 가입 리턴
            LoginPanel.OpenLoginGroup();
        }
    }
    
    public static IEnumerator UserLogin(string username, string password)
    {
        const string path = "/auth/login";

        LoginSerializer login = new LoginSerializer(username, password);

        string json = JsonUtility.ToJson(login);
        byte[] bytes = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = UnityWebRequest.Post(BaseURL + path, json);
        request.uploadHandler = new UploadHandlerRaw(bytes);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            // 유저에게 JWT Token 저장하기
            AuthenticationSerializer
                auth = JsonUtility.FromJson<AuthenticationSerializer>(request.downloadHandler.text);
            _token = auth.authorization;
            
            // Atom 객체에 Token 저장
            AtomManager.Token = _token;
            
            // LoginPanel 해제
            AtomManager.ClosePanel();
        }
    }
}

