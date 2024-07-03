using TMPro;
using UnityEngine;

namespace CodeBase.UI.Windows
{
    public class ShopWindow : WindowBase
    {
        [SerializeField] private TextMeshProUGUI _skullText;

        protected override void Init()
        {
            RefreshSkullTextText();
        }

        protected override void SubscribeUpdates()
        {
            Progress.WorldData.LootData.Changed += RefreshSkullTextText;
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            Progress.WorldData.LootData.Changed -= RefreshSkullTextText;
        }

        private void RefreshSkullTextText()
        {
            _skullText.text = Progress.WorldData.LootData.Collected.ToString();
        }
    }
}