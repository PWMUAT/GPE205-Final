using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DebugCube : MonoBehaviour
{
    public Vector3 size = new Vector3(2,2,2);
    public Vector3 offset = new Vector3(0,2,0);
    public Color color = Color.red;
    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawCube(gameObject.transform.position + offset, size);
    }
}
