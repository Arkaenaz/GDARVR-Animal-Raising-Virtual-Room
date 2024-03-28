using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaneAnchorPlacer : MonoBehaviour
{
    ARAnchorManager anchorManager;
    ARRaycastManager raycastManager;
    [SerializeField] private float _scrollSensitivity = 100.0f;


    [SerializeField] private GameObject[] anchorPrefabs;
    [SerializeField] private GameObject currentPrefabToAnchor;

    private List<GameObject> _anchorsInScene = new();

    private List<ARRaycastHit> hitResults;

    private bool _movingObject = false;

    private GameObject _objectToMove = null;

    void Start()
    {
        anchorManager = GetComponent<ARAnchorManager>();
        raycastManager = GetComponent<ARRaycastManager>();
        currentPrefabToAnchor = anchorPrefabs[0];
        hitResults = new();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    return;
                FireEvent(Input.GetTouch(0).position);
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                return;
            FireEvent(Input.mousePosition);
        }
        MoveObject();
    }

    private void FireEvent(Vector2 position) 
    {

        if (_movingObject)
        {
            _movingObject = false;
            _objectToMove = null;
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(position);
        GameObject hitObject = GetHitObject(position);

        if (hitObject != null && !hitObject.GetComponent<ARPlane>())
        {
            if (!_movingObject)
            {
                _movingObject = true;
                _objectToMove = hitObject;
            }
        }
        else if (raycastManager.Raycast(ray, hitResults, TrackableType.PlaneWithinPolygon))
        {
            Debug.Log("Hit Count:" + hitResults.Count);
            foreach (ARRaycastHit hit in hitResults)
            {
                if (hit.trackable is ARPlane plane && (plane.alignment == PlaneAlignment.HorizontalUp || plane.alignment == PlaneAlignment.HorizontalDown))
                {
                    AnchorObject(hit.pose.position);
                    break;
                }
            }
        }
    }

    private GameObject GetHitObject(Vector2 position)
    {
        GameObject hitObject = null;
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            hitObject = hit.collider.gameObject;
        }
        return hitObject;
    }
    public void MoveObject()
    {
        if (_movingObject)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            GameObject _anchor = _objectToMove.transform.parent.gameObject;
            _anchor.transform.Rotate(0, Input.mouseScrollDelta.y * _scrollSensitivity, 0);
            if (raycastManager.Raycast(ray, hitResults, TrackableType.PlaneWithinPolygon))
            {
                foreach (ARRaycastHit hit in hitResults)
                {
                    if (hit.trackable is ARPlane plane && (plane.alignment == PlaneAlignment.HorizontalUp || plane.alignment == PlaneAlignment.HorizontalDown))
                    {
                        _anchor.transform.position = hit.pose.position;
                        break;
                    }
                }
            }
        }
    }
    public void AnchorObject(Vector3 worldPos)
    {
        GameObject newAnchor = new GameObject("NewAnchor");
        newAnchor.transform.parent = null;
        newAnchor.transform.position = worldPos;
        newAnchor.AddComponent<ARAnchor>();

        _anchorsInScene.Add(newAnchor);

        Debug.Log(currentPrefabToAnchor.transform.position.y);
        GameObject obj = Instantiate(currentPrefabToAnchor, newAnchor.transform);
        obj.transform.localPosition = currentPrefabToAnchor.transform.position;
    }

    

    public void SwitchAnchor(int objectIndex)
    {
        Debug.Log("Object Index: " + objectIndex);
        if (anchorPrefabs.Length < objectIndex)
        {
            Debug.LogError("Object Index Not in Range");
            return;
        }
        currentPrefabToAnchor = anchorPrefabs[objectIndex];
    }
}
