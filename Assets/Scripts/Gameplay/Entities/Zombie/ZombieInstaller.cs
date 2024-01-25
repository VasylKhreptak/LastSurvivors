﻿using Adapters.Velocity;
using Entities.Animations;
using Entities.StateMachine.States;
using Gameplay.Entities.Health.Core;
using Gameplay.Entities.Zombie.DeathHandlers.Core;
using Gameplay.Entities.Zombie.StateMachine;
using Gameplay.Entities.Zombie.StateMachine.States;
using Gameplay.Entities.Zombie.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using UnityEngine;
using UnityEngine.AI;
using Utilities.GameObjectUtilities;
using Utilities.PhysicsUtilities;
using Utilities.TransformUtilities;
using Zenject;

namespace Gameplay.Entities.Zombie
{
    public class ZombieInstaller : MonoInstaller
    {
        [SerializeField] private float _maxHealth = 100f;
        [SerializeField] private MoveAnimation.Preferences _moveAnimationPreferences;
        [SerializeField] private GameObjectRandomizer.Preferences _skinRandomizerPreferences;
        [SerializeField] private RotationRandomizer.Preferences _rotationRandomizerPreferences;
        [SerializeField] private AgentMoveState.Preferences _moveStatePreferences;
        [SerializeField] private ZombieStateController.Preferences _stateControllerPreferences;
        [SerializeField] private ZombieAttacker.Preferences _zombieAttackPreferences;
        [SerializeField] private Ragdoll.Preferences _ragdollPreferences;

        public override void InstallBindings()
        {
            Container.BindInstance(gameObject).AsSingle();
            Container.Bind<Animator>().FromComponentOnRoot().AsSingle();
            Container.Bind<NavMeshAgent>().FromComponentOnRoot().AsSingle();
            Container.BindInterfacesTo<AdaptedAgentForVelocity>().AsSingle();
            Container.Bind<IHealth>().FromInstance(new Health.Health(_maxHealth)).AsSingle();

            RandomizeSkin();
            RandomizeRotation();
            BindMoveAnimation();
            BindRagdoll();
            BindStateMachine();
            BindStateController();
            BindDeathHandler();
            BindZombieAttacker();
            EnterIdleState();
        }

        private void BindMoveAnimation() =>
            Container.BindInterfacesTo<MoveAnimation>().AsSingle().WithArguments(_moveAnimationPreferences);

        private void BindDeathHandler()
        {
            Container.Bind<Collider>().FromComponentOnRoot().AsSingle().WhenInjectedInto<ZombieDeathHandler>();
            Container.BindInterfacesTo<ZombieDeathHandler>().AsSingle();
        }

        private void RandomizeSkin()
        {
            GameObjectRandomizer randomizer = new GameObjectRandomizer(_skinRandomizerPreferences);
            randomizer.Randomize();
        }

        private void RandomizeRotation()
        {
            RotationRandomizer randomizer = new RotationRandomizer(transform, _rotationRandomizerPreferences);
            randomizer.Randomize();
        }

        private void BindStateMachine()
        {
            BindStates();
            Container.Bind<ZombieStateFactory>().AsSingle();
            Container.BindInterfacesTo<ZombieStateMachine>().AsSingle();
        }

        private void BindStates()
        {
            Container.Bind<IdleState>().AsSingle();
            Container.Bind<MoveState>().AsSingle().WithArguments(_moveStatePreferences);
        }

        private void BindStateController() =>
            Container.BindInterfacesTo<ZombieStateController>().AsSingle().WithArguments(transform, _stateControllerPreferences);

        private void EnterIdleState() => Container.Resolve<IStateMachine<IZombieState>>().Enter<IdleState>();

        private void BindZombieAttacker() =>
            Container.BindInterfacesTo<ZombieAttacker>().AsSingle().WithArguments(_zombieAttackPreferences);

        private void BindRagdoll()
        {
            Container.Bind<Ragdoll>().AsSingle().WithArguments(_ragdollPreferences);
            Container.Resolve<Ragdoll>().Disable();
        }
    }
}