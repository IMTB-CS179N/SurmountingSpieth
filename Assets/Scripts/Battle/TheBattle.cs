using System.Collections;
using System.Collections.Generic;
using Project.Input;
using UnityEngine;

namespace Project.Battle
{
    public class TheBattle : MonoBehaviour
    {
        public enum StateType
        {
            Start,
            Player,
            Enemy
        }

        private static TheBattle ms_instance;

        public static TheBattle Instance => ms_instance == null ? (ms_instance = FindFirstObjectByType<TheBattle>()) : ms_instance;

        private void Start()
        {
        }

        private void Update()
        {
        }

        private bool PlayerFinished()
        {
            //if UI button got clicked if animation finished
            return InputProcessor.Instance.IsButtonPressed(UnityEngine.InputSystem.Key.C);
        }

        private bool EnemyFinished()
        {
            return InputProcessor.Instance.IsButtonPressed(UnityEngine.InputSystem.Key.V);
        }
    }
}
