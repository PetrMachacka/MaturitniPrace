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
        public string steamId;
        public List<ObjectData> objects;
    }

    [System.Serializable]
    public class ObjectData
    {
        public string name;
        public Vector3 position;
        public Quaternion rotation;
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
}
