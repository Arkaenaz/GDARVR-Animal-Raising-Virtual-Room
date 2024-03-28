using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityStatField
{
    public float maxThirst = 100.0f;
    public float maxHunger = 100.0f;
    public float maxMood = 100.0f;

    public float currentThirst = 100.0f;
    public float currentHunger = 100.0f;
    public float currentMood = 100.0f;
}
