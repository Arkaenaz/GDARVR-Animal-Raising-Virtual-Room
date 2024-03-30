using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGUIManager : MonoBehaviour
{
    [SerializeField] private Canvas _expandedButtonGroup;
    [SerializeField] private Canvas _cameraCanvas;

    private bool _menuOpened = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMenuButtonClicked()
    {
        _menuOpened = !_menuOpened;
        _expandedButtonGroup.gameObject.SetActive(_menuOpened);
    }

    public void OnFurnishButtonClicked()
    {
        _menuOpened = false;
        _expandedButtonGroup.gameObject.SetActive(_menuOpened);
        // TODO : OPEN FURNISH MENU
    }

    public void OnPetButtonClicked()
    {
        _menuOpened = false;
        _expandedButtonGroup.gameObject.SetActive(_menuOpened);
        // TODO : OPEN PET MENU
    }

    public void OnCameraButtonClicked()
    {
        _menuOpened = false;
        _expandedButtonGroup.gameObject.SetActive(_menuOpened);
        gameObject.SetActive(false);
        _cameraCanvas.gameObject.SetActive(true);
    }

    public void OnSettingsButtonClicked()
    {
        _menuOpened = false;
        _expandedButtonGroup.gameObject.SetActive(_menuOpened);
        // TODO : OPEN SETTINGS MENU
    }
}
