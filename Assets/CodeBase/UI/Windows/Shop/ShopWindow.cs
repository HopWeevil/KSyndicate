using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services.Ads;
using CodeBase.UI.Windows.Shop;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Windows
{
    public class ShopWindow : WindowBase
    {
        [SerializeField] private TextMeshProUGUI _skullText;
        [SerializeField] protected RewardedAdItem _adItem;

        public void Construct(IPersistentProgressService persistentProgress, IAdsService adsService)
        {
            base.Construct(persistentProgress);
            _adItem.Construct(adsService, persistentProgress);
        }

        protected override void Init()
        {
            _adItem.Initialize();
            RefreshSkullTextText();
        }

        protected override void SubscribeUpdates()
        {
            _adItem.Subscribe();
            Progress.WorldData.LootData.Changed += RefreshSkullTextText;
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            _adItem.Cleanup();
            Progress.WorldData.LootData.Changed -= RefreshSkullTextText;
        }

        private void RefreshSkullTextText()
        {
            _skullText.text = Progress.WorldData.LootData.Collected.ToString();
        }
    }
}