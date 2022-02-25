using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using API.Models;

public sealed class AtomManager : MonoBehaviour
{
    public static AtomManager Instance = null;

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

        DontDestroyOnLoad(gameObject);
        
    }
    
    

    public static string Token { get; set; }
    
    public static Texture[] Images { get; set; }
    
    // public static ExhibitionSerializer Exhibition { get; set; }
     
    public static void StartUserLogin(string username, string password)
    {
        username = username.Replace("\u200b", "");
        password = password.Replace("\u200b", "");
        Instance.StartCoroutine(Auth.UserLogin(username, password));
    }
}
