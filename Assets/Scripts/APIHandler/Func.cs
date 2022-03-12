using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Func : MonoBehaviour
{
    public void Close()
    {
        AtomManager.ClosePanel();
    }

    public void UploadImage()
    {
        AtomManager.ClosePanel();
        AtomManager.OpenPanel("Upload Group");
    }

    public void SetUploadTarget(int target)
    {
        AtomManager.ClosePanel();
        AtomManager.UploadTarget = target;
        AtomManager.ConfirmType = "UPLOAD";
        AtomManager.OpenConfirmPanel(String.Format("정말 {0}을 판매하시겠습니까?", AtomManager.Inventories[target].name));
    }

    public void ConfirmSubmit()
    {
        AtomManager.ClosePanel();
        if (AtomManager.ConfirmType == "UPLOAD")
        {
            AtomManager.InputType = "UPLOAD";
            AtomManager.OpenInputPanel(String.Format("{0}의 가격을 입력하세요.", AtomManager.Inventories[AtomManager.UploadTarget].name));
        } 
    }

    public void InputSubmit()
    {
        AtomManager.ClosePanel();
        if (AtomManager.InputType == "UPLOAD")
        {
            string money = GameObject.Find("Input Group").transform.GetChild(1).GetChild(2).GetChild(0).GetChild(1)
                .GetComponent<TextMeshProUGUI>().text;
            money = money.Replace("\u200b", "");
            AtomManager.StartPostExhibition(AtomManager.Inventories[AtomManager.UploadTarget].id, float.Parse(money), AtomManager.clickedHall, AtomManager.clickedNum);
        }
    }

    public static void RefreshExhibition()
    {
        GameObject gameObject =
            GameObject.Find(String.Format("Exhibit {0}-{1}", AtomManager.clickedHall, AtomManager.clickedNum));
        gameObject.transform.GetChild(0).GetComponent<ExhibitionItem>().SetImage();
    }
}
