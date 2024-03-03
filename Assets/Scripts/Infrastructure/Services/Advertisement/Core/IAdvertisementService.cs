﻿using System;

namespace Infrastructure.Services.Advertisement.Core
{
    public interface IAdvertisementService
    {
        public void ShowBanner();

        public void DestroyBanner();

        public void LoadInterstitial();

        public void ShowInterstitial();

        public void DestroyInterstitial();

        public void LoadRewardedVideo();

        public bool ShowRewardedVideo(Action onRewarded);

        public void DestroyRewardedVideo();
    }
}