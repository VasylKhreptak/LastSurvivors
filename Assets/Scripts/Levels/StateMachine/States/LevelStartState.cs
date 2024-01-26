﻿using System.Collections.Generic;
using Gameplay.Entities.Player;
using Gameplay.Entities.Zombie;
using Infrastructure.StateMachine.Main.Core;
using Levels.StateMachine.States.Core;
using UnityEngine;

namespace Levels.StateMachine.States
{
    public class LevelStartState : ILevelState
    {
        private readonly IStateMachine<ILevelState> _levelStateMachine;
        private readonly PlayerHolder _playerHolder;
        private readonly List<Zombie> _zombies;

        public LevelStartState(IStateMachine<ILevelState> levelStateMachine, PlayerHolder playerHolder, List<Zombie> zombies)
        {
            _levelStateMachine = levelStateMachine;
            _playerHolder = playerHolder;
            _zombies = zombies;
        }

        public void Enter()
        {
            _zombies.ForEach(zombie => zombie.TargetFollower.Start());
            _playerHolder.Instance.WaypointFollower.Start();
            _levelStateMachine.Enter<LevelLoopState>();
        }
    }
}