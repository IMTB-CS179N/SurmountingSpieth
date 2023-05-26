#define DEFAULT_ENEMIES

using Project.Game;
using Project.Input;
using Project.UI;

using System;
using System.Collections;

using UnityEngine;

namespace Project.Battle
{
    public class BattleManager : MonoBehaviour
    {
        public enum BattleState
        {
            None,
            StartBattle,
            PlayerMove,
            EnemyMove,
            FinishBattle,
        }

        private static readonly Vector2[][] ms_enemyPositions = new Vector2[][]
        {
            new Vector2[0]
            {
            },
            new Vector2[1]
            {
                new Vector2(+0.6600f, +0.1600f),
            },
            new Vector2[2]
            {
                new Vector2(+0.3000f, +0.5000f),
                new Vector2(+0.7125f, +0.0000f),
            },
            new Vector2[3]
            {
                new Vector2(+0.6450f, +0.5400f),
                new Vector2(+0.7200f, +0.0000f),
                new Vector2(+0.2700f, +0.2600f),
            },
            new Vector2[4]
            {
                new Vector2(+0.3750f, +0.5400f),
                new Vector2(+0.7800f, +0.5400f),
                new Vector2(+0.7800f, +0.0000f),
                new Vector2(+0.3750f, +0.0000f),
            },
        };

        private static readonly Vector2 ms_playerPosition = new(-0.6600f, +0.1600f);

        private static BattleManager ms_instance;

        private BattleBehavior[] m_enemyBehaviors;
        private BattleBehavior m_playerBehavior;

        private Enemy[] m_enemyEntities;
        private Player m_playerEntity;

        private BattleState m_state;
        private int m_abilityIndex; // <= -2 is default, -1 if regular attack, >= 0 is ability index

        private Coroutine m_currentRoutine;
        private bool m_forceExit;

        private bool m_isKeyPressed;

        [SerializeField]
        private BattleBuilder BattleUI;

        [SerializeField]
        private GameObject EntityPrefab;

        public static BattleManager Instance => ms_instance == null ? (ms_instance = FindFirstObjectByType<BattleManager>()) : ms_instance;

        private void Update()
        {
            if (InputProcessor.Instance.IsButtonPressed(UnityEngine.InputSystem.Key.D))
            {
                if (!this.m_isKeyPressed)
                {
                    this.m_isKeyPressed = true;

                    this.m_playerEntity.ApplyDamage(5);

                    this.BattleUI.UpdateInterface();
                }
            }
            else
            {
                this.m_isKeyPressed = false;
            }
        }

        public void StartBattle()
        {
            if (this.m_state != BattleState.None)
            {
                throw new Exception("We are already in battle!");
            }

            if (this.BattleUI == null)
            {
                throw new Exception("Cannot start battle because Battle UI is null!");
            }

            this.m_forceExit = false;

            this.SetupCallbacks(true);

            UIManager.Instance.PerformScreenChange(UIManager.ScreenType.Battle);

            this.InitializePlayer();
            this.InitializeEnemies();

            this.m_currentRoutine = this.StartCoroutine(this.PerformBattleStart());
        }

        public void FinishBattle()
        {
            this.SetupCallbacks(false);
        }

        private void InitializePlayer()
        {
            this.m_playerEntity = Player.Instance;

            var behavior = GameObject.Instantiate(this.EntityPrefab).GetComponent<BattleBehavior>();

            behavior.Entity = this.m_playerEntity;
            behavior.UnitPosition = ms_playerPosition;
            behavior.DefaultScale = 1.25f;
            behavior.MaximumScale = 1.25f;
            behavior.MinimumScale = 1.10f;
            behavior.AnimationSpeed = 0.2f;
            behavior.Index = 0;

            behavior.Initialize("Player Object"); // player sprites are slightly bigger

            this.m_playerBehavior = behavior;

            this.m_playerEntity.InitBattle();
        }

        private void InitializeEnemies()
        {
#if DEFAULT_ENEMIES
            this.m_enemyEntities = new Enemy[UnityEngine.Random.Range(1, 5)];

            for (int i = 0; i < this.m_enemyEntities.Length; ++i)
            {
                this.m_enemyEntities[i] = Enemy.CreateDefaultEnemy();
            }
            
            this.m_enemyBehaviors = new BattleBehavior[this.m_enemyEntities.Length];
#else
            this.m_enemyEntities = Array.Empty<Enemy>(); // #TODO from world map
            this.m_enemyBehaviors = Array.Empty<BattleBehavior>(); // #TODO from world map
#endif
            var enemyPositions = ms_enemyPositions[this.m_enemyEntities.Length];

            for (int i = 0; i < this.m_enemyBehaviors.Length; ++i)
            {
                var behavior = GameObject.Instantiate(this.EntityPrefab).GetComponent<BattleBehavior>();

                behavior.Entity = this.m_enemyEntities[i];
                behavior.UnitPosition = enemyPositions[i];
                behavior.DefaultScale = 1.0f;
                behavior.MaximumScale = 1.0f;
                behavior.MinimumScale = 0.9f;
                behavior.AnimationSpeed = 0.12f;
                behavior.Index = i;

                behavior.Initialize("Enemy Object " + i.ToString());

                this.m_enemyBehaviors[i] = behavior;
                this.m_enemyEntities[i].InitBattle();
            }
        }

