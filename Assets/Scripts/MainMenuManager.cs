using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Connect.Core
{
    public class MainMenuManager : MonoBehaviour
    {
        public static MainMenuManager Instance;

        [SerializeField] private GameObject _playPanel;
        [SerializeField] private GameObject _levelPanel;
        public AudioSource clickSound;

        public UnityAction LevelOpened;

        [HideInInspector]
        public Color CurrentColor;

        private void Awake()
        {
            Instance = this;
            _playPanel.SetActive(true);
            _levelPanel.SetActive(false);
        }

        public void ClickedPlay()
        {
            clickSound.Play();
            _playPanel.SetActive(false);
            _levelPanel.SetActive(true);
        }

        public void ClickedBackToStage()
        {
            clickSound.Play();
            _playPanel.SetActive(true);
            _levelPanel.SetActive(false);
        }
    } 
}

