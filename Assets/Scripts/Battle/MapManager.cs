using Project.UI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Project.Battle
{
    public class MapManager : MonoBehaviour
    {
        private static MapManager ms_instance;

        private GameObject m_overworld;

        public static MapManager Instance => ms_instance == null ? (ms_instance = FindFirstObjectByType<MapManager>()) : ms_instance;

        [SerializeField]
        private InGameBuilder InGameUI;

        [SerializeField]
        private GameObject OverworldPrefab;

        private void Start()
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

        private void Update()
        {
        }

        private void FixedUpdate()
        {
        }

        public void Load()
        {
            this.m_overworld = GameObject.Instantiate(this.OverworldPrefab);
        }

        public void UpdateAction(InGameBuilder.ActionType action)
        {
            this.InGameUI.UpdateAction(action);
        }
    }
}
