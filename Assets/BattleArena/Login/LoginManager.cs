using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using BattleArena.Core;
using System.Collections;
using UnityEngine.UI;

namespace BattleArena.Login
{
    public class LoginManager : MonoBehaviour
    {
        public static LoginManager Instance;

        public GameObject LoginPanel;
        public GameObject RegisterPanel;
        public GameObject MainMenuPanel;
        public TextMeshProUGUI ErrorText;
        public Button PressStartBtn;

        void Awake()
        {
            Instance = this;
        }

        void OnEnable()
        {
            UGSAuthManager.OnAuthSuccess += HandleSuccess;
            UGSAuthManager.OnAuthFailed += HandleFailed;
        }

        void OnDisable()
        {
            UGSAuthManager.OnAuthSuccess -= HandleSuccess;
            UGSAuthManager.OnAuthFailed -= HandleFailed;
        }

        public void PressToStart()
        {
            SceneManager.LoadScene("Lobby");
        }

        void HandleSuccess()
        {
            RegisterPanel.SetActive(false);
            LoginPanel.SetActive(false);
            MainMenuPanel.SetActive(false);
            PressStartBtn.gameObject.SetActive(true);
        }

        void HandleFailed(string msg)
        {
            //LoadingPanel.SetActive(false);
            //LoginPanel.SetActive(true);
            ShowError(msg);
        }
        public void ShowError(string msg)
        {
            ErrorText.text = msg;
            ErrorText.gameObject.SetActive(true);

            CancelInvoke(nameof(HideError));
            Invoke(nameof(HideError), 2f);
        }

        void HideError()
        {
            ErrorText.text = "";
            ErrorText.gameObject.SetActive(false);
        }
    }
}
