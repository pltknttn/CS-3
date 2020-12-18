﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFEmailsDb.Data
{
    public class Sender : NamedEntity
    {
        [Column, Required, MinLength(6), MaxLength(100)]
        public string Address { get; set;}

        [Column, StringLength(500)]
        public string Description { get; set; }
    }
}
