﻿using System;
using System.Collections.Generic;
using Infrastructure.StateMachine.Main.States.Core;
using Infrastructure.StateMachine.Main.States.Factory;
using Zenject;

namespace Gameplay.Entities.Player.StateMachine.States.Core
{
    public class PlayerStateFactory : StateFactory
    {
        public PlayerStateFactory(DiContainer container) : base(container) { }

        protected override Dictionary<Type, Func<IBaseState>> BuildStatesRegister() =>
            new Dictionary<Type, Func<IBaseState>>
            {
                [typeof(IdleState)] = _container.Resolve<IdleState>,
                [typeof(MoveState)] = _container.Resolve<MoveState>
            };
    }
}