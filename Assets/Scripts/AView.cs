using UnityEngine;
using System.Collections.Generic;

public abstract class AView : MonoBehaviour
{
    public float weight;

    public abstract CameraConfiguration GetConfiguration();

    public void ResetWeight()
    {
        weight = 0.0f;
    }

    public void MultiplyWeightBy(float value)
    {
        weight *= value;
    }

    public void AddWeight(float value)
    {
        weight += value;
    }
}