using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] private Image _barSprite;

    public void UpdateBar(float max, float current)
    {
        // Debug.Log(name + " updating");
        // Debug.Log(name + " " + current + "/" + max);
        Vector3 scale = _barSprite.GetComponent<RectTransform>().localScale;
        _barSprite.GetComponent<RectTransform>().localScale = new Vector3(current / max, scale.y, scale.z);
        // Debug.Log(name + " fill amount : " + _barSprite.fillAmount);
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
