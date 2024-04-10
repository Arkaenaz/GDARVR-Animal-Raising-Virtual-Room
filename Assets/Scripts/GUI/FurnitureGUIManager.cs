using System.Collections.Generic;
using UnityEngine;

public class FurnitureGUIManager : MonoBehaviour
{
    [SerializeField] private Canvas _menuCanvas;

    [SerializeField] private FurnitureItemGUI _itemPrefab;
    [SerializeField] private RectTransform _contentPanel;

    private List<FurnitureItemGUI> _furnitureList = new List<FurnitureItemGUI>();

    public void InitializeInventory()
    {
        for (int i = 0; i < 99; i++)
        {
            FurnitureItemGUI item = Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity);
            item.transform.parent = _contentPanel;
            _furnitureList.Add(item);

            item.OnItemClicked += HandleSelectFurniture;
        }
    }

    public void AddItem(FurnitureObject obj)
    {
        FurnitureItemGUI item = Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity);
        item.transform.parent = _contentPanel;
        item.SetData(obj.Sprite, obj.Name);
        _furnitureList.Add(item);

        item.OnItemClicked += HandleSelectFurniture;
    }

    public void ClearList()
    {
        _furnitureList.Clear();
    }

    public void HandleSelectFurniture(FurnitureItemGUI item)
    {
        Debug.Log("CLICK");
    }


    public void OnBackButtonClicked()
    {
        _menuCanvas.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        InventoryManager.Instance.InitializeInventory();
    }

    void OnDisable()
    {
        foreach (FurnitureItemGUI item in _furnitureList)
        {
            Destroy(item.gameObject);
        }
        ClearList();
    }
}