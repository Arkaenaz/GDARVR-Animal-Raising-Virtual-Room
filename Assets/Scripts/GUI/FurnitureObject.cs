using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Furniture Object")]
public class FurnitureObject : ScriptableObject
{
    public enum FurnitureType
    {
        DEFAULT,
        HUNGER,
        THIRST,
        FUN
    }

    [SerializeField]
    public FurnitureType Type;

    [SerializeField] public string Name;
    [SerializeField] public Sprite Sprite;
}
