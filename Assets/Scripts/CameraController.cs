using UnityEngine;using System.Collections.Generic;
using Unity.VisualScripting;

public class CameraController : MonoBehaviour
{
    new public Camera camera;

    [SerializeField] private List<AView> activeViews = new List<AView>();
    CameraConfiguration currentConfiguration;
    CameraConfiguration targetConfiguration;
    [SerializeField] float smoothSpeed;


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
        if(!activeViews.Contains(view))
            activeViews.Add(view);
    }

    public void RemoveView(AView view)
    {
        activeViews.Remove(view);
    }

    private void Update()
    {
        CameraConfiguration averageConfig = ComputeAverageConfiguration();
        SetTargetConfiguration(averageConfig);
        UpdateCameraConfiguration();
    }

    private void SetTargetConfiguration(CameraConfiguration cameraConfiguration)
    {
        targetConfiguration = cameraConfiguration;
    }

    private void UpdateCameraConfiguration()
    {
        if (camera != null && targetConfiguration != null)
        {
            if (currentConfiguration == null)
            {
                currentConfiguration = targetConfiguration;
            }

            float deltaSpeed = smoothSpeed * Time.deltaTime;

            currentConfiguration.pivot = Vector3.Lerp(currentConfiguration.pivot, targetConfiguration.pivot, deltaSpeed);
            currentConfiguration.distance = Mathf.Lerp(currentConfiguration.distance, targetConfiguration.distance, deltaSpeed);
            currentConfiguration.fov = Mathf.Lerp(currentConfiguration.fov, targetConfiguration.fov, deltaSpeed);

            currentConfiguration.pitch = Mathf.Lerp(currentConfiguration.pitch, targetConfiguration.pitch, deltaSpeed);
            currentConfiguration.roll = Mathf.Lerp(currentConfiguration.roll, targetConfiguration.roll, deltaSpeed);
            currentConfiguration.yaw = Mathf.Lerp(currentConfiguration.yaw, targetConfiguration.yaw, deltaSpeed);

            ApplyConfiguration(currentConfiguration);
        }
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
        Vector2 yawSum = Vector2.zero;

        foreach (AView view in activeViews)
        {
            CameraConfiguration config = view.GetConfiguration();
            totalWeight += view.weight;

            yawSum += new Vector2( Mathf.Cos(config.yaw * Mathf.Deg2Rad),
                                Mathf.Sin(config.yaw * Mathf.Deg2Rad)) * view.weight;
            

            Vector3 euler = config.GetRotation().eulerAngles;
            euler.x = Mathf.Repeat(euler.x + 180f, 360f) - 180f;
            euler.y = Mathf.Repeat(euler.y + 180f, 360f) - 180f;
            euler.z = Mathf.Repeat(euler.z + 180f, 360f) - 180f;
            averageEulerAngles += euler * view.weight;

            averagePivot += config.pivot * view.weight;
            averageFov += config.fov * view.weight;
        }

        float averageYaw = Vector2.SignedAngle(Vector2.right, yawSum);

        averageEulerAngles /= totalWeight;
        averagePivot /= totalWeight;
        averageFov /= totalWeight;

        CameraConfiguration averageConfig = new CameraConfiguration();
        averageConfig.yaw = averageYaw;
        averageConfig.pitch = averageEulerAngles.x;
        averageConfig.roll = averageEulerAngles.z;
        averageConfig.pivot = averagePivot;
        averageConfig.distance = 0;
        averageConfig.fov = averageFov;

        return averageConfig;
    }

    private void OnDrawGizmos()
    {
        foreach (var view in activeViews)
        {
            view.GetConfiguration().DrawGizmos(Color.blue);
        }

        ComputeAverageConfiguration().DrawGizmos(Color.red);
    }
}