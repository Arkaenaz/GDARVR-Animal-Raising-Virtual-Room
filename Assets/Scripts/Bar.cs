using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] private Image _barSprite;

    public void UpdateBar(float max, float current)
    {
        Debug.Log(name + " updating");
        Debug.Log(name + " " + current + "/" + max);
        _barSprite.fillAmount = current / max;
        Debug.Log(name + " fill amount : " + _barSprite.fillAmount);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
