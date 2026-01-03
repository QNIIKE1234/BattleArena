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
            try
            {
                await AuthenticationService.Instance
                    .SignInWithUsernamePasswordAsync(username, password);

                Debug.Log("LOGIN SUCCESS");
                Debug.Log("PlayerId : " + AuthenticationService.Instance.PlayerId);

                await CloudSaveManager.Instance.LoadPlayerData();
            }
            catch (AuthenticationException e)
            {
                Debug.LogError("LOGIN ERROR : " + e.Message);
            }
            catch (RequestFailedException e)
            {
                Debug.LogError("LOGIN REQUEST ERROR : " + e.Message);
            }
        }
    }
}
