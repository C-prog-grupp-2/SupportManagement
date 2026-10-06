using Microsoft.UI.Xaml;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace SupportManagement.Presentation;

public partial class App : Microsoft.UI.Xaml.Application
{
    private readonly IServiceProvider _serviceProvider;
    private Window? _window;


    public App()
    {
        InitializeComponent();
        var services = new ServiceCollection();

        services.AddTransient<MainWindow>();

        _serviceProvider = services.BuildServiceProvider();
    }

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        _window = _serviceProvider.GetRequiredService<MainWindow>();
        _window.Activate();
    }
}
