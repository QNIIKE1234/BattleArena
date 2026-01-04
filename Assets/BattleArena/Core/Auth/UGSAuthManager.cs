using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace BattleArena.Core
{
    public class UGSAuthManager : MonoBehaviour
    {
        private bool isInitialized = false;

        async void Start()
        {
            await InitUGS();

            if (AuthenticationService.Instance.IsSignedIn)
            {
                Debug.Log("Already Logged In");
                await CloudSaveManager.Instance.LoadPlayerData();
            }
            else
            {
                Login("test001", "Test@1234");
            }
        }

        async System.Threading.Tasks.Task InitUGS()
        {
            if (isInitialized) return;

            await UnityServices.InitializeAsync();
            isInitialized = true;

            Debug.Log("UGS Initialized");
        }

        public async void Login(string username, string password)
        {
            await AuthenticationService.Instance
                .SignInWithUsernamePasswordAsync(username, password);

            Debug.Log("LOGIN SUCCESS");

            await CloudSaveManager.Instance.LoadPlayerData();

          
            CloudSaveManager.Instance.PlayerData.coin += 100;
            await CloudSaveManager.Instance.SavePlayerData();
            Debug.Log("Add coin + Save");
        }
    }
}
