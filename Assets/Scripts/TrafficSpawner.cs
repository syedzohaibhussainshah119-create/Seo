using UnityEngine;
using System.Collections.Generic;

public class TrafficSpawner : MonoBehaviour
{
    public GameObject trafficPrefab;
    public List<Transform> spawnPoints = new List<Transform>();
    public float spawnInterval = 3f;
    public int maxTraffic = 12;

    List<GameObject> spawned = new List<GameObject>();

    void Start()
    {
        InvokeRepeating(nameof(SpawnTick), 1f, spawnInterval);
    }

    void SpawnTick()
    {
        spawned.RemoveAll(x => x == null);
        if (spawned.Count >= maxTraffic) return;
        if (spawnPoints.Count == 0 || trafficPrefab == null) return;

        var sp = spawnPoints[Random.Range(0, spawnPoints.Count)];
        var car = Instantiate(trafficPrefab, sp.position, sp.rotation);
        var ai = car.AddComponent<SimpleWaypointAI>();
        // If you have Waypoint objects placed, find them
        var wps = GameObject.FindObjectsOfType<Waypoint>();
        if (wps.Length > 0) ai.SetWaypoints(wps);
        spawned.Add(car);
    }
}
