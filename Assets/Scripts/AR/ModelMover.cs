using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ModelMover : MonoBehaviour
{

    private bool isMoving = false;
    public bool IsMoving
    {
        get { return isMoving; }
    }


    private GameObject targetObj = null;

    private ARRaycastManager raycastManager;

    [SerializeField] private List<ARRaycastHit> hits = new();

    // Start is called before the first frame update
    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isMoving && targetObj && transform.hasChanged)
        {
            Debug.Log("Moving");
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            if (raycastManager.Raycast(ray, hits, TrackableType.PlaneWithinPolygon))
            {
                foreach (ARRaycastHit arHit in hits)
                {
                    if (arHit.trackable is ARPlane plane)
                    {
                        if (plane.alignment == PlaneAlignment.HorizontalUp)
                        {
                            targetObj.transform.position = arHit.pose.position + Vector3.up * (targetObj.GetComponent<Collider>().bounds.size.y / 2);
                            return;
                        }
                            
                    }
                }
            }
        }
        
    }

    

    public void StartMoving(GameObject targetObj)
    {
        isMoving = true;
        this.targetObj = targetObj;
    }

    public void StopMoving()
    {
        isMoving = false;
        this.targetObj = null;
    }
}
