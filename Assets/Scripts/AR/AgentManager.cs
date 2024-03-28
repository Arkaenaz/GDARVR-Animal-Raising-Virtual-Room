using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    public static AgentManager Instance;
    private List<ARAgent> agents;
    private bool isTrackedImageFound = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        agents = new List<ARAgent>(GetComponentsInChildren<ARAgent>());
    }

    void Update()
    {
        if (!isTrackedImageFound)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(r, out hitInfo))
            {
                if (hitInfo.collider.CompareTag("Plane"))
                {
                    MoveAllAgents(hitInfo.point);
                }
            }
        }
    }

    public void MoveAllAgents(Vector3 position)
    {
        foreach (ARAgent agent in agents)
        {
            agent.MoveAgent(position);
        }
    }

    public void StopAllAgents()
    {
        foreach (ARAgent agent in agents)
        {
            agent.StopAgent();
        }
    }

    public void SetIsTrackedImageFound(bool val)
    {
        isTrackedImageFound = val;
    }
}
