using KindleToolAPI.Util.Enums;

namespace KindleToolAPI.DTOs
{
    public class ClippingsNotionDto : IClippingsDto
    {
        public IFormFile File { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public ClippingTypeEnum Type { get; set; } = ClippingTypeEnum.All;
        public bool TakeFirst { get; set; } = false;
        public bool TakeLast { get; set; } = false;
        public int Limit { get; set; } = 0;
        public string DatabaseId { get; set; }
        public string Secret { get; set; }
    }
}
