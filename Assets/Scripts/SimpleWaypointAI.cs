using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class SimpleWaypointAI : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();
    public float maxSpeed = 18f; // m/s
    public float accelForce = 3000f;
    public float steerSpeed = 3f;
    public float reachThreshold = 4f;

    Rigidbody rb;
    int current = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.drag = 0.2f;
        rb.angularDrag = 0.4f;
    }

    void FixedUpdate()
    {
        if (waypoints.Count == 0) return;
        Transform target = waypoints[current];
        if (target == null) return;

        Vector3 toTarget = (target.position - transform.position);
        Vector3 forward = transform.forward;

        // Steering: rotate towards direction smoothly
        Vector3 desiredDir = toTarget.normalized;
        Quaternion desiredRot = Quaternion.LookRotation(desiredDir, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, steerSpeed * Time.fixedDeltaTime);

        // Throttle: simple forward force
        float currentSpeed = rb.velocity.magnitude;
        if (currentSpeed < maxSpeed)
        {
            rb.AddForce(transform.forward * accelForce * Time.fixedDeltaTime, ForceMode.Acceleration);
        }

        if (toTarget.magnitude < reachThreshold)
        {
            current = (current + 1) % waypoints.Count;
        }
    }

    // helper to set waypoints from other scripts
    public void SetWaypoints(Waypoint[] wpArray)
    {
        waypoints.Clear();
        foreach (var w in wpArray)
            if (w != null) waypoints.Add(w.transform);
    }

    public void SetWaypoints(List<Transform> wpList)
    {
        waypoints = wpList;
    }
}
