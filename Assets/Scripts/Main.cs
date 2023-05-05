using Project.Input;

using UnityEngine;

namespace Project
{
    public class Main : MonoBehaviour
    {
        private static Main ms_instance;

        public static Main Instance =>
            Main.ms_instance == null
                ? (Main.ms_instance = Object.FindFirstObjectByType<Main>())
                : Main.ms_instance;

        private void Start()
        {
            var array = AssetParser.ParseFromCSV<Ability>("/Users/susan/Desktop/CS179N/Project/Resources/Character Abilities - Sheet1.csv.bytes");
        
            Debug.Log(array.ToString());
        }

        private void Update() { }

        private void FixedUpdate()
        {
            var collider = InputProcessor.Instance.RaycastLeftSingular();
            if (collider)
            {
                var clickObj = collider.transform.gameObject.GetComponent<OnClick>();
                if (clickObj)
                {
                    clickObj.Click();
                }
            }
        }
    }
}
