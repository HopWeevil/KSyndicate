using CodeBase.Infrastructure.ServiceLocator;

namespace CodeBase.UI.Services.Windows
{
    public interface IWindowService : IService
    {
        void Open(WindowId windowId);
    }
}