using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MissionManager : MonoBehaviour
{
    [Header("Missions")]
    public List<Transform> missionTargets = new List<Transform>();
    public float reachRadius = 12f;

    [Header("UI")]
    public Text missionText;

    int currentMission = 0;

    void Start()
    {
        UpdateMissionText();
    }

    void Update()
    {
        if (currentMission < missionTargets.Count)
        {
            Transform t = missionTargets[currentMission];
            if (t == null) return;
            if (Vector3.Distance(PlayerPosition(), t.position) < reachRadius)
            {
                CompleteMission();
            }
        }
    }

    Vector3 PlayerPosition()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        return p != null ? p.transform.position : Vector3.zero;
    }

    void CompleteMission()
    {
        Debug.Log("Mission " + currentMission + " complete!");
        currentMission++;
        UpdateMissionText();
        // Reward player: could add score, spawn vehicles, unlock waypoint, etc.
    }

    void UpdateMissionText()
    {
        if (missionText == null) return;
        if (currentMission < missionTargets.Count)
            missionText.text = "Mission: Reach " + missionTargets[currentMission].name;
        else
            missionText.text = "All missions complete!";
    }
}
