using System;
using System.ComponentModel;
using System.Reflection;

public static class EnumUtils
{
    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field.GetCustomAttribute<DescriptionAttribute>();
        return attribute == null ? value.ToString() : attribute.Description;
    }

    public static T FromDescription<T>(string description) where T : Enum
    {
        var type = typeof(T);
        foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            var fieldDescription = attribute?.Description ?? field.Name;

            if (fieldDescription == description) return (T)field.GetValue(null);
        }

        throw new ArgumentException($"No {type.Name} with description '{description}' found.");
    }
}