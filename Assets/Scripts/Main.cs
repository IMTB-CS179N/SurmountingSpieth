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

                var resolution = new Vector2(Screen.width, Screen.height);

                Debug.Log($"Resolution: {resolution.x} x {resolution.y}");

                var worldLeftTop = this.m_camera.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f));
                var worldRightBottom = this.m_camera.ScreenToWorldPoint(new Vector3(resolution.x, resolution.y, 0.0f));

                Debug.Log($"World Dimensions along X-axis: {worldLeftTop.x} to {worldRightBottom.x}");
                Debug.Log($"World Dimensions along Y-axis: {worldRightBottom.y} to {worldLeftTop.y}");

                var mappedLeftTop = ScreenManager.Instance.MapPointBasedOnResolution(new Vector2(-0.5f, -0.5f), new Vector2(1.0f, 1.0f));
                var mappedRightBottom = ScreenManager.Instance.MapPointBasedOnResolution(new Vector2(0.5f, 0.5f), new Vector2(1.0f, 1.0f));

                Debug.Log($"Mapped Dimensions along X-axis: {mappedLeftTop.x} to {mappedRightBottom.x}");
                Debug.Log($"Mapped Dimensions along Y-axis: {mappedRightBottom.y} to {mappedLeftTop.y}");
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
