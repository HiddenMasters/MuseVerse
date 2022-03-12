using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using API.Models;
using UnityEngine.UI;

public class InventoryGroup : MonoBehaviour
{

    public static void OpenInventory()
    {
        AtomManager.ClosePanel();
        AtomManager.OpenPanel("Inventory Group");
    }

    public static void CloseInventory()
    {
        AtomManager.ClosePanel();
    }

    public static void LoadInventories()
    {
        AtomManager.StartGetInventories();
        AtomManager.StartGetExhibitions();
    }

    public static void SetInventoriesImage()
    {
        ItemSerializer[] inventories = AtomManager.Inventories;
        GameObject gameObject = GameObject.Find("InventoriesContent");
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

        for (int i = 0; i < 20; i++)
        {
            int childNum = i / 2;
            int position = i % 2;
            if (i >= inventories.Length)
            {
                gameObject.transform.GetChild(childNum).GetChild(position).gameObject.SetActive(false);
            }
        }
    }

    public static void SetExhibitionInventories()
    {
        ExhibitionSerializer[] inventories = AtomManager.ExhibitionInventories;
        GameObject gameObject = GameObject.Find("ExhibitionInventoriesContent");
        for (int i = 0; i < inventories.Length; i++)
        {
            Transform line = gameObject.transform.GetChild(i);
            SpriteRenderer renderer = line.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
            Image image = line.GetChild(0).GetChild(0).GetComponent<Image>();
            AtomManager.StartGetItemImage(inventories[i].item.id, renderer, image);
            Transform description = line.GetChild(0).GetChild(1);
            description.GetChild(0).GetComponent<Text>().text = inventories[i].item.name;
            description.GetChild(1).GetComponent<Text>().text = inventories[i].trade.price.ToString("N1");
            description.GetChild(2).GetComponent<Text>().text = inventories[i].expire.ToString("MM/dd/yyyy");
        }
        
        for (int i = 0; i < 10; i++)
        {
            if (i >= inventories.Length)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public static void ShowInventoryDetail(int number)
    {
        if (AtomManager.Inventories.Length < number)
        {
            return;
        }
        AtomManager.ClosePanel();
        DetailSetting(AtomManager.Inventories[number].id, AtomManager.Inventories[number]);
        AtomManager.OpenPanel("Item Info Group");
    }

    private static void DetailSetting(int number, ItemSerializer item)
    {
        Transform temp = GameObject.Find("Item Info Group").transform.GetChild(0);
        SpriteRenderer renderer = temp.GetComponent<SpriteRenderer>();
        Image image = temp.GetComponent<Image>();
        AtomManager.StartGetItemImage(number, renderer, image);
        AtomManager.StartGetItem(number);
    }

    public static void ShowExhibitionDetail(int number)
    {
        if (AtomManager.ExhibitionInventories.Length < number)
        {
            return;
        }

        AtomManager.ExhibitionInventoryNumber = number;
        
        GameObject extendGroup = GameObject.Find("Extend Group");
        AtomManager.ClosePanel();
        extendGroup.transform.GetChild(0).GetChild(0).GetComponent<Text>().text =
            AtomManager.ExhibitionInventories[number].item.name;
        extendGroup.transform.GetChild(0).GetChild(1).GetComponent<Text>().text =
            AtomManager.ExhibitionInventories[number].trade.price.ToString("N1");
        extendGroup.transform.GetChild(0).GetChild(2).GetComponent<Text>().text =
            AtomManager.ExhibitionInventories[number].expire.ToString("MM/dd/yyyy");
        AtomManager.OpenPanel("Extend Group");
    }

    public static void ExtendExpire()
    {
        int item = AtomManager.ExhibitionInventories[AtomManager.ExhibitionInventoryNumber].item.id;
        AtomManager.StartPutTradeExtend(item);
    }
}
