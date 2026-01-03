using UnityEngine;

namespace BattleArena.Core
{
    [System.Serializable]
    public class PlayerData
    {
        public int level;
        public int coin;

        public PlayerData()
        {
            level = 1;
            coin = 0;
        }
    }
}
