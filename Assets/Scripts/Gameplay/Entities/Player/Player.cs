﻿using System.Collections.Generic;
using Gameplay.Entities.Health.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Gameplay.Entities.Player
{
    public class Player : MonoBehaviour
    {
        private IHealth _health;

        [Inject]
        private void Constructor(IHealth health, PlayerWaypointFollower waypointFollower)
        {
            _health = health;
            WaypointFollower = waypointFollower;
        }

        public PlayerWaypointFollower WaypointFollower { get; private set; }

        [Button]
        private void Kill() => _health.Kill();
    }
}