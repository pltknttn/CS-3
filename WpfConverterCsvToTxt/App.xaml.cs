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
using WpfConverterCsvToTxt.ViewModels;

namespace WpfConverterCsvToTxt
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string Program = Assembly.GetExecutingAssembly().Location;
        public static string AppDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); 


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
            services.AddSingleton<MainWindowViewModel>(); 
        } 
    }
}
