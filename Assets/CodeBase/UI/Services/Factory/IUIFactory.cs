using CodeBase.Infrastructure.ServiceLocator;

namespace CodeBase.UI.Services.Factory
{
    public interface IUIFactory: IService
    {
        void CreateShop();
        void CreateUIRoot();
    }
}