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
        public List<ObjectData> objects;
    }

    [System.Serializable]
    public class ObjectData
    {
        public string name;
        public Vector3 position;
    }
}
