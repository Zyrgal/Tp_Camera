using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFollowView : AView
{
    public float[] pitch = new float[3];
    public float[] roll = new float[3];
    public float[] fov = new float[3];

    public float yaw;
    public float yawSpeed;
    public Transform target;
    public Curve curve;
    public float curvePosition;
    public float curveSpeed;
    public float distance;

    private Matrix4x4 curveToWorldMatrix;

    private void Start()
    {
        CalculateCurveToWorldMatrix();
    }

    private void Update()
    {
        float inputYaw = Input.GetAxis("Horizontal");
        yaw += inputYaw * yawSpeed * Time.deltaTime;

        float inputCurve = Input.GetAxis("Vertical");
        curvePosition = Mathf.Clamp01(curvePosition + inputCurve * curveSpeed * Time.deltaTime);

        CalculateCurveToWorldMatrix();
    }

    private void CalculateCurveToWorldMatrix()
    {
        Vector3 curvePositionWorld = target.transform.position;
        Quaternion rotation = Quaternion.Euler(0, yaw, 0);
        Vector3 position = curvePositionWorld - rotation * Vector3.forward * distance;

        curveToWorldMatrix = Matrix4x4.TRS(position, rotation, Vector3.one);
    }

    public override CameraConfiguration GetConfiguration()
    {
        Vector3 position = curve.GetPosition(curvePosition, curveToWorldMatrix);

        Quaternion rotation;
        if (curvePosition <= 0.5f)
        {
            rotation = Quaternion.Lerp(Quaternion.Euler(pitch[0], 0f, roll[0]), Quaternion.Euler(pitch[1], 0f, roll[1]), curvePosition * 2f);
        }
        else
        {
            rotation = Quaternion.Lerp(Quaternion.Euler(pitch[1], 0f, roll[1]), Quaternion.Euler(pitch[2], 0f, roll[2]), (curvePosition - 0.5f) * 2f);
        }

        float newYaw = yaw;
        float newPitch = rotation.eulerAngles.x;
        float newRoll = rotation.eulerAngles.z;

        return new CameraConfiguration()
        {
            pivot = position,
            yaw = newYaw,
            pitch = newPitch,
            roll = newRoll,
            distance = distance,
            fov = fov[1]
        };
    }

    private void OnDrawGizmos()
    {
        if (curve != null)
        {
            curve.DrawGizmo(Color.green, transform.localToWorldMatrix);
        }
    }
}
