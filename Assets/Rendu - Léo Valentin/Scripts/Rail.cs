using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Rail : MonoBehaviour
{
    public bool isLoop;
    private float length;

    private void Start()
    {
        CalculateLength();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        for (int i = 0; i < transform.childCount; i++)
        {
            Vector3 currentNode = transform.GetChild(i).position;

            if (isLoop && i == transform.childCount - 1)
            {
                Vector3 firstNode = transform.GetChild(0).position;
                Gizmos.DrawLine(currentNode, firstNode);
            }
            else if (i < transform.childCount - 1)
            {
                Vector3 nextNode = transform.GetChild(i + 1).position;
                Gizmos.DrawLine(currentNode, nextNode);
            }

            Gizmos.DrawSphere(currentNode, 0.1f);
        }
    }

    private void CalculateLength()
    {
        length = 0f;

        for (int i = 0; i < transform.childCount; i++)
        {
            Vector3 currentNode = transform.GetChild(i).position;
            Vector3 nextNode;

            if (isLoop)
            {
                nextNode = transform.GetChild((i + 1) % transform.childCount).position;
            }
            else
            {
                if (i < transform.childCount - 1)
                {
                    nextNode = transform.GetChild(i + 1).position;
                }
                else
                {
                    break;
                }
            }

            length += Vector3.Distance(currentNode, nextNode);
        }
    }

    public float GetLength()
    {
        return length;
    }

    public Vector3 GetPosition(float distance)
    {
        int nodeCount = transform.childCount;

        if (isLoop)
            distance %= length;
        else
            distance = Mathf.Clamp(distance, 0f, length);

        for (int i = 0; i < nodeCount; i++)
        {
            Transform currentNode = transform.GetChild(i);
            Transform nextNode = transform.GetChild((i + 1) % nodeCount);
            float segmentLength = Vector3.Distance(currentNode.position, nextNode.position);

            if (distance <= segmentLength)
            {
                return Vector3.Lerp(currentNode.position, nextNode.position, distance / segmentLength);
            }

            distance -= segmentLength;
        }

        return Vector3.zero;
    }
}
