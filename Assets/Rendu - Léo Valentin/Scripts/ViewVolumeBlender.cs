using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewVolumeBlender: MonoBehaviour
{
    private List<AViewVolume> ActiveViewVolumes = new List<AViewVolume>();
    private Dictionary<AView, List<AViewVolume>> VolumesPerViews = new Dictionary<AView, List<AViewVolume>>();

    private static ViewVolumeBlender instance;
    public static ViewVolumeBlender Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ViewVolumeBlender>();
            }
            return instance;
        }
    }

    public void AddVolume(AViewVolume volume)
    {
        if (!ActiveViewVolumes.Contains(volume))
        {
            ActiveViewVolumes.Add(volume);

            if (!VolumesPerViews.ContainsKey(volume.View))
            {
                VolumesPerViews[volume.View] = new List<AViewVolume>();
                volume.SetActive(true);
                CameraController.Instance.AddView(volume.View);
            }

            VolumesPerViews[volume.View].Add(volume);
        }
    }

    public void RemoveVolume(AViewVolume volume)
    {
        if (ActiveViewVolumes.Remove(volume))
        {
            if (VolumesPerViews.TryGetValue(volume.View, out var volumesList))
            {
                volumesList.Remove(volume);

                if (volumesList.Count == 0)
                {
                    VolumesPerViews.Remove(volume.View);
                    CameraController.Instance.RemoveView(volume.View);
                    volume.SetActive(false);
                }
            }
        }
    }

    public void Update()
    {
        foreach (var view in VolumesPerViews.Keys)
        {
            view.ResetWeight();
        }

        ActiveViewVolumes.Sort((a, b) =>
        {
            if (a.Priority == b.Priority)
                return a.Uid.CompareTo(b.Uid);
            return a.Priority.CompareTo(b.Priority);
        });

        foreach (var volume in ActiveViewVolumes)
        {
            float weight = volume.ComputeSelfWeight();
            weight = Mathf.Clamp01(weight);
            float remainingWeight = 1.0f - weight;

            foreach (var view in VolumesPerViews.Keys)
            {
                view.MultiplyWeightBy(remainingWeight);
            }

            volume.View.AddWeight(weight);
        }
    }
}
