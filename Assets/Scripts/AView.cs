using UnityEngine;
using System.Collections.Generic;

public abstract class AView : MonoBehaviour
{
    public float weight;
    public bool isActiveOnStart = true;

    public abstract CameraConfiguration GetConfiguration();

    private void OnEnable()
    {
        if (isActiveOnStart)
            CameraController.Instance.AddView(this);
    }

    private void OnDisable()
    {
        CameraController.Instance.RemoveView(this);
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);

        if (isActive)
            CameraController.Instance.AddView(this);
        else
            CameraController.Instance.RemoveView(this);
    }

    private void Start()
    {
        SetActive(isActiveOnStart);
    }

    /*private void OnDestroy()
    {
        CameraController.Instance.RemoveView(this);
    }*/
}