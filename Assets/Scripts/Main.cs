using Project.Game;
using Project.Input;

using System;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Project
{
    public class Main : MonoBehaviour
    {
        private static Main ms_instance;

        private bool m_showAllTrade;
        private bool m_enableMusic;

        public static Main Instance =>
            Main.ms_instance == null
                ? (Main.ms_instance = Object.FindFirstObjectByType<Main>())
                : Main.ms_instance;

        public bool ShowAllTradeElements
        {
            get => this.m_showAllTrade;
            set => this.m_showAllTrade = value;
        }

        public bool EnableMusic
        {
            get => this.m_enableMusic;
            set => this.SetMusicStatus(value);
        }

        private void Start()
        {
            ScreenManager.Instance.SetCursorTexture(ResourceManager.DefaultCursor, Vector2.zero, CursorMode.Auto);

            EffectFactory.Initialize();
            TrinketFactory.Initialize();
        }

        private void Update()
        {
        }

        private void FixedUpdate()
        {
        }

        private void SetMusicStatus(bool enable)
        {
            if (enable != this.m_enableMusic)
            {
                this.m_enableMusic = enable;
                this.GetComponent<AudioSource>().enabled = enable;
                // #TODO enable / disable
            }
        }

        public bool CanInitializeFromSaveData()
        {
            return SaveSystem.HasData();
        }

        public void InitializeFromSaveData()
        {
            var value = PlayerPrefs.GetString("PlayerSave");

            // #TODO
        }

        public void SaveDataToPlayerPrefs()
        {
            string something = String.Empty;

            PlayerPrefs.SetString("PlayerSave", something);

            PlayerPrefs.Save();
        }
    }
}
