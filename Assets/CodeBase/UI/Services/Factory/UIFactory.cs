﻿using System.Runtime.Serialization;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services.Ads;
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

        public void CreateUIRoot()
        {
            _uiRoot = _assets.Instantiate(AssetPath.UIRootPath).transform;
        }
    }
}