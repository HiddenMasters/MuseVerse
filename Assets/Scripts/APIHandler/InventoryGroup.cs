using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using API.Models;
using UnityEngine.UI;

public class InventoryGroup : MonoBehaviour
{
    private static GameObject _inventoryGroup;
    private static GameObject _itemInfoGroup;
    private static GameObject _extendGroup;

    public static void OpenInventory()
    {
        AtomManager.ClosePanel();
        AtomManager.OpenPanel("Inventory Group");
    }
    
    public static void OpenInventoryGroup()
    {
        AtomManager.ClosePanel();
        LoadInventories();
        AtomManager.OpenPanel("Inventory Group");
    }

    public static void CloseInventory()
    {
        AtomManager.ClosePanel();
    }

    public static void LoadInventories()
    {
        AtomManager.StartGetInventories();
        AtomManager.StartGetExhibitionInventories();
    }

    public static void SetInventoriesImage()
    {
        InventorySerializer[] inventories = AtomManager.Inventories;
        GameObject gameObject = GameObject.Find("InventoriesContent");
        for (int i = 0; i < inventories.Length; i++)
        {
            int childNum = i / 2;
            int position = i % 2;
            Transform line = gameObject.transform.GetChild(childNum);
            Transform button = line.GetChild(position);
            SpriteRenderer renderer = button.GetChild(0).GetComponent<SpriteRenderer>();
            Image image = button.GetChild(0).GetComponent<Image>();
            AtomManager.StartGetImageByItem(inventories[i].id, renderer, image);
        }
    }

    public static void SetExhibitionInventories()
    {
        ExhibitionInventorySerializer[] inventories = AtomManager.ExhibitionInventories;
        GameObject gameObject = GameObject.Find("ExhibitionInventoriesContent");
        for (int i = 0; i < inventories.Length; i++)
        {
            Transform line = gameObject.transform.GetChild(i);
            SpriteRenderer renderer = line.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
            Image image = line.GetChild(0).GetChild(0).GetComponent<Image>();
            Debug.Log(inventories[i]);
            AtomManager.StartGetImageByItem(inventories[i].item.id, renderer, image);
            Transform description = line.GetChild(1);
            description.GetChild(3).GetComponent<Text>().text = inventories[i].item.name;
            description.GetChild(4).GetComponent<Text>().text = inventories[i].item.price.ToString("N1");
            description.GetChild(5).GetComponent<Text>().text = inventories[i].expire.ToString("MM/dd/yyyy");
        }
    }

    public static void ShowInventoryDetail(int number)
    {
        if (AtomManager.Inventories.Length < number)
        {
            return;
        }
        
        AtomManager.ClosePanel();
        DetailSetting(AtomManager.Inventories[number].id);
        AtomManager.OpenPanel("Item Info Group");
    }

    private static void DetailSetting(int number)
    {
        Transform transform = _itemInfoGroup.transform.GetChild(0).GetChild(0);
        SpriteRenderer renderer = transform.GetComponent<SpriteRenderer>();
        Image image = transform.GetComponent<Image>();
        AtomManager.StartGetImageByItem(number, renderer, image);
        AtomManager.StartGetItemById(number);
    }

    public static void ShowExhibitionDetail(int number)
    {
        if (AtomManager.ExhibitionInventories.Length < number)
        {
            return;
        }

        AtomManager.ExhibitionInventoryNumber = number;
        
        AtomManager.ClosePanel();
        _extendGroup.transform.GetChild(0).GetChild(3).GetComponent<Text>().text =
            AtomManager.ExhibitionInventories[number].item.name;
        _extendGroup.transform.GetChild(0).GetChild(4).GetComponent<Text>().text =
            AtomManager.ExhibitionInventories[number].item.price.ToString("N1");
        _extendGroup.transform.GetChild(0).GetChild(5).GetComponent<Text>().text =
            AtomManager.ExhibitionInventories[number].expire.ToString("MM/dd/yyyy");
        AtomManager.OpenPanel("Extend Group");
    }

    public static void ExtendExpire()
    {
        int item = AtomManager.ExhibitionInventories[AtomManager.ExhibitionInventoryNumber].item.id;
        AtomManager.StartExtendExhibition(item);
    }
}
