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
        GameObject gameObject = GameObject.Find("My Info Contents Group");
        for (int i = 0; i < 3; i++)
        {
            if (i == number)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        GetMyInfoData(number);
    }

    public static void GetMyInfoData(int number)
    {
        GameObject gameObject;
        switch (number)
        {   
            case 0:
                gameObject = GameObject.Find("My Info Contents Group");
                gameObject.transform.GetChild(number).GetChild(2).GetChild(1).GetComponent<Text>().text =
                    AtomManager.Profile.nickname;
                gameObject.transform.GetChild(number).GetChild(3).GetChild(1).GetComponent<Text>().text =
                    AtomManager.Profile.money.ToString("N1");
                break;
            case 1:
                gameObject = GameObject.Find("My Info Buy History Content");
                for (int i = 0; i < AtomManager.BuyHistories.Length; i++)
                {
                    Transform transform = gameObject.transform.GetChild(i);
                    SpriteRenderer renderer = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
                    Image image = transform.GetChild(0).GetChild(0).GetComponent<Image>();
                    AtomManager.StartGetImageByItem(AtomManager.BuyHistories[i].id, renderer, image);
                    
                    transform.GetChild(1).GetChild(3).GetComponent<Text>().text =
                        AtomManager.BuyHistories[i].name;
                    transform.GetChild(1).GetChild(4).GetComponent<Text>().text =
                        AtomManager.BuyHistories[i].price.ToString("N1");
                    transform.GetChild(1).GetChild(5).GetComponent<Text>().text =
                        AtomManager.BuyHistories[i].created_at.ToString("MM/dd/yyyy");
                }

                break;
            case 2:
                gameObject = GameObject.Find("My Info Sell History Content");
                for (int i = 0; i < AtomManager.SellHistories.Length; i++)
                {
                    Transform transform = gameObject.transform.GetChild(i);
                    SpriteRenderer renderer = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
                    Image image = transform.GetChild(0).GetChild(0).GetComponent<Image>();
                    AtomManager.StartGetImageByItem(AtomManager.SellHistories[i].id, renderer, image);
                    
                    transform.GetChild(1).GetChild(3).GetComponent<Text>().text =
                        AtomManager.SellHistories[i].name;
                    transform.GetChild(1).GetChild(4).GetComponent<Text>().text =
                        AtomManager.SellHistories[i].price.ToString("N1");
                    transform.GetChild(1).GetChild(5).GetComponent<Text>().text =
                        AtomManager.SellHistories[i].created_at.ToString("MM/dd/yyyy");
                }
                break;
        }
    }
}
