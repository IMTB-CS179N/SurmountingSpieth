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

        public static MapManager Instance => ms_instance == null ? (ms_instance = FindFirstObjectByType<MapManager>()) : ms_instance;

        private void Start()
        {
        }

        private void Update()
        {
        }

        private void FixedUpdate()
        {
        }

        public void Load()
        {

        }
    }
}
