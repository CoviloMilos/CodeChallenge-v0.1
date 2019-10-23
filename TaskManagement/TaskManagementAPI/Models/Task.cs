using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementAPI.Models
{
    [Table("Task")]
    public class Task
    {
        [Key]
        public int TaskId { get; set; }
        [Required]
        public Guid TaskGuid { get; set; }
        [Required]
        public int TaskNum { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Explanation { get; set; }
        [Required]
        public string ReturnDataType { get; set; }
        [Required]
        public string MethodName { get; set; }
        [Required]
        public string FirstInputParameterDataType { get; set; }
        public string SecondInputParameterDataType { get; set; }
        [Required]
        public bool IsProdcution { get; set; }
        [NotMapped]
        [Required]
        public ICollection<Case> Cases { get; set; }

        public Task()
        {
            TaskGuid = Guid.NewGuid();
            Cases = new Collection<Case>();
        }
    }
}