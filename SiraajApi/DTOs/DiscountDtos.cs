namespace SiraajApi.DTOs
{
    public class ValidateDiscountRequest
    {
        public string Code { get; set; } = string.Empty;
    }

    public class ValidateDiscountResponse
    {
        public bool Valid { get; set; }
        public int DiscountAmount { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
