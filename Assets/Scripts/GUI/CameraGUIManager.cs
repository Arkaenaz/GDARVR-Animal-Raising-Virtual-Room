using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGUIManager : MonoBehaviour
{
    [SerializeField] private Canvas _menuCanvas;

    public void OnBackButtonClicked()
    {
        _menuCanvas.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnSnapButtonClicked()
    {
        CameraManager.Instance.StartCaptureScreen();
    }
}
