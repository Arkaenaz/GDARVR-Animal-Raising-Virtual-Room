using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField]
    private FurnitureGUIManager _furnitureGUI;

    [SerializeField] private InventoryObject _data;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }
    public void InitializeInventory()
    {
        foreach (FurnitureObject obj in _data.FurnitureObjects)
        {
            _furnitureGUI.AddItem(obj);
        }
    }

    public void AddItem(FurnitureObject obj)
    {
        _data.FurnitureObjects.Add(obj);
    }
}
