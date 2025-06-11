using System.Windows;
using Bartz24.RandoWPF;

namespace FF13Rando;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        // Set up assembly loading from libs folder
        AssemblyLoader.ConfigureLibsFolder();
        base.OnStartup(e);
    }
}
