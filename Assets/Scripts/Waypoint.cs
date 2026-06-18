using UnityEngine;

// Simple marker component for waypoint placement in scene
public class Waypoint : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.6f);
        Gizmos.DrawIcon(transform.position + Vector3.up * 0.5f, "sv_label_0", true);
    }
}
