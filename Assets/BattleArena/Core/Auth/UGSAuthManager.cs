using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using TMPro;
using System;

namespace BattleArena.Core
{
    public class UGSAuthManager : MonoBehaviour
    {
        public static UGSAuthManager Instance;

        public static event Action OnAuthSuccess;
        public static event Action<string> OnAuthFailed;

        private bool isInitialized = false;

        public TMP_InputField LoginUsernameInput;
        public TMP_InputField LoginPasswordInput;
        public TMP_InputField RegisterUsernameInput;
        public TMP_InputField RegisterPasswordInput;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        async void Start()
        {
            await InitUGS();
        }

        async System.Threading.Tasks.Task InitUGS()
        {
            if (isInitialized) return;

            await UnityServices.InitializeAsync();
            isInitialized = true;
        }

        public void OnClickLogin()
        {
            if (!ValidateInput(LoginUsernameInput, LoginPasswordInput))
                return;

            Login(LoginUsernameInput.text, LoginPasswordInput.text);
        }

        public void OnClickRegister()
        {
            if (!ValidateInput(RegisterUsernameInput, RegisterPasswordInput))
                return;

            Register(RegisterUsernameInput.text, RegisterPasswordInput.text);
        }

        public async void Login(string username, string password)
        {
            try
            {
                await AuthenticationService.Instance
                    .SignInWithUsernamePasswordAsync(username, password);

                await CloudSaveManager.Instance.LoadPlayerData();

                OnAuthSuccess?.Invoke();   // 👈 ยิงสัญญาณ
            }
            catch (Exception e)
            {
                OnAuthFailed?.Invoke(e.Message);
            }
        }

        public async void Register(string username, string password)
        {
            try
            {
                await AuthenticationService.Instance
                    .SignUpWithUsernamePasswordAsync(username, password);

                CloudSaveManager.Instance.CreateNewPlayerData();
                await CloudSaveManager.Instance.SavePlayerData();

                OnAuthSuccess?.Invoke();   // 👈 ยิงสัญญาณ
            }
            catch (Exception e)
            {
                OnAuthFailed?.Invoke(e.Message);
            }
        }

        bool ValidateInput(TMP_InputField username, TMP_InputField password)
        {
            if (string.IsNullOrEmpty(username.text) ||
                string.IsNullOrEmpty(password.text))
            {
                OnAuthFailed?.Invoke("Username หรือ Password ว่าง");
                return false;
            }

            if (password.text.Length < 6)
            {
                OnAuthFailed?.Invoke("Password ต้องอย่างน้อย 6 ตัว");
                return false;
            }

            return true;
        }
    }
}
