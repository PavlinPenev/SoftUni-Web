using Store_Ge.Data.Enums;

namespace Store_Ge.Services.Models.StoreModels
{
    public class StoreDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public StoreTypeEnum Type { get; set; }
    }
}
