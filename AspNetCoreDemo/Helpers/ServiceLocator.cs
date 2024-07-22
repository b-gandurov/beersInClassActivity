using System;

public static class ServiceLocator
{
    private static IServiceProvider _serviceProvider;

    public static void Configure(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public static T GetService<T>()
    {
        return (T)_serviceProvider.GetService(typeof(T));
    }
}
