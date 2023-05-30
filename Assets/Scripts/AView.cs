using UnityEngine;
using System.Collections.Generic;

public abstract class AView : MonoBehaviour
{
    public float weight;
    public bool isActiveOnStart = true;

    public abstract CameraConfiguration GetConfiguration();

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);

        if (isActive)
            CameraController.Instance.AddView(this);
    }

    private void Start()
    {
        SetActive(isActiveOnStart);
    }
}