using System.ComponentModel.DataAnnotations;

namespace Store_Ge.Services.Models.SupplierModels
{
    public class AddSupplierRequestDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
