﻿using Entities.StateMachine.States;
using Gameplay.Entities.Player.StateMachine.States.Core;
using UnityEngine.AI;

namespace Gameplay.Entities.Player.StateMachine.States
{
    public class MoveState : AgentMoveState, IPlayerState
    {
        public MoveState(NavMeshAgent agent, Preferences preferences)
            : base(agent, preferences) { }
    }
}