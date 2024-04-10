using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FurnitureItemGUI : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    private string _name;

    public event Action<FurnitureItemGUI> OnItemClicked;

    private bool _empty = true;

    public void Awake()
    {
        ResetData();
        //Deselect();
    }

    public void ResetData()
    {
        _itemImage.gameObject.SetActive(false);
        _empty = true;  
    }

    public void Deselect()
    {

    }

    public void SetData(Sprite sprite, string name)
    {
        _itemImage.gameObject.SetActive(true);
        _itemImage.sprite = sprite;
        _name = name;
        _empty = false;
    }

    public void OnPointerClick(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;
        if (pointerData.button == PointerEventData.InputButton.Left)
            OnItemClicked?.Invoke(this);
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
