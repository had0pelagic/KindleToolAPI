using KindleToolAPI.Util.Enums;

namespace KindleToolAPI.DTOs
{
    public class ClippingsNotionDto : IClippingsDto
    {
        public IFormFile File { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public ClippingTypeEnum Type { get; set; }
        public string DatabaseId { get; set; }
        public string Secret { get; set; }
    }
}
