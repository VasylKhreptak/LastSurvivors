﻿using System.Collections.Generic;
using Gameplay.Aim;
using Gameplay.Entities.Player;
using Gameplay.Entities.Zombie;
using Gameplay.Weapons;
using Levels.StateMachine.States.Core;
using UI.Gameplay.Windows;

namespace Levels.StateMachine.States
{
    public class LevelCompletedState : ILevelState
    {
        private readonly List<Zombie> _zombies;
        private readonly PlayerHolder _playerHolder;
        private readonly Trackpad _trackpad;
        private readonly LevelCompletedWindow _levelCompletedWindow;
        private readonly WeaponAim _weaponAim;
        private readonly WeaponAimer _weaponAimer;
        private readonly HUD _hud;

        public LevelCompletedState(List<Zombie> zombies, PlayerHolder playerHolder, Trackpad trackpad,
            LevelCompletedWindow levelCompletedWindow, WeaponAim weaponAim, WeaponAimer weaponAimer, HUD hud)
        {
            _zombies = zombies;
            _playerHolder = playerHolder;
            _trackpad = trackpad;
            _levelCompletedWindow = levelCompletedWindow;
            _weaponAim = weaponAim;
            _weaponAimer = weaponAimer;
            _hud = hud;
        }

        public void Enter()
        {
            _zombies.ForEach(zombie => zombie.TargetFollower.Stop());

            if (_playerHolder.Instance != null)
                _playerHolder.Instance.WaypointFollower.Stop();

            _trackpad.enabled = false;
            _levelCompletedWindow.Show();
            _weaponAim.Hide();
            _weaponAimer.Enabled = false;
            _hud.Hide();
        }
    }
}