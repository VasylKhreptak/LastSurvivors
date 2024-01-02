﻿using Data.Persistent.Platforms;
using Data.Static.Balance.Platforms;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.StaticData.Core;
using Main.Platforms.RecruitmentLogic;
using Main.Platforms.Zones;
using Plugins.Banks;
using UI.ClampedBanks;
using UnityEngine;
using Zenject;

namespace Main.Platforms.CollectorsPlatform
{
    public class CollectorsPlatformInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private ReceiveZone _hireCollectorZone;
        [SerializeField] private EntityRecruiter _collectorRecruiter;

        private IntegerBank _bank;
        private CollectorsPlatformData _platformData;
        private CollectorsPlatformPreferences _platformPreferences;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService, IStaticDataService staticDataService)
        {
            _bank = persistentDataService.PersistentData.PlayerData.Resources.Money;
            _platformData = persistentDataService.PersistentData.PlayerData.PlatformsData.CollectorsPlatformData;
            _platformPreferences = staticDataService.Balance.CollectorsPlatformPreferences;
        }

        #region

        private void OnValidate()
        {
            _hireCollectorZone ??= GetComponentInChildren<ReceiveZone>(true);
            _collectorRecruiter ??= GetComponentInChildren<EntityRecruiter>(true);
        }

        #endregion

        public override void InstallBindings()
        {
            Container.BindInstance(_platformData).AsSingle();
            BindCollectorHireZone();
            BindCollectorsCountText();
            BindCollectorsRecruiter();
            BindCollectorPriceUpdater();
        }

        private void BindCollectorHireZone()
        {
            Container.BindInstance(_platformData.CollectorsBank).WhenInjectedInto<ReceiveZoneDrawer>();
            Container.BindInstance(_platformData.HireCollectorBank).WhenInjectedInto<ClampedBankLeftValueText>();
            Container.BindInstance(_bank).WhenInjectedInto<ReceiveZone>();
            Container.BindInstance(_platformData.HireCollectorBank).WhenInjectedInto<ReceiveZone>();
            Container.BindInstance(_hireCollectorZone).AsSingle();
        }

        private void BindCollectorsCountText()
        {
            Container.BindInstance(_platformData.CollectorsBank).WhenInjectedInto<ClampedBankValuesText>();
        }

        private void BindCollectorsRecruiter()
        {
            Container.BindInstance(_platformData.CollectorsBank).WhenInjectedInto<EntityRecruiter>();
            Container.BindInstance(_collectorRecruiter).AsSingle();
        }

        private void BindCollectorPriceUpdater()
        {
            Container
                .BindInterfacesTo<EntityPriceUpdater>()
                .AsSingle()
                .WithArguments(_platformData.CollectorsBank,
                    _platformData.HireCollectorBank,
                    _platformPreferences.CollectorsHirePreferences);
        }
    }
}