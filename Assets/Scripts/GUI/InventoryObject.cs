using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Inventory Object")]
public class InventoryObject : ScriptableObject
{
    [SerializeField] public List<FurnitureObject> FurnitureObjects;

    public void Initialize()
    {

    }

    public void AddFurniture(FurnitureObject furniture)
    {
        FurnitureObjects.Add(furniture);
    }
}
