using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TrackedImageObjectHandler : MonoBehaviour
{
    /*public GameObject[] objectsA;
    public GameObject[] objectsB;

    private GameObject[] currentGameObjects;*/
    [SerializeField] private GameObject contentObject;
    [SerializeField] private float yOffset = 0.05f;
    private void Start()
    {
        Debug.Log("ho");
        //currentGameObjects = objectsA;
    }
    public void OnTrackedImageChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        
        foreach (ARTrackedImage image in eventArgs.added)
        {
            
            Debug.Log("Tracked new Image : " + image.referenceImage.name + " | Tracking State : " + image.trackingState);
            GameObject obj = Instantiate(contentObject);
            obj.transform.SetParent(image.transform, true);
            obj.transform.position = image.transform.position;


            //if (image.referenceImage.name == "cobblestone")
                //InventoryManager.Instance.ADD
            
        }
        foreach (ARTrackedImage image in eventArgs.updated)
        {
            // Handle updated event
            /*foreach (Transform child in image.transform)
            {
                Destroy(child.gameObject);
            }*/
            Debug.Log("Updated Image : " + image.referenceImage.name + " | Tracking State : " + image.trackingState);
            /*if (image.referenceImage.name == "cobblestone")
                Instantiate(currentGameObjects[0], image.transform);
            if (image.referenceImage.name == "dlsu-logo")
                Instantiate(currentGameObjects[1], image.transform);
            if (image.referenceImage.name == "dog")
                Instantiate(currentGameObjects[2], image.transform);*/
            if (image.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
                AgentManager.Instance.SetIsTrackedImageFound(true);
            else
            {
                AgentManager.Instance.SetIsTrackedImageFound(false);
            }
        }
        foreach (ARTrackedImage image in eventArgs.removed)
        {
            // Handle removed event
            
            Debug.Log("Removed Image : " + image.referenceImage.name + " | Tracking State : " + image.trackingState);
        }
    }

    void GenerateFurnitureObj()
    {
        //FurnitureObject
    }

    /*public void SwitchObjects(int value)
    {
        if (value == 0)
        {
            currentGameObjects = objectsA;
        }
        else
            currentGameObjects = objectsB;
    }*/
}
