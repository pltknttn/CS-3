using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
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
        public static string Program = Assembly.GetExecutingAssembly().Location;
        public static string AppDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string DataFileName = Path.Combine(AppDirectory, WpfMailSenderScheduler.Properties.Settings.Default.DataFileName); 


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
#if DEBUG
            services.AddTransient<IMailService, DebugMailService>();
#else
            services.AddTransient<IMailService, SmtpMailService>();
#endif 
             
            var memoryStorage = new DataStorageInMemory();
            services.AddSingleton<ISendersStorage>(memoryStorage);
            services.AddSingleton<IServersStorage>(memoryStorage);
            services.AddSingleton<IMessagesStorage>(memoryStorage);
            services.AddSingleton<IRecipientsStorage>(memoryStorage);

            //const string xmlFileName = "MailSenderStorage.xml";
            //var xmlStorage = new DataStorageInXmlFile(xmlFileName);
            //services.AddSingleton<ISendersStorage>(xmlStorage);
            //services.AddSingleton<IServersStorage>(xmlStorage);
            //services.AddSingleton<IMessagesStorage>(xmlStorage);
            //services.AddSingleton<IRecipientsStorage>(xmlStorage);

            services.AddSingleton<MainWindowViewModel>();
            services.AddTransient<SenderEditWindowViewModel>();
            services.AddTransient<ServerEditWindowViewModel>();
            services.AddTransient<RecipientEditWindowViewModel>();
            services.AddTransient<TaskEditWindowViewModel>();
        }

        public static void ShowDialogInfo(string msg)
        {
            Services.GetService<IDialogService>().ShowInfo(msg);
        }

        public static void ShowDialogError(string msg)
        {
            Services.GetService<IDialogService>().ShowError(msg);
        }

        public static void ShowDialogError(Exception ex)
        {
            Services.GetService<IDialogService>().ShowError(ex.InnerException?.Message??ex.Message);
        }
        public static void ShowDialogWarn(string msg)
        {
            Services.GetService<IDialogService>().ShowWarn(msg);
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
