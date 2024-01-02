﻿using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.Aim
{
    public class AimMover : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;

        private AimTrackpad _trackpad;

        [Inject]
        private void Constructor(AimTrackpad trackpad)
        {
            _trackpad = trackpad;
        }

        private IDisposable _subscription;

        #region MonoBehaviour

        private void OnValidate() => _rectTransform ??= GetComponent<RectTransform>();

        private void OnEnable() => StartObserving();

        private void OnDisable() => StopObserving();

        #endregion

        private void StartObserving() => _trackpad.Position.Subscribe(SetAimPosition);

        private void StopObserving() => _subscription?.Dispose();

        private void SetAimPosition(Vector2 position) => _rectTransform.anchoredPosition = position;
    }
}