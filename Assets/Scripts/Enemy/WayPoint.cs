using System;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    [Serializable]
    public class WaypointList
    {
        public string name = "";
        public List<Transform> List = new List<Transform>();
    }

    public List<WaypointList> _waypointList = new List<WaypointList>();
}
