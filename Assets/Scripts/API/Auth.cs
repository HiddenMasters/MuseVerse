using System;
using System.Collections;
using System.Text;
using API.Models;
using UnityEngine;
using UnityEngine.Networking;

public class Auth : MonoBehaviour
{
    // private const string BaseURL = "http://museverse.kro.kr/api";
    private const string BaseURL = "http://0.0.0.0:8080/api";
    private static string _token;

    public static IEnumerator PostRegister(string username, string password, string nickname, bool gender,
        string email = null)
    {
        const string path = "/auth/register";

        AuthUserCreateSerializer register = new AuthUserCreateSerializer(username, password, email, nickname);
        string json = JsonUtility.ToJson(register);
        byte[] bytes = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = UnityWebRequest.Post(BaseURL + path, json);
        request.uploadHandler = new UploadHandlerRaw(bytes);
        request.SetRequestHeader("Content-Type", "application/json");
        request.timeout = 1;

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
    
    public static IEnumerator PostLogin(string username, string password)
    {
        const string path = "/auth/login";

        AuthLoginSerializer login = new AuthLoginSerializer(username, password);

        string json = JsonUtility.ToJson(login);
        byte[] bytes = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = UnityWebRequest.Post(BaseURL + path, json);
        request.uploadHandler = new UploadHandlerRaw(bytes);
        request.SetRequestHeader("Content-Type", "application/json");
        request.timeout = 1;

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            // 유저에게 JWT Token 저장하기
            TokenSerializer
                auth = JsonUtility.FromJson<TokenSerializer>(request.downloadHandler.text);
            _token = auth.authorization;
            
            // Atom 객체에 Token 저장
            AtomManager.Token = _token;
            
            // LoginPanel 해제
            AtomManager.LastPanel = "Login Group";
            AtomManager.ClosePanel();
        }
    }
}

