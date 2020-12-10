﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WpfMailSenderLibrary;
using WpfMailSenderLibrary.Models;
using WpfMailSenderLibrary.Services;

namespace WpfMailSenderLibraryTest
{
    [TestClass]
    public class SchedulerClassTest
    {
        private List<SenderTask> senderTasks;       

        [TestInitialize]
        public void TestPrepare()
        {
            senderTasks = Enumerable.Range(1, 20).Select(s => new SenderTask
            {
                SendDate = DateTime.Now.AddSeconds(30),
                Sender = new Sender { Login = $"test {s}", Password = "test", Address = "yandex.ru", Name = $"test {s}" },
                Recipient = new Recipient { Name = $"test {s}", Address = $"test{s}@yandex.ru" },
                Server=new Server { Address="yandex.ru", Port=587},
                Body = $"test {s} Body {s}",
                Subject = $"test {s} Body {s}"
            }
            ).Concat
            (
                Enumerable.Range(1, 20).Select(s => new SenderTask
            {
                SendDate = DateTime.Now.AddSeconds(-60),
                Sender = new Sender { Login = $"test {s}", Password = "test", Address = "yandex.ru", Name = $"test {s}" },
                Recipient = new Recipient { Name = $"test {s}", Address = $"test{s}@yandex.ru" },
                Server = new Server { Address = "yandex.ru", Port = 587 },
                Body = $"test {s} Body {s}",
                Subject = $"test {s} Body {s}"
            })).ToList();
        }

        [TestMethod]
        public void TestSendTask()
        {
            Debug.WriteLine($"Ожидаем {DateTime.Now:F}...."); 
            Debug.WriteLine($"ОТПРАВКА {DateTime.Now:F}....");
            var mailService = new DebugMailService();
            var sc = new SchedulerClass();
            sc.SendTask(senderTasks, mailService);
            var countExpected = 20;
            var countSend = senderTasks.Sum(x => x.IsSendEnd ? 1 : 0);
            Assert.AreEqual(countExpected, countSend);
        }

        [TestMethod]
        public void TestWithoutSendTask()
        {
            Debug.WriteLine($"Ожидаем {DateTime.Now:F}....");
            Thread.Sleep(60000);
            Debug.WriteLine($"ОТПРАВКА {DateTime.Now:F}....");
            var mailService = new DebugMailService();
            var sc = new SchedulerClass();
            sc.SendTask(senderTasks, mailService);
            var countExpected = 0;
            var countSend = senderTasks.Sum(x => x.IsSendEnd ? 1 : 0);
            Assert.AreEqual(countExpected, countSend);
        }
    }
}
