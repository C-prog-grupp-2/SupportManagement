using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;

namespace SupportManagement.Presentation;

public partial class App : Microsoft.UI.Xaml.Application
{
    // Håller en ServiceProvider som skapar och hämtar våra services och fönster.
    private readonly IServiceProvider _serviceProvider;

    // Referens till programmets huvudfönster.
    private Window? _window;

    public App()
    {
        // Kopplar App.xaml till den här C#-klassen.
        InitializeComponent();

        // Skapar en lista där vi registrerar de klasser som ska hanteras av Dependency Injection.
        var services = new ServiceCollection();

        // Registrerar MainWindow i Dependency Injection
        services.AddTransient<MainWindow>();

        // Bygger en ServiceProvider från våra registrerade services.
        _serviceProvider = services.BuildServiceProvider();
    }

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        // Hämtar MainWindow från Dependency Injection.
        _window = _serviceProvider.GetRequiredService<MainWindow>();

        // Visar huvudfönstret på skärmen.
        _window.Activate();
    }
}