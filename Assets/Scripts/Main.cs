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
        private bool m_pressed;
        private bool m_upsizing;

        public static Main Instance => Main.ms_instance == null ? (Main.ms_instance = Object.FindFirstObjectByType<Main>()) : Main.ms_instance;

        public float Speedster = 0.2f;

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

        private void Start()
        {
            EffectFactory.Initialize();
            TrinketFactory.Initialize();
        }

        private void Update()
        {
            if (this.Player != null)
            {
                var transform = this.Player.transform;

                var oldScale = transform.localScale;

                var position = transform.position;

                float newScale;

                if (this.m_upsizing)
                {
                    newScale = oldScale.y + this.Speedster * Time.deltaTime;

                    if (newScale >= 1.25f)
                    {
                        newScale = 1.25f;

                        this.m_upsizing = false;
                    }
                }
                else
                {
                    newScale = oldScale.y - this.Speedster * Time.deltaTime;

                    if (newScale <= 1.1f)
                    {
                        newScale = 1.1f;

                        this.m_upsizing = true;
                    }
                }

                transform.localScale = new Vector3(oldScale.x, newScale, oldScale.z);

                position.y += newScale - oldScale.y;

                transform.position = position;
            }

            if (InputProcessor.Instance.IsButtonPressed(UnityEngine.InputSystem.Key.P))
            {
                if (!this.m_pressed)
                {
                    this.m_pressed = true;

                    Debug.Log("Printing screen info...");

                    if (this.Player != null)
                    {
                        var point = ScreenManager.Instance.WorldPositionToUnitScreenPoint(this.Player.transform.position);

                        Debug.Log($"Player - unit screens point is: ({point.x:0.000000000000}, {point.y:0.000000000000})");
                        Debug.Log($"Player - the world position is: {ScreenManager.Instance.UnitScreenPointToWorldPosition(point)}");
                    }

                    if (this.Enemy1 != null)
                    {
                        var point = ScreenManager.Instance.WorldPositionToUnitScreenPoint(this.Enemy1.transform.position);

                        Debug.Log($"Enemy1 - unit screens point is: ({point.x:0.000000000000}, {point.y:0.000000000000})");
                        Debug.Log($"Enemy1 - the world position is: {ScreenManager.Instance.UnitScreenPointToWorldPosition(point)}");
                    }

                    if (this.Enemy2 != null)
                    {
                        var point = ScreenManager.Instance.WorldPositionToUnitScreenPoint(this.Enemy2.transform.position);

                        Debug.Log($"Enemy2 - unit screens point is: ({point.x:0.000000000000}, {point.y:0.000000000000})");
                        Debug.Log($"Enemy2 - the world position is: {ScreenManager.Instance.UnitScreenPointToWorldPosition(point)}");
                    }

                    if (this.Enemy3 != null)
                    {
                        var point = ScreenManager.Instance.WorldPositionToUnitScreenPoint(this.Enemy3.transform.position);

                        Debug.Log($"Enemy3 - unit screens point is: ({point.x:0.000000000000}, {point.y:0.000000000000})");
                        Debug.Log($"Enemy3 - the world position is: {ScreenManager.Instance.UnitScreenPointToWorldPosition(point)}");
                    }

                    if (this.Enemy4 != null)
                    {
                        var point = ScreenManager.Instance.WorldPositionToUnitScreenPoint(this.Enemy4.transform.position);

                        Debug.Log($"Enemy4 - unit screens point is: ({point.x:0.000000000000}, {point.y:0.000000000000})");
                        Debug.Log($"Enemy4 - the world position is: {ScreenManager.Instance.UnitScreenPointToWorldPosition(point)}");
                    }
                }
            }
            else
            {
                this.m_pressed = false;
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
