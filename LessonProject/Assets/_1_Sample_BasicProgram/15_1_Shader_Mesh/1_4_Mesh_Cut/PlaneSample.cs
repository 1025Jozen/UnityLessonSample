using UnityEngine;
using System.Collections;

public class PlaneSample : MonoBehaviour
{
    public GameObject StartPoint;
    public GameObject Plane;
    public GameObject Pointer;


    void Update() {
        var n  = Plane.transform.up;
        var x  = Plane.transform.position;
        var x0 = StartPoint.transform.position;
        var m  = StartPoint.transform.forward;
        var h  = Vector3.Dot(n, x);

        var intersectPoint = x0 + ((h - Vector3.Dot(n, x0)) / (Vector3.Dot(n, m))) * m;

        Pointer.transform.position = intersectPoint;
    }
}