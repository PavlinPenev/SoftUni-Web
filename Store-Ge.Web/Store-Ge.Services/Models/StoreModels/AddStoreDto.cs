using Store_Ge.Data.Enums;
using System.ComponentModel.DataAnnotations;
using static Store_Ge.Common.Constants.ValidationConstants;

namespace Store_Ge.Services.Models.StoreModels
{
    public class AddStoreDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [MaxLength(STORE_NAME_MAX_LENGTH)]
        public string Name { get; set; }

        [Required]
        [Range(0, 6)]
        public StoreTypeEnum Type { get; set; }
    }
}
