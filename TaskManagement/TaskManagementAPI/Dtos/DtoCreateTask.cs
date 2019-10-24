using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Dtos
{
    public class DtoCreateTask
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Explanation { get; set; }
        [Required]
        public string ReturnDataType { get; set; }
        [Required]
        public string MethodName { get; set; }
        [Required]
        public bool IsProdcution { get; set; }
        [Required]
        public string FirstInputParameterDataType { get; set; }
        public string SecondInputParameterDataType { get; set; }
        [Required]
        public ICollection<DtoCreateCase> CreateCases { get; set; }
    }
}