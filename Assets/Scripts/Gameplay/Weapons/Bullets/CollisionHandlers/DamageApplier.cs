﻿using Gameplay.Weapons.Bullets.Core;
using UnityEngine;
using Visitor;

namespace Gameplay.Weapons.Bullets.CollisionHandlers
{
    public class DamageApplier
    {
        private readonly IBullet _bullet;

        public DamageApplier(IBullet bullet)
        {
            _bullet = bullet;
        }

        public void TryApply(GameObject gameObject)
        {
            if (gameObject.TryGetComponent(out IVisitable<IBullet> visitable))
                visitable.Accept(_bullet);
        }
    }
}