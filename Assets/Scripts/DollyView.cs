using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DollyView : AView
{
    public float roll;
    public float distance;
    public float fov;
    public Transform target;
    public Rail rail;
    public float speed;
    public bool isAuto;

    private float distanceOnRail;

    private void Update()
    {
        if (isAuto)
        {
            UpdateAutoFollow();
        }
        else
        {
            UpdateManualControl();
        }
    }

    private void UpdateAutoFollow()
    {
        if (rail == null || target == null)
            return;

        float nearestDistance = float.MaxValue;
        float nearestDistanceOnRail = 0f;

        for (float d = 0f; d <= rail.GetLength(); d += 0.1f)
        {
            Vector3 railPosition = rail.GetPosition(d);
            float distanceToTarget = Vector3.Distance(railPosition, target.position);

            if (distanceToTarget < nearestDistance)
            {
                nearestDistance = distanceToTarget;
                nearestDistanceOnRail = d;
            }
        }

        distanceOnRail = nearestDistanceOnRail;
    }

    private void UpdateManualControl()
    {
        float input = Input.GetAxis("Horizontal");
        distanceOnRail += input * speed * Time.deltaTime;

        if (rail != null)
        {
            float railLength = rail.GetLength();

            if (rail.isLoop)
            {
                distanceOnRail %= railLength;
                if (distanceOnRail < 0)
                    distanceOnRail += railLength;
            }
            else
            {
                distanceOnRail = Mathf.Clamp(distanceOnRail, 0f, railLength);
            }
        }
    }

    public override CameraConfiguration GetConfiguration()
    {
        Vector3 railPosition = rail != null ? rail.GetPosition(distanceOnRail) : Vector3.zero;

        Vector3 direction = (target.position - railPosition).normalized;
        float yaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float pitch = -Mathf.Asin(direction.y) * Mathf.Rad2Deg;

        CameraConfiguration config = new CameraConfiguration();
        config.pivot = railPosition;
        config.yaw = yaw;
        config.pitch = pitch;
        config.roll = roll;
        config.distance = distance;
        config.fov = fov;

        return config;
    }
}
