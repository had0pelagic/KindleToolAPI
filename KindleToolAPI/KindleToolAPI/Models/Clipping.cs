using KindleToolAPI.Util.Enums;

namespace KindleToolAPI.Models
{
    public class Clipping
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Location { get; set; }
        public string AddedOn { get; set; }
        public DateTime Date { get; set; }
        public ClippingTypeEnum Type { get; set; }
        public string Text { get; set; }
    }
}
