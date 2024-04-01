using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NavUtility {
    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * distance;

        randDirection += origin;

        UnityEngine.AI.NavMeshHit navHit;

        UnityEngine.AI.NavMesh.SamplePosition(randDirection, out navHit, distance, layermask);

        return navHit.position;
    }
}
