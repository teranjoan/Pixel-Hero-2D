using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private ItemsManager itemsManager;


    void Start(){
        itemsManager = FindObjectOfType<ItemsManager>();
    }

    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 120, 120), itemsManager.GetLeftItemsOnLevelString());
    }
}
