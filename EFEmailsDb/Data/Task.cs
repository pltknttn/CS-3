using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFEmailsDb.Data
{
    public class Task : NamedEntity
    {
        [Required, Column(TypeName = "int"), EnumDataType(typeof(TaskKind))]
        public TaskKind TaskKind { get; set; } = TaskKind.Send;

        [Column, Required, ForeignKey("Server")]
        public int ServerId { get; set; }

        [Column, Required]
        public DateTime TaskDate { get; set; }

        [Column]
        public DateTime? RunDate { get; set; }

        [Required, Column(TypeName = "int"), EnumDataType(typeof(TaskState))]
        public TaskState TaskState { get; set; } = TaskState.None;

        [Column, Required]
        public int Attempt { get; set; }

        public Server Server { get; set; }
        public ICollection<TaskMessage> TaskMessages { get; set; }
    }

    public enum TaskKind
    {
        Send = 1,
        Receive = 2
    }

    public enum TaskState : int
    {
        None = 0,
        Wait = 1,
        InWork = 2,
        Performed = 3,
        Error = 4 
    }

    public class TaskMessage : Entity
    { 
        [Column, Required, ForeignKey("Task")]
        public int TaskId { get; set; }

        [Column, Required, ForeignKey("Message")]
        public int MessageId { get; set; }

        [Column]
        public string Error { get; set; }

        [Column, Required]
        public bool IsSuccessful { get; set; }


        public Message Message { get; set; }
        public Task Task { get; set; }
    } 
}
