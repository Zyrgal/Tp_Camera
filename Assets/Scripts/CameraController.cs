using UnityEngine;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    public Camera camera;
    private List<AView> activeViews = new List<AView>();

    private static CameraController instance;

    public static CameraController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CameraController>();

                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "CameraController";
                    instance = obj.AddComponent<CameraController>();
                }
            }

            return instance;
        }
    }

    public void AddView(AView view)
    {
        activeViews.Add(view);
    }

    public void RemoveView(AView view)
    {
        activeViews.Remove(view);
    }

    private void Update()
    {
        CameraConfiguration averageConfig = ComputeAverageConfiguration();
        ApplyConfiguration(averageConfig);
    }

    private void ApplyConfiguration(CameraConfiguration configuration)
    {
        if (camera != null && configuration != null)
        {
            camera.transform.rotation = configuration.GetRotation();
            camera.transform.position = configuration.GetPosition();
            camera.fieldOfView = configuration.fov;
        }
    }

    private CameraConfiguration ComputeAverageConfiguration()
    {
        float totalWeight = 0f;
        Vector3 averageEulerAngles = Vector3.zero;
        Vector3 averagePivot = Vector3.zero;
        float averageFov = 0f;

        foreach (AView view in activeViews)
        {
            CameraConfiguration config = view.GetConfiguration();
            totalWeight += view.weight;

            Vector3 euler = config.GetRotation().eulerAngles;
            euler.x = Mathf.Repeat(euler.x + 180f, 360f) - 180f;
            euler.y = Mathf.Repeat(euler.y + 180f, 360f) - 180f;
            euler.z = Mathf.Repeat(euler.z + 180f, 360f) - 180f;
            averageEulerAngles += euler * view.weight;

            averagePivot += config.pivot * view.weight;
            averageFov += config.fov * view.weight;
        }

        averageEulerAngles /= totalWeight;
        averagePivot /= totalWeight;
        averageFov /= totalWeight;

        CameraConfiguration averageConfig = new CameraConfiguration();
        averageConfig.yaw = averageEulerAngles.y;
        averageConfig.pitch = averageEulerAngles.x;
        averageConfig.roll = averageEulerAngles.z;
        averageConfig.pivot = averagePivot;
        averageConfig.distance = 0;
        averageConfig.fov = averageFov;

        return averageConfig;
    }
}