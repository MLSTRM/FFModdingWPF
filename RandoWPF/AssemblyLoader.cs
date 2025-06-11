using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Bartz24.RandoWPF;

/// <summary>
/// Provides shared assembly loading functionality for randomizer applications
/// to support loading DLLs from a libs folder.
/// </summary>
public static class AssemblyLoader
{
    /// <summary>
    /// Configures assembly loading to resolve assemblies from the libs folder.
    /// Call this during application startup.
    /// </summary>
    public static void ConfigureLibsFolder()
    {
        AssemblyLoadContext.Default.Resolving += OnAssemblyResolving;
    }

    private static Assembly? OnAssemblyResolving(AssemblyLoadContext context, AssemblyName assemblyName)
    {
        string? assemblyPath = GetLibsAssemblyPath(assemblyName.Name);
        return assemblyPath != null ? context.LoadFromAssemblyPath(assemblyPath) : null;
    }

    private static string? GetLibsAssemblyPath(string? assemblyName)
    {
        if (string.IsNullOrEmpty(assemblyName))
            return null;

        string appDir = AppDomain.CurrentDomain.BaseDirectory;
        string libsDir = Path.Combine(appDir, "libs");
        string assemblyPath = Path.Combine(libsDir, $"{assemblyName}.dll");

        return File.Exists(assemblyPath) ? assemblyPath : null;
    }
}