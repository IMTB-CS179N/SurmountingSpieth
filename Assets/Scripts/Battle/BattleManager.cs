using System.Collections;
using System.Collections.Generic;

using Project.Game;
using Project.Input;
using Project.UI;

using UnityEngine;

namespace Project.Battle
{
    public class BattleManager : MonoBehaviour
    {
        public enum StateType
        {
            Start,
            Player,
            Enemy
        }

        private static BattleManager ms_instance;

        private bool m_dPressed;

        public static BattleManager Instance => ms_instance == null ? (ms_instance = FindFirstObjectByType<BattleManager>()) : ms_instance;

        public BattleBuilder BattleUI;

        private void Start()
        {
        }

        private void Update()
        {
            if (InputProcessor.Instance.IsButtonPressed(UnityEngine.InputSystem.Key.P))
            {
                this.BattleUI.CurrentEntity = Player.Instance;
            }

            if (InputProcessor.Instance.IsButtonPressed(UnityEngine.InputSystem.Key.E))
            {

            }

            if (InputProcessor.Instance.IsButtonPressed(UnityEngine.InputSystem.Key.N))
            {
                this.BattleUI.CurrentEntity = null;
            }

            if (InputProcessor.Instance.IsButtonPressed(UnityEngine.InputSystem.Key.D))
            {
                if (!this.m_dPressed)
                {
                    this.m_dPressed = true;

                    Player.Instance.ApplyDamage(20);

                    this.BattleUI.UpdateInterface();
                }
            }
            else
            {
                this.m_dPressed = false;
            }
        }

        public void StartBattle()
        {
            UIManager.Instance.PerformScreenChange(UIManager.ScreenType.Battle);

            Player.Instance.Init();
        }
    }
}
