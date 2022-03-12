using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{ 
    public void LoginButtonClick()
    {
        AtomManager.OpenPanel("Login Group");
        string username = GameObject.Find("Login Group").transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text;
        string password = GameObject.Find("Login Group").transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text;
        AtomManager.StartUserLogin(username, password);
    }

    public void RegisterButtonClick()
    {
        AtomManager.OpenPanel("Register Group");
        string email = GameObject.Find("Register Form").transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text;
        string username = GameObject.Find("Register Form").transform.GetChild(1).GetChild(1).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text;
        string password = GameObject.Find("Register Form").transform.GetChild(2).GetChild(1).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text;
        string nickname = GameObject.Find("Register Form").transform.GetChild(3).GetChild(1).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text;
        AtomManager.StartUserRegister(email, username, password, nickname);
    }

    public static void OpenLoginGroup()
    {
        AtomManager.ClosePanel();
        AtomManager.OpenPanel("Login Group");
    }
    
    public static void OpenRegisterGroup()
    {
        AtomManager.ClosePanel();
        AtomManager.OpenPanel("Register Group");
    }
}
