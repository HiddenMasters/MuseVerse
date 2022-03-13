using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawingZone : MonoBehaviour
{
    private string message = "그림그리기를 시작하시겠습니까?";
    private Button confirm;
    private Button cancel;
    
    private void OnTriggerEnter(Collider other)
    {
        AtomManager.OpenConfirmPanel(message);
    }

    public void ConfirmButton()
    {
        AtomManager.ClosePanel();
        AtomManager.OpenPanel("Drawing Group");
    }

    public void CancelButton()
    {
        
    }
}
