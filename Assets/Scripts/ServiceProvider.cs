using System;
using System.Collections.Generic;

public static class ServiceProvider
{
    private static readonly Dictionary<Type, object> Services = new();

    public static void SetService<T>(T service, bool overwriteIfFound = false)
    {
        if (!Services.TryAdd(typeof(T), service) && overwriteIfFound)
            Services[typeof(T)] = service;
    }

    public static bool TryGetService<T>(out T service) where T : class
    {
        if (Services.TryGetValue(typeof(T), out var myService)
            && myService is T tService)
        {
            service = tService;
            return true;
        }

        service = null;
        return false;
    }
}
