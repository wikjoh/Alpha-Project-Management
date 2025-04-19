using System.Reflection;

namespace Domain.Extensions;

public static class MapExtensions
{
    public static TDestination MapTo<TDestination>(this object source, HashSet<object>? visited = null)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        // Initialize the visited set if it's null
        visited ??= new HashSet<object>();

        // Check if the source object has already been visited
        if (visited.Contains(source))
        {
            return default!;
        }

        // Mark the current object as visited
        visited.Add(source);

        TDestination destination = Activator.CreateInstance<TDestination>()!;
        var sourceType = source.GetType();
        var sourceProps = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var destProps = destination.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var destProp in destProps)
        {
            var sourceProp = sourceProps.FirstOrDefault(p => p.Name == destProp.Name && p.CanRead);
            if (sourceProp == null || !destProp.CanWrite) continue;

            var sourceValue = sourceProp.GetValue(source);
            if (sourceValue == null)
            {
                destProp.SetValue(destination, null);
                continue;
            }

            if (destProp.PropertyType == sourceProp.PropertyType)
            {
                destProp.SetValue(destination, sourceValue);
            }
            else if (IsComplexType(destProp.PropertyType) && IsComplexType(sourceProp.PropertyType))
            {
                var mapMethod = typeof(MapExtensions)
                    .GetMethod(nameof(MapTo), BindingFlags.Public | BindingFlags.Static)!
                    .MakeGenericMethod(destProp.PropertyType);

                // Pass the visited set to the recursive call
                var nestedMapped = mapMethod.Invoke(null, new object[] { sourceValue, visited });
                destProp.SetValue(destination, nestedMapped);
            }
        }

        return destination;
    }

    private static bool IsComplexType(Type type)
    {
        return type.IsClass && type != typeof(string);
    }
}