        private IEnumerator PerformBattleStart()
        {
            this.m_state = BattleState.StartBattle;

            yield return null;

            // here we perform animations where entities move from outside of screen to their corresponding positions



            this.m_playerBehavior.PlayAnimation(BattleBehavior.AnimationType.Idle);

            for (int i = 0; i < this.m_enemyBehaviors.Length; ++i)
            {
                this.m_enemyBehaviors[i].PlayAnimation(BattleBehavior.AnimationType.Idle);
            }

            this.m_currentRoutine = this.StartCoroutine(this.PerformPlayerMove());
        }

        private IEnumerator PerformBattleFinal(bool forced)
        {
            this.m_state = BattleState.FinishBattle;

            yield return null;

            Debug.Log($"Finished the battle; forced stop was = {forced}");
        }

        private IEnumerator PerformPlayerMove()
        {
            this.m_state = BattleState.PlayerMove;

            Debug.Log("Performing player move...");

            this.m_abilityIndex = -2;

            yield return null;

            // #TODO prepare for the current move

            this.BattleUI.UnlockActions();

            for (int i = 0; i < this.m_enemyEntities.Length; ++i)
            {
                this.m_enemyEntities[i].InitTurn();
            }

            // #TODO more ???

            while (true)
            {
                if (this.m_forceExit)
                {
                    this.m_currentRoutine = this.StartCoroutine(this.PerformBattleFinal(true));

                    yield break;
                }

                if (this.m_abilityIndex <= -2)
                {
                    if (InputProcessor.Instance.RaycastLeftClickIfHappened(out var collider))
                    {
                        if (collider != null && collider.gameObject.TryGetComponent<BattleBehavior>(out var behavior))
                        {
                            this.BattleUI.CurrentEntity = behavior.Entity;
                        }
                        else
                        {
                            this.BattleUI.CurrentEntity = null;
                        }
                    }
                }
                else
                {
                    var performAttack = false;
                    int attackedIndex = -1;

                    if (this.m_abilityIndex >= 0 && !this.m_playerEntity.Abilities[this.m_abilityIndex].DoesDamage)
                    {
                        performAttack = true;
                    }
                    else
                    {
                        if (InputProcessor.Instance.RaycastLeftClickIfHappened(out var collider))
                        {
                            if (collider != null && collider.gameObject.TryGetComponent<BattleBehavior>(out var behavior) && !behavior.Entity.IsPlayer)
                            {
                                attackedIndex = behavior.Index;

                                performAttack = true;
                            }
                        }
                    }

                    if (performAttack)
                    {
                        var outcome = BattleUtility.Attack(this.m_playerEntity, this.m_enemyEntities, attackedIndex, this.m_abilityIndex);

                        this.TryAddAllyTextInformation(this.m_playerBehavior, outcome.Cleansed, outcome.HealReceived);

                        for (int i = 0; i < outcome.EnemyInfos.Length; ++i)
                        {
                            var info = outcome.EnemyInfos[i];

                            if (info.Engaged)
                            {
                                this.TryAddEnemyTextInformation(this.m_enemyBehaviors[i], info.Dispel, info.Missed, info.DamageDealt);
                            }

                            if (info.Killed)
                            {
                                Debug.Log($"Enemy {i} killed!");
                            }
                        }

                        // #TODO UPDATE HEALTH BARS HERE

                        this.BattleUI.ConfirmActionFinished();

                        this.BattleUI.UpdateInterface();

                        this.BattleUI.LockActions();

                        break;
                    }
                }

                yield return null;
            }

            this.m_abilityIndex = -2;

            yield return null;

            while (true)
            {
                if (this.m_forceExit)
                {
                    this.m_currentRoutine = this.StartCoroutine(this.PerformBattleFinal(true));

                    yield break;
                }

                if (!this.BattleUI.HasAnyAnimationsPlaying) // #TODO also any kill animations
                {
                    break;
                }

                yield return null;
            }

            yield return new WaitForSeconds(1.0f);

            bool isSomeoneAlive = false;

            for (int i = 0; i < this.m_enemyEntities.Length; ++i)
            {
                if (this.m_enemyEntities[i].IsAlive)
                {
                    isSomeoneAlive = true;

                    break;
                }
            }

            if (!isSomeoneAlive)
            {
                this.m_currentRoutine = this.StartCoroutine(this.PerformBattleFinal(false));

                yield break;
            }

            if (this.m_forceExit)
            {
                this.m_currentRoutine = this.StartCoroutine(this.PerformBattleFinal(true));

                yield break;
            }

            this.m_playerEntity.Cooldown(); // #TODO any health ++ display as a result of effect affections

            this.BattleUI.UpdateInterface();

            // #TODO some other stuff

            this.m_currentRoutine = this.StartCoroutine(this.PerformEnemyMove());
        }

