﻿using System;
using Infrastructure.Services.Screen.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.Screen
{
    public class ScreenService : IScreenService, IInitializable, IDisposable
    {
        private const float _updateInterval = 1 / 10f;

        private readonly ReactiveProperty<ScreenOrientation> _screenOrientation = new ReactiveProperty<ScreenOrientation>();
        private readonly ReactiveProperty<Vector2Int> _screenResolution = new ReactiveProperty<Vector2Int>();

        private IDisposable _intervalSubscription;

        public IReadOnlyReactiveProperty<ScreenOrientation> ScreenOrientation => _screenOrientation;
        public IReadOnlyReactiveProperty<Vector2Int> ScreenResolution => _screenResolution;

        public void Initialize()
        {
            StartObserving();
        }

        public void Dispose()
        {
            StopObserving();
        }

        private void StartObserving()
        {
            StopObserving();

            _intervalSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_updateInterval))
                .DoOnSubscribe(Observe)
                .Subscribe(_ => Observe());
        }

        private void StopObserving()
        {
            _intervalSubscription?.Dispose();
            _intervalSubscription = null;
        }

        private void Observe()
        {
            _screenOrientation.Value = UnityEngine.Screen.orientation;
            _screenResolution.Value = new Vector2Int(UnityEngine.Screen.width, UnityEngine.Screen.height);
        }
    }
}