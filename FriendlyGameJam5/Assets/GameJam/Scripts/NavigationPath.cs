﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationPath : MonoBehaviour {
    public List<Transform> Waypoints;
    public float StopTime { get { return Random.Range(MinStopTime, MaxStopTime); } }
    public float MinStopTime = 1;
    public float MaxStopTime = 1;

    private Transform lastWaypoint = null;

    private void Awake()
    {
        lastWaypoint = null;
    }

    public Transform GetNextWaypoint()
    {
        if (Waypoints.Count == 0) return null;
        if (Waypoints.Count == 1) return Waypoints[0];
        if (lastWaypoint != null)
        {
            Waypoints.Remove(lastWaypoint);
        }
        int index = Mathf.RoundToInt(Random.Range(0, Waypoints.Count - 1));
        Transform toRet = Waypoints[index];
        if (lastWaypoint != null)
        {
            Waypoints.Add(lastWaypoint);
        }
        lastWaypoint = toRet;
        return toRet;
    }

    private void OnDrawGizmosSelected()
    {
        foreach (Transform t in Waypoints)
        {
            Gizmos.DrawWireSphere(t.position, 0.5f);
        }
    }
}
