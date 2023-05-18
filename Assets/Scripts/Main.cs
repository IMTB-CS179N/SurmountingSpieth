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
    }
}
