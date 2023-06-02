using Project.Game;
using Project.UI;
using Project.Overworld;

using System.Collections;

using UnityEngine;

namespace Project.Battle
{
    public class MapManager : MonoBehaviour
    {
        private static MapManager ms_instance;

        // private GameObject m_overworld;

        public static MapManager Instance =>
            ms_instance == null ? (ms_instance = FindFirstObjectByType<MapManager>()) : ms_instance;

        [SerializeField]
        public InGameBuilder InGameUI;

        [SerializeField]
        private GameObject OverworldPrefab;

        public enum Difficulty
        {
            Easy,
            Medium,
            Hard
        }

        public Difficulty difficulty { get; set; }

        public int levelIndex = 0;

        private void Awake()
        {
            if (this.InGameUI == null)
            {
                Debug.Log("Warning: InGameUI attached is null");
            }

            if (this.OverworldPrefab == null)
            {
                Debug.Log("Warning: Overworld prefab is null");
            }
        }

        public OverworldManager CreateOverworld() {
            return GameObject.Instantiate(this.OverworldPrefab).GetComponent<OverworldManager>();
        }

        public void LoadInGame()
        {
            UIManager.Instance.TransitionWithDelay(
                () =>
                {
                    var overworld = OverworldManager.Instance;

                    overworld.gameObject.SetActive(true);

                UIManager.Instance.PerformScreenChange(UIManager.ScreenType.InGame);
            }, null, 2.0f);
        }

        public void ReturnToMain()
        {
            UIManager.Instance.TransitionWithDelay(
                () =>
                {
                    OverworldManager.Instance.gameObject.SetActive(false);

                    UIManager.Instance.PerformScreenChange(UIManager.ScreenType.Main);
                },
                null,
                2.0f
            );
        }

        public void StartBattle()
        {
            this.StartCoroutine(this.StartBattleInternal());
        }

        public void FinishBattle()
        {
            this.StartCoroutine(this.FinishBattleInternal());
        }

        public void UpdateAction(InGameBuilder.ActionType action)
        {
            this.InGameUI.UpdateAction(action);
        }

        private IEnumerator StartBattleInternal()
        {
            bool done = false;

            UIManager.Instance.BeginTransitioning(() => done = true);

            while (!done)
            {
                yield return null;
            }

            // #TODO get enemy information based on map cell here

            var enemies = new Enemy[Random.Range(3, 5)];

            for (int i = 0; i < enemies.Length; ++i)
            {
                enemies[i] = Enemy.CreateDefaultEnemy();
            }

            yield return null;

            OverworldManager.Instance.gameObject.SetActive(false);

            yield return null;

            UIManager.Instance.PerformScreenChange(UIManager.ScreenType.Battle);

            yield return null;

            BattleManager.Instance.StartBattle(Player.Instance, enemies, () =>
            {
                this.StartCoroutine(this.EndTransitionsAfterDelay(2.0f));
            }, this.FinishBattle);
        }

        private IEnumerator FinishBattleInternal()
        {
            bool done = false;

            UIManager.Instance.BeginTransitioning(() => done = true);

            while (!done)
            {
                yield return null;
            }

            var outcome = BattleManager.Instance.Outcome;

            BattleManager.Instance.FinishBattle();

            yield return null;

            var overworld = OverworldManager.Instance;

            overworld.gameObject.SetActive(true);

            yield return null;

            UIManager.Instance.PerformScreenChange(UIManager.ScreenType.InGame);

            yield return null;

            switch (outcome)
            {
                case BattleManager.BattleOutcome.Exit:
                    MapManager.Instance.UpdateAction(UI.InGameBuilder.ActionType.Battle);
                    break;

                case BattleManager.BattleOutcome.Victory:
                    MapManager.Instance.UpdateAction(UI.InGameBuilder.ActionType.None);
                    Debug.Log("calling");
                    OverworldManager.Instance.GenerateShop();
                    // m_overworld.GetComponent<OverworldManager>().GenerateShop();
                    break;

                default:
                    break;
            }

            yield return this.StartCoroutine(this.EndTransitionsAfterDelay(2.0f));
        }

        private IEnumerator EndTransitionsAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            UIManager.Instance.EndTransitioning(null);
        }
    }
}
