using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Economy;
using UnityEngine;
using System.Linq;
namespace BattleArena.Core
{
    public class EconomyManager : MonoBehaviour
    {
        public static EconomyManager Instance;

        public long GemBalance { get; private set; }

        private const string GEM_ID = "GEM";

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
        // SYNC GEM
        // =====================
        public async Task SyncGem()
        {
            if (!AuthenticationService.Instance.IsSignedIn)
                return;

            var balancesResult = await EconomyService.Instance
                .PlayerBalances.GetBalancesAsync();

            var gemBalance = balancesResult.Balances
                .FirstOrDefault(b => b.CurrencyId == GEM_ID);

            GemBalance = gemBalance != null ? gemBalance.Balance : 0;

            Debug.Log($"Gem Synced : {GemBalance}");
        }
        // =====================
        // SPEND GEM
        // =====================
        public async Task<bool> SpendGem(int amount)
        {
            if (!AuthenticationService.Instance.IsSignedIn)
                return false;

            if (amount <= 0)
                return false;

            try
            {
                var result = await EconomyService.Instance.PlayerBalances
                    .DecrementBalanceAsync(GEM_ID, amount);

                GemBalance = result.Balance;
                Debug.Log($"Gem Spent. Remaining: {GemBalance}");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"SpendGem Failed: {e.Message}");
                return false;
            }
        }
    }
}
