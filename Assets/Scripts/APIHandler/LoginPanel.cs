using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    public static GameObject LoginGroup;
    public static GameObject RegisterGroup;
    public GameObject usernameLoginInput;
    public GameObject passwordLoginInput;

    public GameObject emailRegisterInput;
    public GameObject usernameRegisterInput;
    public GameObject passwordRegisterInput;
    public GameObject nicknameRegisterInput;
    public GameObject maleRegisterToggle;
    public GameObject femaleRegisterToggle;
    
    public void Login()
    {
        string username = usernameLoginInput.GetComponent<TextMeshProUGUI>().text;
        string password = passwordLoginInput.GetComponent<TextMeshProUGUI>().text;
        AtomManager.StartUserLogin(username, password);
    }

    public void Register()
    {
        string email = emailRegisterInput.GetComponent<TextMeshProUGUI>().text;
        string username = usernameRegisterInput.GetComponent<TextMeshProUGUI>().text;
        string password = passwordRegisterInput.GetComponent<TextMeshProUGUI>().text;
        string nickname = nicknameRegisterInput.GetComponent<TextMeshProUGUI>().text;
        bool flag = maleRegisterToggle.GetComponent<Toggle>().isOn;
        AtomManager.StartUserRegister(email, username, password, nickname, flag);
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

    public void checkMale()
    {
        Toggle male = maleRegisterToggle.GetComponent<Toggle>();
        Toggle female = femaleRegisterToggle.GetComponent<Toggle>();
        male.isOn = true;
        female.isOn = false;
    }

    public void checkFemale()
    {
        Toggle male = maleRegisterToggle.GetComponent<Toggle>();
        Toggle female = femaleRegisterToggle.GetComponent<Toggle>();
        male.isOn = false;
        female.isOn = true;
    }
}
