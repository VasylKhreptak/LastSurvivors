﻿using Cinemachine;
using Entities.Player;
using Infrastructure.Data.Static;
using Infrastructure.Data.Static.Core;
using Infrastructure.EntryPoints.Core;
using Infrastructure.Services.StaticData.Core;
using UnityEngine;
using Zenject;

namespace Infrastructure.EntryPoints
{
    public class MainEntryPoint : MonoBehaviour, IEntryPoint
    {
        private DiContainer _container;
        private GamePrefabs _prefabs;

        [Inject]
        private void Constructor(DiContainer container, IStaticDataService staticDataService)
        {
            _container = container;
            _prefabs = staticDataService.Prefabs;
        }

        #region MonoBehaviour

        private void Start()
        {
            Enter();
        }

        #endregion

        public void Enter()
        {
            Transform playerTransform = InitializePlayer();
            InitializeCamera(playerTransform);
            InitializePlatforms();
        }

        private Transform InitializePlayer()
        {
            GameObject playerObject = _container.InstantiatePrefab(_prefabs[Prefab.Player]);
            _container.Bind<Player>().FromComponentOn(playerObject).AsSingle();
            return playerObject.transform;
        }

        private void InitializeCamera(Transform target)
        {
            GameObject cameraRoot = _container.InstantiatePrefab(_prefabs[Prefab.Camera]);
            CinemachineVirtualCamera virtualCamera = cameraRoot.GetComponentInChildren<CinemachineVirtualCamera>(true);
            virtualCamera.Follow = target;
            virtualCamera.LookAt = target;

            Camera camera = cameraRoot.GetComponentInChildren<Camera>(true);
            _container.BindInstance(camera).AsSingle();
        }

        private void InitializePlatforms()
        {
            InitializeHelicopterPlatform();
            InitializeOilPlatform();
            InitializeDumpPlatform();
        }

        private void InitializeHelicopterPlatform() => _container.InstantiatePrefab(_prefabs[Prefab.HelicopterPlatform]);

        private void InitializeOilPlatform() => _container.InstantiatePrefab(_prefabs[Prefab.OilPlatform]);

        private void InitializeDumpPlatform() => _container.InstantiatePrefab(_prefabs[Prefab.DumpPlatform]);
    }
}