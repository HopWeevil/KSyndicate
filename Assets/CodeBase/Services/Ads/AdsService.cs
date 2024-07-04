using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace CodeBase.Services.Ads
{
    public class AdsService : IAdsService, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private const string AndroidGameId = "5651057";
        private const string IOSGameId = "5651056";

        private const string UnityRewardedVideoIdAndroid = "Rewarded_Android";
        private const string UnityRewardedVideoIdIOS = "Rewarded_iOS";

        private string _gameId;
        private string _placementId;
    
        public event Action RewardedVideoReady;
        public event Action RewardedVideoFinished;
        public int Reward => 15;

        public void Initialize()
        {
            SetIdsForCurrentPlatform();
            Advertisement.Initialize(_gameId, true, this);
        
        }

        public void ShowRewardedVideo()
        {
            Advertisement.Show(_placementId, this);
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log($"OnUnityAdsReady {placementId}");

            if (placementId == _placementId)
            {
                RewardedVideoReady?.Invoke();
            }
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"OnUnityAdsFailedToLoad - {error} {" - "} {message}");
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.Log($"OnUnityAdsShowFailure - {error} {" - "} {message}");
        }

        public void OnUnityAdsShowStart(string placementId)
        {
            Debug.Log($"OnUnityAdsShowStart {placementId}");
        }

        public void OnUnityAdsShowClick(string placementId)
        {
            Debug.Log($"OnUnityAdsShowClick {placementId}");
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            if(showCompletionState == UnityAdsShowCompletionState.COMPLETED)
            {
                RewardedVideoFinished?.Invoke();
            }
        }

        private void SetIdsForCurrentPlatform()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    _gameId = AndroidGameId;
                    _placementId = UnityRewardedVideoIdAndroid;
                    break;

                case RuntimePlatform.IPhonePlayer:
                    _gameId = IOSGameId;
                    _placementId = UnityRewardedVideoIdIOS;
                    break;

                case RuntimePlatform.WindowsEditor:
                    _gameId = IOSGameId;
                    _placementId = UnityRewardedVideoIdIOS;
                    break;

                default:
                    Debug.Log("Unsupported platform for ads.");
                    break;
            }
        }

        public void OnInitializationComplete()
        {
            Debug.Log($"OnInitializationComplete");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"OnInitializationFailed {error}" + message);
        }
    }
}