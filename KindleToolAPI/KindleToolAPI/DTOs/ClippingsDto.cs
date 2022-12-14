using KindleToolAPI.Util.Enums;

namespace KindleToolAPI.DTOs
{
    public class ClippingsDto : IClippingsDto
    {
        public IFormFile File { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public ClippingTypeEnum Type { get; set; } = ClippingTypeEnum.All;
        public bool TakeFirst { get; set; } = false;
        public bool TakeLast { get; set; } = false;
        public int Limit { get; set; } = 0;
    }
}
