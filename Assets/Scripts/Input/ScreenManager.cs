using System;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Project.Input
{
    public class ScreenManager : MonoBehaviour
    {
        private static ScreenManager ms_instance;

        private float m_screenHeight;
        private float m_screenWidth;
        private Camera m_camera;

        public static ScreenManager Instance => ScreenManager.ms_instance == null ? (ScreenManager.ms_instance = Object.FindFirstObjectByType<ScreenManager>()) : ScreenManager.ms_instance;

        public float Width => this.m_screenWidth;

        public float Height => this.m_screenHeight;

        public event Action OnScreenResolutionChanged;

        private void Start()
        {
            this.m_camera = Camera.main;
            this.m_screenWidth = Screen.width;
            this.m_screenHeight = Screen.height;
        }

        private void Update()
        {
            float width = Screen.width;
            float height = Screen.height;

            if (this.m_screenWidth != width || this.m_screenHeight != height)
            {
                this.m_screenWidth = width;
                this.m_screenHeight = height;

                this.OnScreenResolutionChanged?.Invoke();
            }
        }

        public Vector2 ScreenToWorldPoint(Vector2 point)
        {
            return this.m_camera.ScreenToWorldPoint(point);
        }

        public Vector2 MapPointBasedOnResolution(Vector2 point, Vector2 resolution)
        {
            var ratio = this.m_screenHeight / resolution.y;

            return 2.0f * this.m_camera.orthographicSize * new Vector2(point.x * (this.m_screenWidth / resolution.x) / ratio, point.y);
        }

        public Vector2 WorldPositionToUnitScreenPoint(Vector2 point)
        {
            var ratio = this.m_screenHeight / this.m_screenWidth;

            var size = this.m_camera.orthographicSize * 2.0f;

            return new Vector2(point.x * ratio / size, point.y / size);
        }

        public Vector2 UnitScreenPointToWorldPosition(Vector2 point)
        {
            return this.MapPointBasedOnResolution(point, Vector2.one);
        }
    }
}