        private IEnumerator PerformEnemyMove()
        {
            this.m_state = BattleState.EnemyMove;

            //this.m_playerEntity.InitTurn();
            //
            //var enemies = this.m_enemyEntities;
            //
            //for (int i = 0; i < enemies.Length; ++i)
            //{
            //    var enemy = enemies[i];
            //
            //    if (!enemy.TurnStats.BlockCurrentMove)
            //    {
            //
            //    }
            //
            //    enemy.InitTurn();
            //}
            //
            //
            //

            Debug.Log("Performing enemy move...");

            yield return new WaitForSeconds(1.0f);

            this.m_playerEntity.ApplyDamage(5);

            this.BattleUI.UpdateInterface(); 

            this.TryAddEnemyTextInformation(this.m_playerBehavior, false, false, 5);

            yield return new WaitForSeconds(1.0f);

            this.m_currentRoutine = this.StartCoroutine(this.PerformPlayerMove());
        }
        
        

        private void SetupCallbacks(bool attach)
        {
            if (attach)
            {
                this.BattleUI.OnUseAttackRequest += OnUseAttackRequest;

                this.BattleUI.OnUseAbilityRequest += OnUseAbilityRequest;

                this.BattleUI.OnUsePotionRequest += OnUsePotionRequest;

                this.BattleUI.OnCancelInteractRequest += OnCancelInteractRequest;

                this.BattleUI.OnExitRequest += OnExitRequest;
            }
            else
            {
                this.BattleUI.OnUseAttackRequest -= OnUseAttackRequest;

                this.BattleUI.OnUseAbilityRequest -= OnUseAbilityRequest;

                this.BattleUI.OnUsePotionRequest -= OnUsePotionRequest;

                this.BattleUI.OnCancelInteractRequest -= OnCancelInteractRequest;

                this.BattleUI.OnExitRequest -= OnExitRequest;
            }

            void OnUseAttackRequest()
            {
                this.m_abilityIndex = -1;
            }

            void OnUseAbilityRequest(int index)
            {
                this.m_abilityIndex = index;
            }

            void OnUsePotionRequest(int index)
            {
                var player = this.m_playerEntity;

                if (player is not null)
                {
                    int currentHP = player.EntityStats.CurHealth;
                    int currentMP = player.EntityStats.CurMana;
                    int effectNum = player.Effects.Count;

                    player.UsePotion(index);

                    if (currentHP < player.EntityStats.CurHealth)
                    {
                        // #TODO show health plus animation
                    }

                    if (currentMP < player.EntityStats.CurMana)
                    {
                        // #TODO show mana plus animation
                    }

                    if (effectNum < player.Effects.Count)
                    {
                        // #TODO add to effect list?
                    }

                    this.BattleUI.ConfirmActionFinished();
                    this.BattleUI.UpdateInterface();
                }
            }

            void OnCancelInteractRequest()
            {
                this.m_abilityIndex = -2;
            }

            void OnExitRequest()
            {
                this.m_forceExit = true;

                this.BattleUI.LockActions();

                this.BattleUI.CurrentEntity = null;
            }
        }

        private void TryAddAllyTextInformation(BattleBehavior behavior, bool cleansed, int healAmount)
        {
            float delay = 0.0f;

            if (cleansed)
            {
                this.BattleUI.AddAnimatedText("CLEANSED", 2.0f, delay, 30, 1, new Color32(0, 255, 255, 255), behavior.UnitPosition, behavior.TopMostPoint);

                delay += 1.0f;
            }

            if (healAmount > 0)
            {
                this.BattleUI.AddAnimatedText("+" + healAmount.ToString(), 2.0f, delay, 60, 2, new Color32(0, 200, 0, 255), behavior.UnitPosition, behavior.TopMostPoint);
            }
        }

        private void TryAddEnemyTextInformation(BattleBehavior behavior, bool dispel, bool missed, int damageDealt)
        {
            float delay = 0.0f;

            if (dispel)
            {
                this.BattleUI.AddAnimatedText("DISPELLED", 2.0f, delay, 30, 1, new Color32(255, 0, 255, 255), behavior.UnitPosition, behavior.TopMostPoint);

                delay += 1.0f;
            }

            if (missed)
            {
                this.BattleUI.AddAnimatedText("MISSED", 2.0f, delay, 30, 1, new Color(215, 215, 215, 255), behavior.UnitPosition, behavior.TopMostPoint);
            }
            else
            {
                this.BattleUI.AddAnimatedText("-" + damageDealt.ToString(), 2.0f, delay, 60, 2, new Color(200, 0, 0, 255), behavior.UnitPosition, behavior.TopMostPoint);
            }
        }
    }
}
