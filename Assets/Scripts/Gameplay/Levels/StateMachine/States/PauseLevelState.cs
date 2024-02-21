﻿using System.Collections.Generic;
using Gameplay.Aim;
using Gameplay.Entities.Collector;
using Gameplay.Entities.Platoon;
using Gameplay.Entities.Player;
using Gameplay.Entities.Zombie;
using Gameplay.Entities.Zombie.StateMachine.States;
using Gameplay.Levels.StateMachine.States.Core;
using Gameplay.Weapons;
using Infrastructure.StateMachine.Main.States.Core;
using UI.Gameplay.Windows;

namespace Gameplay.Levels.StateMachine.States
{
    public class PauseLevelState : ILevelState, IState
    {
        private readonly List<Zombie> _zombies;
        private readonly PlayerHolder _playerHolder;
        private readonly Trackpad _trackpad;
        private readonly WeaponAim _weaponAim;
        private readonly WeaponAimer _weaponAimer;
        private readonly HUD _hud;
        private readonly Platoon _platoon;
        private readonly List<Collector> _collectors;
        private readonly List<ZombieSpawner.ZombieSpawner> _zombieSpawners;

        public PauseLevelState(List<Zombie> zombies, PlayerHolder playerHolder, Trackpad trackpad, WeaponAim weaponAim,
            WeaponAimer weaponAimer, HUD hud, Platoon platoon, List<Collector> collectors,
            List<ZombieSpawner.ZombieSpawner> zombieSpawners)
        {
            _zombies = zombies;
            _playerHolder = playerHolder;
            _trackpad = trackpad;
            _weaponAim = weaponAim;
            _weaponAimer = weaponAimer;
            _hud = hud;
            _platoon = platoon;
            _collectors = collectors;
            _zombieSpawners = zombieSpawners;
        }

        public void Enter()
        {
            _zombieSpawners.ForEach(spawner => spawner.Disable());
            _zombies.ForEach(zombie => zombie.StateMachine.Enter<IdleState>());

            if (_playerHolder.Instance != null)
                _playerHolder.Instance.StateMachine.Enter<Entities.Player.StateMachine.States.IdleState>();

            _trackpad.enabled = false;
            _weaponAim.Hide();
            _weaponAimer.Enabled = false;
            _hud.Hide();

            _collectors.ForEach(collector =>
                collector.StateMachine.Enter<Entities.Collector.StateMachine.States.IdleState>());

            _platoon.Soldiers.ForEach(soldier =>
                soldier.StateMachine.Enter<Entities.Soldier.StateMachine.States.IdleState>());
        }
    }
}