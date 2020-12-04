using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfMailSenderScheduler.Interfaces;
using WpfMailSenderScheduler.Services;
using WpfMailSenderScheduler.ViewModels;

namespace WpfMailSenderScheduler
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IHost _hosting;

        public static IHost Hosting
        {
            get
            {
                if (_hosting != null) return _hosting;

                var hostBuilder = Host.CreateDefaultBuilder(Environment.GetCommandLineArgs());
                hostBuilder.ConfigureServices(ConfigureServices);
                _hosting = hostBuilder.Build();
                return _hosting;
            }
        }
        public static IServiceProvider Services => Hosting.Services;

        private static void ConfigureServices(HostBuilderContext hostBuilder, IServiceCollection services)
        {
            services.AddSingleton<IDialogService, WindowDialog>();
            services.AddTransient<IMailService, DebugMailService>();
            services.AddTransient<IMailService, SmtpMailService>();
            services.AddSingleton<MainWindowViewModel>();
        }
    }
    /* 
    using Microsoft.Extensions.DependencyInjection;
    
    public partial class App : Application
    {
        private static IServiceProvider _services;

        public static IServiceProvider Services => _services ?? GetServices().BuildServiceProvider();

        private static IServiceCollection GetServices()
        {
            var services = new ServiceCollection();
            InitializeServices(services);
            return services;
        }

        private static void InitializeServices(IServiceCollection services)
        {
            services.AddSingleton<IDialogService, WindowDialog>();
            services.AddTransient<IMailService, DebugMailService>();
            services.AddTransient<IMailService, SmtpMailService>();
        }
    }   
     */
}
