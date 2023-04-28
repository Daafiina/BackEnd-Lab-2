using BmmAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace BmmAPI.DTOs
{
    public class GenreCreationDTO
    {

        [Required(ErrorMessage = "The field with name {0} is required")]
        [StringLength(50)]
        [FirstLetterUppercase]
        public string Name { get; set; }
    }
}
