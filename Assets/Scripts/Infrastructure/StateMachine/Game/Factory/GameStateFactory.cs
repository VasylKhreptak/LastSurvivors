﻿using System;
using System.Collections.Generic;
using Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine.Main.States.Core;
using Infrastructure.StateMachine.Main.States.Factory;
using Zenject;

namespace Infrastructure.StateMachine.Game.Factory
{
    public class GameStateFactory : StateFactory
    {
        public GameStateFactory(DiContainer container) : base(container) { }

        protected override Dictionary<Type, Func<IBaseState>> BuildStatesRegister() =>
            new Dictionary<Type, Func<IBaseState>>
            {
                [typeof(BootstrapState)] = _container.Resolve<BootstrapState>,
                [typeof(LoginState)] = _container.Resolve<LoginState>,
                [typeof(SetupApplicationState)] = _container.Resolve<SetupApplicationState>,
                [typeof(LoadDataState)] = _container.Resolve<LoadDataState>,
                [typeof(SaveDataState)] = _container.Resolve<SaveDataState>,
                [typeof(FinalizeBootstrapState)] = _container.Resolve<FinalizeBootstrapState>,
                [typeof(LoadSceneAsyncState)] = _container.Resolve<LoadSceneAsyncState>,
                [typeof(LoadSceneWithTransitionAsyncState)] = _container.Resolve<LoadSceneWithTransitionAsyncState>,
                [typeof(GameLoopState)] = _container.Resolve<GameLoopState>,
                [typeof(PlayState)] = _container.Resolve<PlayState>,
                [typeof(LoadLevelState)] = _container.Resolve<LoadLevelState>
            };
    }
}