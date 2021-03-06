﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMailSenderLibrary.Models;

namespace WpfMailSenderScheduler.Data
{
    public static class TestData
    { 
        public static List<Server> Servers
        {
            get
            {
                var result = new List<Server>();
                for (int i = 0; i < 10; i++)
                {
                     result.Add(new Server { Address = $"address{i}", Port = 10000 + i });
                }
                return result;
            }
        }

        public static List<Sender> Senders
        {
            get
            {
                return Enumerable.Range(0, 10).Select(i => new Sender() { Address = $"address{i}", Name = $"name{i}" }).ToList();
            }
        }

        public static List<Recipient> Recipients
        {
            get
            {
                return Enumerable.Range(0, 10).Select(i => new Recipient() { Address = $"address{i}", Name = $"name{i}", Id = i }).ToList();
            }
        }

        public static List<Message> Messages
        {
            get
            {
                return Enumerable.Range(0, 10).Select(i => new Message() { Id = i, Subject = $"Subject{i}", Body = $"Body{i}"}).ToList();
            }
        }
    }
}
