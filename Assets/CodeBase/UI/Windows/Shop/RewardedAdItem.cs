using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services.Ads;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Shop
{
    public class RewardedAdItem : MonoBehaviour
    {
        [SerializeField] private Button _showAdButton;

        private IAdsService _adsService;
        private IPersistentProgressService _progressService;

        public void Construct(IAdsService adsService, IPersistentProgressService progresService)
        {
            _adsService = adsService;
            _progressService = progresService;
        }

        public void Initialize()
        {
            _showAdButton.onClick.AddListener(OnShowAdClicked);
        }

        public void Subscribe()
        {
            _adsService.RewardedVideoFinished += OnVideoFinished;
        }

        public void Cleanup()
        {
            _adsService.RewardedVideoFinished -= OnVideoFinished;
        }

        private void OnShowAdClicked()
        {
            _adsService.ShowRewardedVideo();
        }

        private void OnVideoFinished()
        {
            _progressService.Progress.WorldData.LootData.Add(_adsService.Reward);
        }
    }
}