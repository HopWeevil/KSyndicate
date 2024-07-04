using CodeBase.Infrastructure.Services;
using System;

namespace CodeBase.Services.Ads
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