using System.Collections;
using System.Collections.Generic;
using API.Models;
using UnityEngine;
using UnityEngine.UI;

public class TradeGroup : MonoBehaviour
{
    public static void SetImage(Sprite sprite)
    {
        GameObject gameObject = GameObject.Find("Trade Painting Image");
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        gameObject.GetComponent<Image>().sprite = sprite;
    }

    public static void SetDetail(ExhibitionDetailSerializer exhibition)
    {
        GameObject BaseObject = GameObject.Find("Trade Group");
        BaseObject.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = exhibition.owner;
        BaseObject.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = exhibition.expire.ToString("MM/dd/yyyy");

        BaseObject.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = exhibition.item.name;
        BaseObject.transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<Text>().text = exhibition.item.author;
        BaseObject.transform.GetChild(1).GetChild(0).GetChild(2).GetComponent<Text>().text =
            exhibition.trade.price.ToString("N1");
    }

    public static void SetUploadInventories()
    {
        ItemSerializer[] inventories = AtomManager.Inventories;
        GameObject gameObject = GameObject.Find("UploadInventoriesContent");
        for (int i = 0; i < inventories.Length; i++)
        {
            int childNum = i / 2;
            int position = i % 2;
            Transform line = gameObject.transform.GetChild(childNum);
            Transform button = line.GetChild(position);
            SpriteRenderer renderer = button.GetChild(0).GetComponent<SpriteRenderer>();
            Image image = button.GetChild(0).GetComponent<Image>();
            button.GetChild(1).GetComponent<Text>().text = inventories[i].name;
            AtomManager.StartGetItemImage(inventories[i].id, renderer, image);
        }

        for (int i = 0; i < 40; i++)
        {
            int childNum = i / 2;
            int position = i % 2;
            if (i >= inventories.Length)
            {
                gameObject.transform.GetChild(childNum).GetChild(position).gameObject.SetActive(false);
            }
        }
    }

    public void BuyExhibition()
    {
        AtomManager.ClosePanel();
        AtomManager.ConfirmType = "BUY";
        AtomManager.OpenConfirmPanel("정말 구매 하시겠습니까?");
    }
}