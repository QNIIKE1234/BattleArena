using UnityEngine;
using Unity.Services.CloudSave;
using Unity.Services.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BattleArena.Core
{
    public class CloudSaveManager : MonoBehaviour
    {
        public static CloudSaveManager Instance;
        public PlayerData PlayerData { get; private set; }

        private const string PLAYER_DATA_KEY = "player_data";

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // =====================
        // LOAD
        // =====================
        public async Task LoadPlayerData()
        {
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                Debug.LogError("Not logged in");
                return;
            }

            var keys = new HashSet<string> { PLAYER_DATA_KEY };
            var result = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);

            if (result.TryGetValue(PLAYER_DATA_KEY, out var item))
            {
                var dict = item.Value.GetAs<Dictionary<string, object>>();

                PlayerData = new PlayerData
                {
                    level = System.Convert.ToInt32(dict["level"]),
                    coin = System.Convert.ToInt32(dict["coin"])
                };

                Debug.Log("PlayerData Loaded (JSON Object)");
            }
            else
            {
                PlayerData = new PlayerData();
                await SavePlayerData();

                Debug.Log("New PlayerData Created");
            }
        }

        // =====================
        // SAVE
        // =====================
        public async Task SavePlayerData()
        {
            if (PlayerData == null) return;

            var data = new Dictionary<string, object>
            {
                {
                    PLAYER_DATA_KEY,
                    new Dictionary<string, object>
                    {
                        { "level", PlayerData.level },
                        { "coin",  PlayerData.coin }
                    }
                }
            };

            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
            Debug.Log("PlayerData Saved (JSON Object)");
        }
    }
}
