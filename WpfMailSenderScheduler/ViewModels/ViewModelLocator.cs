﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace WpfMailSenderScheduler.ViewModels
{
    public class ViewModelLocator
    {
        public MainWindowViewModel MainWindowModel => App.Services.GetRequiredService<MainWindowViewModel>();
        public SenderEditWindowViewModel SenderEditWindowModel => App.Services.GetRequiredService<SenderEditWindowViewModel>();
        public ServerEditWindowViewModel ServerEditWindowModel => App.Services.GetRequiredService<ServerEditWindowViewModel>();
        public RecipientEditWindowViewModel RecipientEditWindowModel => App.Services.GetRequiredService<RecipientEditWindowViewModel>();
        public TaskEditWindowViewModel TaskEditWindowViewModel => App.Services.GetRequiredService<TaskEditWindowViewModel>();
    }
}
