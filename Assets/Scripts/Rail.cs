using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Rail : MonoBehaviour
{
    [SerializeField] List<Transform> childs;
    public bool isLoop = false;
    float length = 0;

    private void OnValidate()
    {
        childs.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            childs.Add(transform.GetChild(i));
        }
    }

    private void Start()
    {
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            length += Vector3.Distance(transform.GetChild(i).position, transform.GetChild(i + 1).position);
        }
    }

    public float GetLength()
    {
        return length;
    }

    public Vector3 GetPosition(float distance)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (Vector3.Distance(transform.position, transform.GetChild(i).position) == distance)
            {
                return transform.GetChild(i).position;
            }
        }

        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        if (childs.Count <= 0)
            return;

        Gizmos.color = Color.green;

        for (int i = 0; i < childs.Count; i++)
        {
            Gizmos.DrawSphere(childs[i].position, 0.25f);
            if (i < childs.Count - 1)
            {
                Gizmos.DrawLine(childs[i].position, childs[i+1].position);
            }
        }
    }

}
