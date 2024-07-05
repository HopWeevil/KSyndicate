using CodeBase.Infrastructure.ServiceLocator;
using System;

namespace CodeBase.Infrastructure.Services.Ads
{
    public interface IAdsService : IService
    {
        event Action RewardedVideoReady;
        event Action RewardedVideoFinished;
        int Reward { get; }
        void Initialize();

        void ShowRewardedVideo();
    }
}