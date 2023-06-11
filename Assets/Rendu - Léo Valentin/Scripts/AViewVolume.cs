using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AViewVolume : MonoBehaviour
{
    public int Priority = 0;
    public AView View;
    public bool IsActive;
    public bool isCutOnSwitch;

    public int Uid { get; set; }
    private static int NextUid { get; set; } = 0;

    public virtual void SetActive(bool isActive)
    {
        IsActive = isActive;

        if (IsActive)
            ViewVolumeBlender.Instance.AddVolume(this);
        else
            ViewVolumeBlender.Instance.RemoveVolume(this);



        if (isCutOnSwitch)
        {
            ViewVolumeBlender.Instance.Update();
            CameraController.Instance.Cut();
        }
    }

    public virtual float ComputeSelfWeight()
    {
        return 1.0f;
    }

    protected virtual void Awake()
    {
        Uid = NextUid++;
    }
}
