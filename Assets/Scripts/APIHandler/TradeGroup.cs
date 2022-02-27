using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeGroup : MonoBehaviour
{
    private static GameObject _tradeGroup;
    public static void OpenTradeGroup()
    {
        AtomManager.CloseLastPanel();
        _tradeGroup = GameObject.Find("Trade Group");
        RectTransform transform = _tradeGroup.GetComponent<RectTransform>();
        transform.anchoredPosition = Vector3.zero;
        AtomManager.LastPanel = "Trade Group";
    }

    public static void BuyItem()
    {
        int item = AtomManager.ExhibitionItem;
        int trade = AtomManager.ExhibitionTrade;
        
        AtomManager.StartCreateOrder(item, trade);
    }
}
