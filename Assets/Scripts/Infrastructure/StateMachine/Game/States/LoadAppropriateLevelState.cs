﻿using System;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.StaticData.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class LoadAppropriateLevelState : IPayloadedState<Action>, IGameState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly IPersistentDataService _persistentDataService;

        public LoadAppropriateLevelState(IStateMachine<IGameState> stateMachine, IStaticDataService staticDataService,
            IPersistentDataService persistentDataService)
        {
            _stateMachine = stateMachine;
            _staticDataService = staticDataService;
            _persistentDataService = persistentDataService;
        }

        public void Enter(Action onComplete)
        {
            if (_persistentDataService.PersistentData.PlayerData.FinishedTutorial == false)
            {
                LoadSceneAsyncState.Payload payload = new LoadSceneAsyncState.Payload
                {
                    SceneName = _staticDataService.Config.TutorialScene,
                    OnComplete = onComplete
                };

                _stateMachine.Enter<LoadSceneAsyncState, LoadSceneAsyncState.Payload>(payload);
            }

            //load next level
        }
    }
}