using System.Collections;
using System.Collections.Generic;
using Project.Input;
using UnityEngine;

namespace Project
{
    public class Battle : MonoBehaviour
    {
        public enum StateType
        {
            Start,
            Player,
            Enemy
        }

        private static Battle ms_instance;

        public static Battle Instance =>
            ms_instance == null ? (ms_instance = FindFirstObjectByType<Battle>()) : ms_instance;

        public FinishCharacter CharacterBuild;
        private Player player = new Player();
        private List<Enemy> m_enemies = new List<Enemy>();

        private StateType CurrentTurn = StateType.Player;

        // Start is called before the first frame update
        void Start()
        {
            player.SetStats(CharacterBuild);
            m_enemies.Add(new Enemy());
        }

        // Update is called once per frame
        void Update()
        {
            switch (CurrentTurn)
            { //transitions
                case StateType.Player:
                    if (PlayerFinished())
                    {
                        this.PerformPlayerAttack();
                        Debug.Log("Switching to enemy turn");
                        CurrentTurn = StateType.Enemy;
                    }

                    break;
                case StateType.Enemy:
                    if (EnemyFinished())
                    {
                        Debug.Log("extra before health: " + player.Health);
                        this.PerformEnemyAttack();
                        Debug.Log("Switching to player turn");
                        CurrentTurn = StateType.Player;
                    }
                    break;
            }

            switch (CurrentTurn)
            { //actions
                case StateType.Player:

                    break;

                case StateType.Enemy:

                    break;
            }
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

        private void PerformPlayerAttack()
        {
            Debug.Log("Player Attacking");
            var enemy = this.m_enemies[0];

            if (BattleUtil.Attack(player, enemy))
            {
                this.m_enemies.RemoveAt(0);

                if (this.m_enemies.Count == 0)
                {
                    Debug.Log("Won battle");
                    // win
                }
                else
                {
                    // update ui and battlefield
                }
            }
        }

        private void PerformEnemyAttack()
        {
            Debug.Log("Enemy Attacking");
            var enemy = this.m_enemies[0];
            if (BattleUtil.Attack(enemy, player))
            {
                Debug.Log("Enemy killed player");
            }
            else
            {
                // update ui and battlefield
            }
        }

        public void ResetBattle()
        {
            CurrentTurn = StateType.Player;
            // reset monster list
        }
    }
}
