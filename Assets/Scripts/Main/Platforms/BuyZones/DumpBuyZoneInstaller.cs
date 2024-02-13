﻿using Infrastructure.Data.Static.Core;
using Infrastructure.Services.PersistentData.Core;
using Main.Platforms.BuyZones.Core;
using Main.Platforms.Zones;
using Plugins.Banks;
using UI.ClampedBanks;
using UnityEngine;
using Zenject;

namespace Main.Platforms.BuyZones
{
    public class DumpBuyZoneInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private ReceiveZone _receiveZone;

        private ClampedIntegerBank _buyContainer;
        private IntegerBank _bank;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService)
        {
            _buyContainer = persistentDataService.PersistentData.PlayerData.PlatformsData.DumpPlatformData.BuyContainer;
            _bank = persistentDataService.PersistentData.PlayerData.Resources.Money;
        }

        #region MonoBehaviour

        private void OnValidate() => _receiveZone ??= GetComponentInChildren<ReceiveZone>(true);

        #endregion

        public override void InstallBindings()
        {
            BindBuyZone();
            BindPlatformBuyer();
            BindBuyVibration();
        }

        private void BindBuyZone()
        {
            Container.BindInstance(_buyContainer).WhenInjectedInto<ClampedBankLeftValueText>();
            Container.BindInstance(_bank).WhenInjectedInto<ReceiveZone>();
            Container.BindInstance(_buyContainer).WhenInjectedInto<ReceiveZone>();
            Container.BindInstance(_receiveZone).AsSingle();
        }

        private void BindPlatformBuyer() =>
            Container.BindInterfacesAndSelfTo<PlatformBuyer>().AsSingle().WithArguments(Prefab.DumpPlatform);

        private void BindBuyVibration() => Container.BindInterfacesTo<PlatformBuyVibration>().AsSingle();
    }
}