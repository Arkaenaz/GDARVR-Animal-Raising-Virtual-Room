using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using PointerEventData = UnityEngine.EventSystems.PointerEventData;

public class PlanePlacer : MonoBehaviour
{
    public static PlanePlacer Instance;

    //[SerializeField] private ButtonsSwap btnSwp;

    [SerializeField] private ModelMover mover;

    private ARAnchorManager anchorManager;
    private ARRaycastManager raycastManager;

    [SerializeField] private float forwardOffset;

    [SerializeField] private List<GameObject> models;

    [SerializeField] private List<ARRaycastHit> hits = new();

    private int selectedModel = -1;

    public string selectedModelName = "";

    // Start is called before the first frame update
    void Start()
    {
        anchorManager = GetComponent<ARAnchorManager>();
        raycastManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {

        //forwardOffset = aUXML.Slider.value;
        
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            /*Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, 0,0);
            */

            if(mover.IsMoving)
            {
                mover.StopMoving();
            }
            else if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, -1, 0) && hit.collider.CompareTag("Model"))
            {
                Debug.Log("Hit a cube");
                mover.StartMoving(hit.collider.gameObject);
            }
            else if (raycastManager.Raycast(ray, hits, TrackableType.PlaneWithinPolygon) && !IsPointerOverUIObject() && selectedModelName != "")
            {
                foreach (ARRaycastHit arHit in hits)
                {
                    if (arHit.trackable is ARPlane plane)
                    {
                        if (plane.alignment == PlaneAlignment.HorizontalUp)
                        {
                            AnchorObject(arHit.pose.position);
                            selectedModelName = "";
                            return;
                        }
                            
                    }
                }
            }
            else
            {
                Debug.Log("Something went wrong");
            }
        }
    }

    public void AnchorObject(Vector3 worldPos)
    {
        GameObject newAnchor = new GameObject("NewAnchor");
        newAnchor.transform.parent = null;
        newAnchor.transform.position = worldPos;
        newAnchor.AddComponent<ARAnchor>();

        GameObject obj = null;

        for (int i = 0; i < models.Count; i++)
        {
            if (models[i].name == selectedModelName)
            {
                if (models[i] != null)
                {
                    obj = Instantiate(models[i], newAnchor.transform);
                    obj.transform.localPosition = Vector3.zero + Vector3.up * (obj.GetComponent<Collider>().bounds.size.y / 2);
                }
                return;
            }
        }

        
        
    }


    void HandleRaycast(ARRaycastHit hit)
    {
        if ((hit.hitType & TrackableType.Planes) != 0)
        {

        }
    }


    private static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        
        return results.Count > 0;
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



