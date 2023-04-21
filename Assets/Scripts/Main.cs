using Project.Input;

using UnityEngine;

namespace Project
{
    public class Main : MonoBehaviour
    {
        private static Main ms_instance;

        public static Main Instance => Main.ms_instance == null ? (Main.ms_instance = Object.FindFirstObjectByType<Main>()) : Main.ms_instance;

        private void Start()
        {
            Debug.Log("Hello Unity!");
        }

        private void Update()
        {
            // Debug.Log(InputProcessor.Instance.IsPointerOverUIObject());
        }

        private void FixedUpdate()
        {
            _ = InputProcessor.Instance.RaycastLeftSingular();
        }
    }
}
