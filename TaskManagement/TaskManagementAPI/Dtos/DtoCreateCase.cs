using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Dtos
{
    public class DtoCreateCase
    {
        [Required]
        public string FirstInputParameter { get; set; }
        public string SecondInputParameter { get; set; }
        [Required]
        public string ValidReturnValue { get; set; }
        [Required]
        public bool SecretTest { get; set; }
    }
}