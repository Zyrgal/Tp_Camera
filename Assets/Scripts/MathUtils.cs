using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils
{
    public static Vector3 GetNearestPointOnSegment(Vector3 a, Vector3 b, Vector3 target)
    {
        Vector3 ab = b - a;
        float lengthAB = ab.magnitude;
        Vector3 normalizedAB = ab.normalized;
        Vector3 ac = target - a;
        float dotProduct = Vector3.Dot(ac, normalizedAB);
        dotProduct = Mathf.Clamp(dotProduct, 0f, lengthAB);
        Vector3 nearestPoint = a + normalizedAB * dotProduct;
        return nearestPoint;
    }

    public static Vector3 LinearBezier(Vector3 A, Vector3 B, float t)
    {
        return A + (B - A) * t;
    }

    public static Vector3 QuadraticBezier(Vector3 A, Vector3 B, Vector3 C, float t)
    {
        return Mathf.Pow(1 - t, 2) * A + 2 * (1 - t) * t * B + Mathf.Pow(t, 2) * C;
    }

    public static Vector3 CubicBezier(Vector3 A, Vector3 B, Vector3 C, Vector3 D, float t)
    {
        return Mathf.Pow(1 - t, 3) * A + 3 * Mathf.Pow(1 - t, 2) * t * B + 3 * (1 - t) * Mathf.Pow(t, 2) * C + Mathf.Pow(t, 3) * D;
    }
}