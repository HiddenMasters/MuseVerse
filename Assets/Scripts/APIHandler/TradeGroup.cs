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

    public static void BuyItem()
    {
        int item = AtomManager.ExhibitionItem;
        int trade = AtomManager.ExhibitionTrade;
        
        AtomManager.StartCreateOrder(item, trade);
    }
}
