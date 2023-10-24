﻿using Plugins.Banks;
using UniRx;
using UnityEngine;

namespace Data.Persistent
{
    public class HelicopterData
    {
        public readonly FloatReactiveProperty IncomeMultiplier = new FloatReactiveProperty(1f);
        public readonly ClampedIntegerBank FuelTank = new ClampedIntegerBank(0, 5);

        public IReadOnlyReactiveProperty<bool> CanTakeOff =>
            FuelTank.FillAmount.Select(fillAmount => Mathf.Approximately(fillAmount, 1f)).ToReactiveProperty();
    }
}