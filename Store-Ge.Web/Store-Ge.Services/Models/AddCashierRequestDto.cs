using System.ComponentModel.DataAnnotations;

namespace Store_Ge.Services.Models
{
    public class AddCashierRequestDto : ApplicationUserRegisterDto
    {
        [Required]
        public string StoreId { get; set; }
    }
}
