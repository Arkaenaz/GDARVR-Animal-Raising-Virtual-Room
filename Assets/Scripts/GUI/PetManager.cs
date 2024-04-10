using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetManager : MonoBehaviour
{
    public static PetManager Instance;

    //[SerializeField] private PetGUIManager _petGUI;

    public Bar HungerBar;
    public Bar ThirstBar;
    public Bar MoodBar;

    public List<CatBehaviour> _pets;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    //public void InitializePet

    public void AddPet(CatBehaviour pet)
    {
        _pets.Add(pet);
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
