using System.ComponentModel.DataAnnotations;

namespace CodeCompilerAPI.Dtos
{
    public class DtoRequestedCode
    {
        [Required]
        public int TaskId { get; set; }
        [Required]
        public string Code { get; set; }
    }
}