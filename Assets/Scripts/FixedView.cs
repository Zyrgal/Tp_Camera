using UnityEngine;
using System.Collections.Generic;

public class FixedView : AView
{
    
    [Range(0, 360)] public float yaw;
    [Range(-90, 90)] public float pitch;
    [Range(-180, 180)] public float roll;
    [Range(0, 180)] public float fov;

    public override CameraConfiguration GetConfiguration()
    {
        CameraConfiguration config = new CameraConfiguration();

        config.yaw = yaw;
        config.pitch = pitch;
        config.roll = roll;
        config.pivot = transform.position;
        config.distance = 0;
        config.fov = fov;

        return config;
    }
}