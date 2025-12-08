using System;
using System.Collections.Generic;

public static class ServiceProvider
{
    private static readonly Dictionary<Type, object> Services = new();
    private static readonly Dictionary<Type, List<object>> MultiServices = new();

    public static void SetService<T>(T service, bool overwriteIfFound = false)
    {
        var type = typeof(T);

        if (!Services.TryAdd(type, service) && overwriteIfFound)
            Services[type] = service;

        if (!MultiServices.ContainsKey(type))
            MultiServices[type] = new List<object>();

        MultiServices[type].Add(service);
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

    public static List<T> GetAllServices<T>() where T : class
    {
        var type = typeof(T);

        if (!MultiServices.TryGetValue(type, out var list))
            return new List<T>();

        List<T> result = new();

        foreach (var obj in list)
        {
            if (obj is T cast)
                result.Add(cast);
        }

        return result;
    }
}
