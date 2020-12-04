using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfMailSenderScheduler.Interfaces;
using WpfMailSenderScheduler.Services;

namespace WpfMailSenderScheduler
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
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
}
