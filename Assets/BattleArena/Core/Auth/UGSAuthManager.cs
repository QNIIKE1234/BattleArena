using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using TMPro;

namespace BattleArena.Core
{
    public class UGSAuthManager : MonoBehaviour
    {
        
        private bool isInitialized = false;

        public TMP_InputField LoginUsernameInput;

        public TMP_InputField LoginPasswordInput;

        public TMP_InputField RegisterUsernameInput;

        public TMP_InputField RegisterPasswordInput;

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
                //Login("test001", "Test@1234");
            }
        }

        async System.Threading.Tasks.Task InitUGS()
        {
            if (isInitialized) return;

            await UnityServices.InitializeAsync();
            isInitialized = true;

            Debug.Log("UGS Initialized");
        }

        public void OnClickLogin()
        {
            Login(LoginUsernameInput.text, LoginPasswordInput.text);
        }

        public void OnClickRegister()
        {
            Register(RegisterUsernameInput.text, RegisterPasswordInput.text);
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

        public async void Register(string username, string password)
        {
            try
            {
                await AuthenticationService.Instance
                    .SignUpWithUsernamePasswordAsync(username, password);

                Debug.Log("REGISTER SUCCESS");

                // ===== สร้าง PlayerData ครั้งแรก =====
                CloudSaveManager.Instance.CreateNewPlayerData();

                await CloudSaveManager.Instance.SavePlayerData();
                Debug.Log("Create PlayerData + Save");
            }
            catch (AuthenticationException e)
            {
                Debug.LogError($"Register Failed: {e.Message}");
            }
            catch (RequestFailedException e)
            {
                Debug.LogError($"Register Failed: {e.Message}");
            }
        }
    }
}
