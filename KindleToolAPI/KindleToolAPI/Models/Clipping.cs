namespace KindleToolAPI.Models
{
    [Serializable]
    public class Clipping
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Location { get; set; }
        public string AddedOn { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
    }
}
