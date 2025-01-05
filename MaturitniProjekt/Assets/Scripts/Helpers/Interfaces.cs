using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts {
    public enum WorkshopSearchOptions
    {
        sortByVote,
        SortByDate,
        madeByFriends,
        mostPlayed,
        trending
    }

    [System.Serializable]
    public class LevelData
    {
        public string name;
        public bool isCoop;
        public string steamId;
        public List<ObjectData> objects;
    }

    [System.Serializable]
    public class ObjectData
    {
        public string name;
        public Vector3 position;
        public Quaternion rotation;
        public List<Vector3> connectionData;
    }
    public enum direcions
    {
        up,
        down,
        left,
        right,
        forward,
        back
    }
    public enum BuildModes
    {
        logic,
        build,
        rotation,
    }
    [Serializable]
    public class Connection
    {
        public GameObject connectedObject;
        public GameObject ConnectionLine;
    }
}
