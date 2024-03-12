﻿using Firebase.Messaging;
using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class BootstrapMessagingState : IGameState, IState
    {
        private readonly ILogService _logService;
        private readonly IStateMachine<IGameState> _stateMachine;

        public BootstrapMessagingState(ILogService logService, IStateMachine<IGameState> stateMachine)
        {
            _logService = logService;
            _stateMachine = stateMachine;
        }

        private bool _initialized;

        public void Enter()
        {
            _logService.Log("BootstrapMessagingState");

            if (_initialized)
            {
                EnterNextState();
                return;
            }

            FirebaseMessaging.TokenReceived += OnTokenReceived;
            FirebaseMessaging.MessageReceived += OnMessageReceived;

            _initialized = true;

            EnterNextState();
        }

        private void EnterNextState() => _stateMachine.Enter<SetupAutomaticDataSaveState>();

        private void OnTokenReceived(object sender, TokenReceivedEventArgs token) =>
            _logService.Log("Received Registration Token: " + token.Token);

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e) =>
            _logService.Log("Received a new message from: " + e.Message.From);
    }
}