using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginPanel : MonoBehaviour
{
    public static GameObject loginPanel;
    public GameObject usernameInput;
    public GameObject passwordInput;

    public void Login()
    {
        string username = usernameInput.GetComponent<TextMeshProUGUI>().text;
        string password = passwordInput.GetComponent<TextMeshProUGUI>().text;
        AtomManager.StartUserLogin(username, password);
    }

    public static void ClosePanel()
    {
        loginPanel = GameObject.Find("MenuPanel");
        loginPanel.SetActive(false);
    }
}
