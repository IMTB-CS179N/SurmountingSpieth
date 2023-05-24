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
        private Camera m_camera;

        public static Main Instance => Main.ms_instance == null ? (Main.ms_instance = Object.FindFirstObjectByType<Main>()) : Main.ms_instance;

        public GameObject Player;
        public GameObject Enemy1;
        public GameObject Enemy2;
        public GameObject Enemy3;
        public GameObject Enemy4;

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

        private void Awake()
        {
            this.m_camera = Camera.main;
        }

        private void Start()
        {
            EffectFactory.Initialize();
            TrinketFactory.Initialize();
        }

        private void Update()
        {
            if (InputProcessor.Instance.IsButtonPressed(UnityEngine.InputSystem.Key.P))
            {
                Debug.Log("Printing screen info...");

                if (this.Player != null)
                {
                    var point = ScreenManager.Instance.WorldPositionToUnitScreenPoint(this.Player.transform.position);

                    Debug.Log($"Player - unit screens point of Player is: {point}");
                    Debug.Log($"Player - the world position of Player is: {ScreenManager.Instance.UnitScreenPointToWorldPosition(point)}");
                }

                if (this.Enemy1 != null)
                {
                    var point = ScreenManager.Instance.WorldPositionToUnitScreenPoint(this.Enemy1.transform.position);

                    Debug.Log($"Enemy1 - unit screens point of Enemy1 is: {point}");
                    Debug.Log($"Enemy1 - the world position of Enemy1 is: {ScreenManager.Instance.UnitScreenPointToWorldPosition(point)}");
                }

                if (this.Enemy2 != null)
                {
                    var point = ScreenManager.Instance.WorldPositionToUnitScreenPoint(this.Enemy2.transform.position);

                    Debug.Log($"Enemy2 - unit screens point of Enemy2 is: {point}");
                    Debug.Log($"Enemy2 - the world position of Enemy2 is: {ScreenManager.Instance.UnitScreenPointToWorldPosition(point)}");
                }

                if (this.Enemy3 != null)
                {
                    var point = ScreenManager.Instance.WorldPositionToUnitScreenPoint(this.Enemy3.transform.position);

                    Debug.Log($"Enemy3 - unit screens point of Enemy3 is: {point}");
                    Debug.Log($"Enemy3 - the world position of Enemy3 is: {ScreenManager.Instance.UnitScreenPointToWorldPosition(point)}");
                }
            }
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
