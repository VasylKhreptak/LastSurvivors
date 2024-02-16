﻿using System;
using System.Collections.Generic;
using Gameplay.Aim;
using Gameplay.Data;
using Gameplay.Entities.Collector;
using Gameplay.Entities.Helicopter;
using Gameplay.Entities.Platoon;
using Gameplay.Entities.Player;
using Gameplay.Entities.Zombie;
using Gameplay.Levels.StateMachine;
using Gameplay.Levels.StateMachine.States;
using Gameplay.Levels.StateMachine.States.Core;
using Gameplay.Waypoints;
using Gameplay.Weapons;
using ObjectPoolSystem.PoolCategories;
using Plugins.ObjectPoolSystem;
using UnityEngine;
using Utilities.CameraUtilities.Shaker;

namespace Zenject.Installers.SceneContext.Gameplay
{
    public class LevelInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private Camera _camera;
        [SerializeField] private Trackpad _trackpad;

        [Header("Preferences")]
        [SerializeField] private CameraShaker.Preferences _cameraShakerPreferences;
        [SerializeField] private Transform[] _playerWaypoints;

        [Header("Object Pool Preferences")]
        [SerializeField] private ObjectPoolPreference<Particle>[] _particlePoolsPreferences;
        [SerializeField] private ObjectPoolPreference<GeneralPool>[] _generalPoolsPreferences;

        [Header("Weapons")]
        [SerializeField] private WeaponAimer.Preferences _weaponAimPreferences;

        [Header("Entities")]
        [SerializeField] private Helicopter _helicopter;
        [SerializeField] private Platoon _platoon;

        #region MonoBehaviour

        private void OnValidate()
        {
            _camera ??= FindObjectOfType<Camera>(true);
            _trackpad ??= FindObjectOfType<Trackpad>(true);
            _helicopter ??= FindObjectOfType<Helicopter>(true);
            _platoon ??= FindObjectOfType<Platoon>(true);
        }

        #endregion

        public override void InstallBindings()
        {
            Container.BindInstance(_camera).AsSingle();
            Container.BindInstance(_trackpad).AsSingle();
            Container.BindInstance(_helicopter).AsSingle();
            Container.BindInstance(_platoon).AsSingle();

            BindHolders();
            BindCameraShaker();
            BindPlayerWaypoints();
            BindObjectPools();
            BindZombiesList();
            BindCollectorsList();
            BindLevelData();
            BindWeaponAimer();
            BindWeaponShooter();
            BindLevelStateMachine();
        }

        private void BindHolders()
        {
            Container.Bind<WeaponHolder>().AsSingle();
            Container.Bind<PlayerHolder>().AsSingle();
        }

        private void BindCameraShaker()
        {
            Container.BindInterfacesAndSelfTo<CameraShaker>().AsSingle().WithArguments(_cameraShakerPreferences);
        }

        private void BindPlayerWaypoints()
        {
            Container
                .Bind<Waypoints>()
                .AsSingle()
                .WithArguments(_playerWaypoints)
                .WhenInjectedInto<PlayerInstaller>();
        }

        private void BindObjectPools()
        {
            BindObjectPools(_particlePoolsPreferences, new GameObject("Particles").transform);
            BindObjectPools(_generalPoolsPreferences, new GameObject("General Pool").transform);
        }

        private void BindObjectPools<T>(ObjectPoolPreference<T>[] poolPreferences, Transform parent) where T : Enum
        {
            foreach (ObjectPoolPreference<T> preference in poolPreferences)
            {
                preference.CreateFunc = () => Container.InstantiatePrefab(preference.Prefab, parent);
            }

            ObjectPools<T> objectPools = new ObjectPools<T>(poolPreferences);

            Container.Bind<IObjectPools<T>>().FromInstance(objectPools).AsSingle();
        }

        private void BindLevelStateMachine()
        {
            BindLevelStates();
            Container.Bind<LevelStateFactory>().AsSingle();
            Container.BindInterfacesTo<LevelStateMachine>().AsSingle();
        }

        private void BindLevelStates()
        {
            Container.Bind<LevelStartState>().AsSingle();
            Container.Bind<LevelCompletedState>().AsSingle();
            Container.Bind<LevelFailedState>().AsSingle();
            Container.Bind<LevelLoopState>().AsSingle();
            Container.Bind<FinalizeProgressAndLoadMenuState>().AsSingle();
        }

        private void BindZombiesList() => Container.Bind<List<Zombie>>().AsSingle();

        private void BindCollectorsList() => Container.Bind<List<Collector>>().AsSingle();

        private void BindLevelData() => Container.Bind<LevelData>().AsSingle();

        private void BindWeaponAimer() =>
            Container.BindInterfacesAndSelfTo<WeaponAimer>().AsSingle().WithArguments(_weaponAimPreferences);

        private void BindWeaponShooter() => Container.BindInterfacesAndSelfTo<WeaponShooter>().AsSingle();
    }
}