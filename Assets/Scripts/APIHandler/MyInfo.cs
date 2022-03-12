using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyInfo : MonoBehaviour
{
    public static void OpenMyInfo()
    {
        AtomManager.ClosePanel();
        AtomManager.OpenPanel("My Info Group");
    }

    public static void CloseMyInfo()
    {
        AtomManager.ClosePanel();
    }

    public static void MyInfoButton(int number)
    {
        GameObject gameObject = GameObject.Find("My Info Group");
        for (int i = 0; i < 3; i++)
        {
            if (i == number)
            {
                gameObject.transform.GetChild(i + 1).gameObject.SetActive(true);
            }
            else
            {
                gameObject.transform.GetChild(i + 1).gameObject.SetActive(false);
            }
        }
        GetMyInfoData(number);
    }

    public static void SetProfile()
    {
        GameObject gameObject = GameObject.Find("My Info Group");
        gameObject.transform.GetChild(1).GetChild(2).GetChild(1).GetComponent<Text>().text = 
            AtomManager.Profile.profile.nickname;
            gameObject.transform.GetChild(1).GetChild(2).GetChild(3).GetComponent<Text>().text =
            AtomManager.Profile.profile.money.ToString("N1");
    }

    public static void GetMyInfoData(int number)
    {
        GameObject gameObject;
        switch (number)
        {   
            case 0:
                SetProfile();
                break;
            case 1:
                gameObject = GameObject.Find("My Info Sell History Content");
                for (int i = 0; i < AtomManager.SellHistories.Length; i++)
                {
                    Transform transform = gameObject.transform.GetChild(i);
                    SpriteRenderer renderer = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
                    Image image = transform.GetChild(0).GetChild(0).GetComponent<Image>();
                    AtomManager.StartGetItemImage(AtomManager.SellHistories[i].item.id, renderer, image);
                    
                    transform.GetChild(1).GetChild(3).GetComponent<Text>().text =
                        AtomManager.SellHistories[i].item.name;
                    transform.GetChild(1).GetChild(4).GetComponent<Text>().text =
                        AtomManager.SellHistories[i].price.ToString("N1");
                    transform.GetChild(1).GetChild(5).GetComponent<Text>().text =
                        AtomManager.SellHistories[i].item.created_at.ToString("MM/dd/yyyy");
                }

                for (int i = 0; i < 15; i++)
                {
                    if (i >= AtomManager.SellHistories.Length)
                    {
                        gameObject.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                break;
                
            case 2:
                gameObject = GameObject.Find("My Info Buy History Content");
                for (int i = 0; i < AtomManager.BuyHistories.Length; i++)
                {
                    Transform transform = gameObject.transform.GetChild(i);
                    SpriteRenderer renderer = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
                    Image image = transform.GetChild(0).GetChild(0).GetComponent<Image>();
                    AtomManager.StartGetItemImage(AtomManager.BuyHistories[i].item.id, renderer, image);
                    
                    transform.GetChild(1).GetChild(3).GetComponent<Text>().text =
                        AtomManager.BuyHistories[i].item.name;
                    transform.GetChild(1).GetChild(4).GetComponent<Text>().text =
                        AtomManager.BuyHistories[i].price.ToString("N1");
                    transform.GetChild(1).GetChild(5).GetComponent<Text>().text =
                        AtomManager.BuyHistories[i].item.created_at.ToString("MM/dd/yyyy");
                }
                for (int i = 0; i < 15; i++)
                {
                    if (i >= AtomManager.BuyHistories.Length)
                    {
                        gameObject.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                break;
        }
    }
}
