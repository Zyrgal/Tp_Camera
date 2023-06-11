using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curve : MonoBehaviour
{
    public Vector3 A;
    public Vector3 B;
    public Vector3 C;
    public Vector3 D;

    public Vector3 GetPosition(float t)
    {
        return MathUtils.CubicBezier(A, B, C, D, t);
    }

    public Vector3 GetPosition(float t, Matrix4x4 localToWorldMatrix)
    {
        Vector3 localPosition = GetPosition(t);
        Vector3 worldPosition = localToWorldMatrix.MultiplyPoint3x4(localPosition);
        return worldPosition;
    }

    public void DrawGizmo(Color c, Matrix4x4 localToWorldMatrix)
    {
        Gizmos.color = c;
        Gizmos.matrix = localToWorldMatrix;

        Vector3 prevPoint = GetPosition(0f);
        int resolution = 20;

        for (int i = 1; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            Vector3 point = GetPosition(t);
            Gizmos.DrawLine(prevPoint, point);
            prevPoint = point;
        }

        Gizmos.DrawSphere(A, 0.1f);
        Gizmos.DrawSphere(B, 0.1f);
        Gizmos.DrawSphere(C, 0.1f);
        Gizmos.DrawSphere(D, 0.1f);
    }
}
