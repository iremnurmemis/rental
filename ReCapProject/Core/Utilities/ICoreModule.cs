using Microsoft.Extensions.DependencyInjection;



namespace Core
{
    public interface ICoreModule
    {
        void Load(IServiceCollection serviceCollection);
    }
}
