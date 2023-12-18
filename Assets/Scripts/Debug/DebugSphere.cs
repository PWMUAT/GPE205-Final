using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class DebugSphere : MonoBehaviour
{
    public float radius;
    public Color color = Color.green;
    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(gameObject.transform.position, radius);
    }
}
