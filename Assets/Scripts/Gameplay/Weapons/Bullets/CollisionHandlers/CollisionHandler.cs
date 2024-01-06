﻿using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapons.Bullets.CollisionHandlers
{
    public class CollisionHandler : IInitializable, IDisposable
    {
        private readonly GameObject _gameObject;
        private readonly Collider _collider;
        private readonly HitParticle _hitParticle;

        public CollisionHandler(GameObject gameObject, Collider collider, HitParticle hitParticle)
        {
            _gameObject = gameObject;
            _collider = collider;
            _hitParticle = hitParticle;
        }

        private IDisposable _subscription;

        public void Initialize() => _subscription = _collider.OnCollisionEnterAsObservable().Subscribe(OnCollisionEnter);

        public void Dispose() => _subscription.Dispose();

        private void OnCollisionEnter(Collision collision)
        {
            _hitParticle.Spawn(collision);
            _gameObject.SetActive(false);
        }
    }
}