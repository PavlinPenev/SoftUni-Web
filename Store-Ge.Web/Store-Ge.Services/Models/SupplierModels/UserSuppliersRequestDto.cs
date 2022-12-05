using System.ComponentModel.DataAnnotations;

namespace Store_Ge.Services.Models.SupplierModels
{
    public class UserSuppliersRequestDto : FilterRequestDto
    {
        [Required]
        public string UserId { get; set; }
    }
}
