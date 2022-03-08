using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeGroup : MonoBehaviour
{
    private static GameObject _tradeGroup;
    public static void OpenTradeGroup()
    {
        AtomManager.ClosePanel();
        AtomManager.OpenPanel("Trade Group");
    }

    public static void CloseTradeGroup()
    {
        AtomManager.ClosePanel();
    }

    public static void BuyItem()
    {
        int item = AtomManager.ExhibitionItem;
        int trade = AtomManager.ExhibitionTrade;
        
        AtomManager.StartCreateOrder(item, trade);
        
    }

    public static void ExhibitItem()
    {
        AtomManager.StartExhibitItem(43, AtomManager.clickedHall, AtomManager.clickedNum, 1000, 2000);
    }
}
