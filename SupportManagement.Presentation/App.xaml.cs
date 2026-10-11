using SupportManagement.Infrastructure.Repositories;
using SupportManagement.Application.Services;
using SupportManagement.Presentation.Navigation;
using SupportManagement.Presentation.ViewModels;
using SupportManagement.Presentation.Views;
using Microsoft.UI.Xaml;
using Microsoft.Extensions.DependencyInjection;
using System;
using SupportManagement.Application.Interfaces;

namespace SupportManagement.Presentation;

public partial class App : Microsoft.UI.Xaml.Application
{
    private readonly IServiceProvider _serviceProvider;
    private Window? _window;


    public App()
    {
        InitializeComponent();

        var services = new ServiceCollection(); 
                                                
        services.AddSingleton<INavigationService, NavigationService>();

        services.AddSingleton<MainViewModel>();
        services.AddSingleton<MainWindow>();

        services.AddSingleton<ITicketRepository, JsonTicketRepository>();
        services.AddSingleton<ICustomerRepository, JsonCustomerRepository>();

        services.AddSingleton<ITicketService, TicketService>();
        services.AddSingleton<ICustomerService, CustomerService>();

        services.AddTransient<CustomersViewModel>();
        services.AddTransient<CustomersPage>();

        services.AddTransient<CreateCustomerViewModel>();
        services.AddTransient<CreateCustomerPage>();

        services.AddSingleton<EditCustomerViewModel>();
        services.AddTransient<EditCustomerPage>();

        services.AddTransient<TicketOverviewViewModel>();
        services.AddTransient<TicketOverviewPage>();

        services.AddTransient<TicketDetailsViewModel>();
        services.AddTransient<TicketDetailsPage>();

        services.AddTransient<CreateTicketViewModel>();
        services.AddTransient<CreateTicketPage>();

        //services.AddTransient<ManageTicketViewModel>();
        //services.AddTransient<ManageTicketPage>();

        services.AddTransient<HomeViewModel>();
        services.AddTransient<HomePage>();

        _serviceProvider = services.BuildServiceProvider();
    }

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        _window = _serviceProvider.GetRequiredService<MainWindow>();
        _window.Activate();
    }
}
