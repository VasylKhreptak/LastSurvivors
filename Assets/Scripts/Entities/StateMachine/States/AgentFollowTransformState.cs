﻿using System;
using Infrastructure.StateMachine.Main.States.Core;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace Entities.StateMachine.States
{
    public class AgentFollowTransformState : IPayloadedState<Transform>, IExitable
    {
        private readonly NavMeshAgent _agent;
        private readonly Preferences _preferences;

        public AgentFollowTransformState(NavMeshAgent agent, Preferences preferences)
        {
            _agent = agent;
            _preferences = preferences;
        }

        private IDisposable _destinationUpdateSubscription;

        private Transform _target;

        private Vector3 _targetPosition;
        private Vector3 _lastDestinationPosition;

        public void Enter(Transform target)
        {
            _target = target;
            _lastDestinationPosition = _target.position;

            _agent.isStopped = false;
            _agent.SetDestination(_lastDestinationPosition);
            StartUpdatingDestination();
        }

        public void Exit()
        {
            if (_agent.isActiveAndEnabled)
                _agent.isStopped = true;

            StopUpdatingDestination();
        }

        private void StartUpdatingDestination()
        {
            _destinationUpdateSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_preferences.UpdateInterval))
                .Subscribe(_ => TryUpdateDestination());
        }

        private void StopUpdatingDestination() => _destinationUpdateSubscription?.Dispose();

        private void TryUpdateDestination()
        {
            _targetPosition = _target.position;

            if (Vector3.Distance(_targetPosition, _lastDestinationPosition) > _preferences.PositionThreshold)
            {
                _agent.SetDestination(_targetPosition);
                _lastDestinationPosition = _targetPosition;
            }
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private float _positionThreshold = 0.1f;
            [SerializeField] private float _updateInterval = 0.1f;

            public float PositionThreshold => _positionThreshold;
            public float UpdateInterval => _updateInterval;
        }
    }
}