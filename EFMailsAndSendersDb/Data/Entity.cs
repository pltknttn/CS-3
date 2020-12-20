using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMailsAndSendersDb.Data
{
    public class Entity
    {
        [Column, Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }         
    }

    public class NamedEntity: Entity
    {
        [Column, Required, MinLength(6), MaxLength(255)]
        public string Name { get; set; }
    }  
}
