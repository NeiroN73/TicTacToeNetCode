using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Content.Scripts.Utils
{
    public static class TypeExtensions
    {
        public static bool InheritsOrImplements(this Type type, Type baseType)
        {
            type = ResolveGenericType(type);
            baseType = ResolveGenericType(baseType);

            while (type != typeof(object))
            {
                if (baseType == type || HasAnyInterfaces(type, baseType)) return true;

                type = ResolveGenericType(type.BaseType);
                if (type == null) return false;
            }

            return false;
        }

        static Type ResolveGenericType(Type type)
        {
            if (type is not {IsGenericType: true}) return type;

            var genericType = type.GetGenericTypeDefinition();
            return genericType != type ? genericType : type;
        }

        static bool HasAnyInterfaces(Type type, Type interfaceType)
        {
            return type.GetInterfaces().Any(i => ResolveGenericType(i) == interfaceType);
        }
        
        public static string[] FilterTypes<TFilter>() where TFilter : class
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .First(x => x.FullName == "Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");

            var filteredTypes = assembly.GetTypes()
                .Where(t => DefaultFilter(t, typeof(TFilter)))
                .OrderBy(x => x.Name)
                .ToList();

            var typeNames = filteredTypes.Select(t => t.ReflectedType == null ? t.Name : $"{t.ReflectedType.Name}.{t.Name}")
                .ToArray();
            return filteredTypes.Select(t => t.FullName).ToArray();
        }
        
        static bool DefaultFilter(Type type, Type filterType)
        {
            return !type.IsAbstract && !type.IsInterface && !type.IsGenericType && type.InheritsOrImplements(filterType);
        }
    }
}
