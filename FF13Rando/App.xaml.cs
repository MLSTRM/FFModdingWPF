using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Windows;

namespace FF13Rando;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        // Set up assembly loading from libs folder
        AssemblyLoadContext.Default.Resolving += OnAssemblyResolving;
        base.OnStartup(e);
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
