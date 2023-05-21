using Project.Game;
using Project.Input;

using UnityEngine;

namespace Project
{
    public class Main : MonoBehaviour
    {
        private static Main ms_instance;

        private bool m_showAllTrade;
        private bool m_enableMusic;

        public static Main Instance => Main.ms_instance == null ? (Main.ms_instance = Object.FindFirstObjectByType<Main>()) : Main.ms_instance;

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
            PotionFactory.Initialize();
            TrinketFactory.Initialize();
        }

        private void Update()
        {
        }

        private void FixedUpdate()
        {
            var collider = InputProcessor.Instance.RaycastLeftSingular();

            if (collider)
            {
                var clickObj = collider.transform.gameObject.GetComponent<Click>();

                if (clickObj)
                {
                    clickObj.TriggerClick();
                }
            }
        }

        private void SetMusicStatus(bool enable)
        {
            if (enable != this.m_enableMusic)
            {
                this.m_enableMusic = enable;

                // #TODO enable / disable
            }
        }
    }
}
