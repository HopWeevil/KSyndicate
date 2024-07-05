using System.Runtime.Serialization;
using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private readonly IAssetProvider _assets;
        private readonly IStaticDataService _staticData;
    
        private Transform _uiRoot;
        private readonly IPersistentProgressService _progressService;
        private readonly IAdsService _adsService;

        public UIFactory(IAssetProvider assets, IStaticDataService staticData, IPersistentProgressService progressService, IAdsService adsService)
        {
            _assets = assets;
            _staticData = staticData;
            _progressService = progressService;
            _adsService = adsService;
        }

        public void CreateShop()
        {
            WindowConfig confing = _staticData.ForWindow(WindowId.Shop);
            ShopWindow window = Object.Instantiate(confing.Template, _uiRoot) as ShopWindow;
            window.Construct(_progressService, _adsService);
        }

        public async Task CreateUIRoot()
        {
            GameObject root = await _assets.Instantiate(AssetAddress.UIRootPath);
            _uiRoot = root.transform;
        }
    }
}