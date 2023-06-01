using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DollyView : AView
{
    [SerializeField] float roll, distance, fov, distanceOnRail, speed;
    [SerializeField] Transform target;
    [SerializeField] Rail rail;



    public override CameraConfiguration GetConfiguration()
    {
        CameraConfiguration config = new CameraConfiguration();
        config.roll = roll;
        config.distance = distance;
        config.fov = fov;
        config.pivot = rail.GetPosition(distanceOnRail);

        if (target != null)
        {
            Vector3 dir = (target.position - config.pivot).normalized;
            float targetYaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            float targetPitch = -Mathf.Asin(dir.y) * Mathf.Rad2Deg;
            config.yaw = targetYaw;
            config.pitch = targetPitch;
        }

        return config;
    }

    private void Update()
    {
        distanceOnRail += Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        if (rail.isLoop)
            distanceOnRail = Mathf.Repeat(distanceOnRail, rail.GetLength());
        else
            distanceOnRail = Mathf.Clamp(distanceOnRail, 0f, rail.GetLength());

    }
}
