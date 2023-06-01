using UnityEngine;

public class FixedFollowView : AView
{
    public float roll;
    public float fov;
    public Transform target;

    // Contraintes de suivi
    public GameObject centralPoint;
    public float yawOffsetMax;
    public float pitchOffsetMax;

    public override CameraConfiguration GetConfiguration()
    {
        CameraConfiguration config = new CameraConfiguration();
        config.pivot = transform.position;
        config.roll = roll;
        config.fov = fov;

        if (target != null)
        {
            Vector3 dir = (target.position - transform.position).normalized;

            // Calcul du yaw et du pitch avec les contraintes
            float targetYaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            float targetPitch = - Mathf.Asin(dir.y) * Mathf.Rad2Deg;

            if (centralPoint != null)
            {
                Vector3 centralDir = (centralPoint.transform.position - transform.position).normalized;
                float centralYaw = Mathf.Atan2(centralDir.x, centralDir.z) * Mathf.Rad2Deg;
                float centralPitch = -Mathf.Asin(centralDir.y) * Mathf.Rad2Deg;

                float yawDifference = targetYaw - centralYaw;
                float pitchDifference = targetPitch - centralPitch;
                
                // Vérification des contraintes de yaw
                yawDifference = Mathf.Repeat(yawDifference + 180f, 360f) - 180f;

                if (Mathf.Abs(yawDifference) > yawOffsetMax)
                {
                    targetYaw = centralYaw + Mathf.Sign(yawDifference) * yawOffsetMax;
                }

                // Vérification des contraintes de pitch
                pitchDifference = Mathf.Repeat(pitchDifference + 180f, 360f) - 180f;

                if (Mathf.Abs(pitchDifference) > pitchOffsetMax)
                {
                    targetPitch = centralPitch + Mathf.Sign(pitchDifference) * pitchOffsetMax;
                }
            }

            config.yaw = targetYaw;
            config.pitch = targetPitch;
        }

        return config;
    }
}