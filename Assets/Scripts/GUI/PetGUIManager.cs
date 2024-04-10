using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetGUIManager : MonoBehaviour
{
    [SerializeField] private Canvas _menuCanvas;

    public void OnBackButtonClicked()
    {
        _menuCanvas.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
