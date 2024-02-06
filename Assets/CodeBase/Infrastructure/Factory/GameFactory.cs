using CodeBase.Infrastructure.AssetManagment;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
      
        private readonly IAssetProvider _assetProvider;

        public GameFactory(IAssetProvider assetProvider) 
        {
            _assetProvider = assetProvider;
        }

        public GameObject CreateHero(Vector3 position)
        {
            return _assetProvider.Instantiate(AssetsPath.HeroPath, position);
        }

        public void CreateHud()
        {
            _assetProvider.Instantiate(AssetsPath.HudPath);
        }
    }
}