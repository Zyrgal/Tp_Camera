using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereViewVolume : AViewVolume
{
    public GameObject target;
    public float outerRadius;
    public float innerRadius;

    private float distance;

    private void Update()
    {
        distance = Vector3.Distance(target.transform.position, transform.position);

        if (distance <= outerRadius && !IsActive)
        {
            SetActive(true);
        }
        else if (distance > outerRadius && IsActive)
        {
            SetActive(false);
        }
    }

    public override float ComputeSelfWeight()
    {
        float weight = 1.0f - Mathf.Clamp01((distance - innerRadius) / (outerRadius - innerRadius));
        return weight;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, outerRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, innerRadius);
    }
}
