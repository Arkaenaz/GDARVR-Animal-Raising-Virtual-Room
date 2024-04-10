using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetGUIManager : MonoBehaviour
{
    [SerializeField] private Canvas _menuCanvas;

    public Bar HungerBar;
    public Bar ThirstBar;
    public Bar MoodBar;

    public void OnBackButtonClicked()
    {
        _menuCanvas.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        PetManager.Instance.HungerUpdate += HungerBar.UpdateBar;
        PetManager.Instance.ThirstUpdate += ThirstBar.UpdateBar;
        PetManager.Instance.MoodUpdate += MoodBar.UpdateBar;
    }

    void OnDisable()
    {
        PetManager.Instance.HungerUpdate -= HungerBar.UpdateBar;
        PetManager.Instance.ThirstUpdate -= ThirstBar.UpdateBar;
        PetManager.Instance.MoodUpdate -= MoodBar.UpdateBar;
    }
}
