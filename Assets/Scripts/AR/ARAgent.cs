using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ARAgent : MonoBehaviour
{
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    public void MoveAgent(Vector3 position)
    {
        agent.isStopped = false;
        agent.destination = position;
    }

    public void StopAgent()
    {
        agent.isStopped = true;
    }
}
