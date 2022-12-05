namespace Store_Ge.Services.Models
{
    public class FilterRequestDto
    {
        public string SearchTerm { get; set; }

        public string? OrderBy { get; set; }

        public bool IsDescending { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }

        public DateTime? DateAddedFrom { get; set; }

        public DateTime? DateAddedTo { get; set; }
    }
}
