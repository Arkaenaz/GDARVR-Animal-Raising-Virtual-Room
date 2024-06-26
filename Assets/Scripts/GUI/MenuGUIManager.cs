using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class MenuGUIManager : MonoBehaviour
{
    static public MenuGUIManager Instance;

    [SerializeField] private Canvas _expandedButtonGroup;
    [SerializeField] private Canvas _furnitureCanvas;
    
    public Canvas FurnitureCanvas
    {
        get { return _furnitureCanvas; }
    }
    [SerializeField] private Canvas _petCanvas;
    [SerializeField] private Canvas _cameraCanvas;
    
    [SerializeField] private Canvas _settings;

    [SerializeField] private Toggle _toggle;

    [SerializeField] private List<ARPlaneMeshVisualizer> planeVisualizers;

    private bool _menuOpened = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        planeVisualizers = GameObject.Find("Trackables").GetComponentsInChildren<ARPlaneMeshVisualizer>().ToList();
        foreach (ARPlaneMeshVisualizer visual in planeVisualizers)
        {
            visual.enabled = _toggle.isOn;
        }
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
        gameObject.SetActive(false);
        _furnitureCanvas.gameObject.SetActive(true);
    }

    public void OnPetButtonClicked()
    {
        _menuOpened = false;
        _expandedButtonGroup.gameObject.SetActive(_menuOpened);
        gameObject.SetActive(false);
        _petCanvas.gameObject.SetActive(true);
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
        _settings.gameObject.SetActive(true);
        // TODO : OPEN SETTINGS MENU
    }

    public void OnCloseSettingsClicked()
    {
        _settings.gameObject.SetActive(false);
    }

    public void OnExitClick()
    {
        Debug.Log("Exiting");
        Application.Quit();
    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }
}
