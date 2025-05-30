using MeadowServer.CardEngine.Components;
using MeadowShared.CardEngine.Models;

namespace MeadowServer.CardEngine.Factories;

public class ReflectionBasedFactory<T>(string rootNamespace) where T : ICardComponent
{
    protected readonly Dictionary<string, Type> TypesCache = new();

    public T CreateNew(ICardComponentModel model)
    {
        if (TypesCache.TryGetValue(model.TypeName, out var type))
        {
            Console.WriteLine($"Found cached type for {model.TypeName}");
            return CreateInstance(model, type);
        }
        
        var fullName = $"{rootNamespace}.{model.TypeName}";
        type = Type.GetType(fullName);
        if (type == null)
        {
            // TODO - log a warning here
            Console.WriteLine("Could not find type. searching all assemblies...");
            type = AppDomain.CurrentDomain.GetAssemblies()
                .Select(a => a.GetType(fullName))
                .FirstOrDefault(t => t != null);
        }

        TypesCache[model.TypeName] = type ?? throw new Exception($"Type '{model.TypeName}' not found'");
        Console.WriteLine($"Found type for {model.TypeName}. Cached new type");
        return CreateInstance(model, type);
    }

    private T CreateInstance(ICardComponentModel model, Type type)
    {
        var instance = (T)Activator.CreateInstance(type)!;;
        instance.OnCreate(model);
        return instance;
    }
}