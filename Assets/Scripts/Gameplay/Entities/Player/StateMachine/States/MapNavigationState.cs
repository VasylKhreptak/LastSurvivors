﻿using System;
using Entities.AI;
using Gameplay.Entities.Player.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Levels.StateMachine.States;
using Levels.StateMachine.States.Core;
using UniRx;
using Utilities.PhysicsUtilities.Trigger;

namespace Gameplay.Entities.Player.StateMachine.States
{
    public class MapNavigationState : IPlayerState, IState, IExitable
    {
        private readonly IStateMachine<ILevelState> _levelStateMachine;
        private readonly AgentWaypointsFollower _waypointsFollower;
        private readonly MeleeAttacker _meleeAttacker;
        private readonly ClosestTriggerObserver<LootBox.LootBox> _lootBoxObserver;

        public MapNavigationState(IStateMachine<ILevelState> levelStateMachine, AgentWaypointsFollower waypointsFollower,
            MeleeAttacker meleeAttacker, ClosestTriggerObserver<LootBox.LootBox> lootBoxObserver)
        {
            _levelStateMachine = levelStateMachine;
            _waypointsFollower = waypointsFollower;
            _meleeAttacker = meleeAttacker;
            _lootBoxObserver = lootBoxObserver;
        }

        private IDisposable _lootBoxObserverSubscription;

        public void Enter() => StartObserving();

        public void Exit()
        {
            StopObserving();
            _meleeAttacker.Stop();
            _waypointsFollower.Stop();
        }

        private void StartObserving()
        {
            StopObserving();
            _lootBoxObserverSubscription = _lootBoxObserver.Trigger.Select(x => x?.Value).Subscribe(OnClosestLootBoxChanged);
        }

        private void StopObserving() => _lootBoxObserverSubscription?.Dispose();

        private void OnClosestLootBoxChanged(LootBox.LootBox lootBox)
        {
            if (lootBox == null)
            {
                _meleeAttacker.Stop();
                _waypointsFollower.Start(() => _levelStateMachine.Enter<LevelCompletedState>());
                return;
            }

            _waypointsFollower.Stop();
            _meleeAttacker.Start(lootBox.transform, lootBox.Health, lootBox);
        }
    }
}