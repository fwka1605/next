namespace Rac.VOne.Web.Spreadsheets
{
    public class Picture : IPicture
    {
        public string EmbedId { get; set; }
        public uint PropertyId { get; set; }
        public string PropertyName { get; set; }
        public IMarker From { get; set; }
        public IMarker To { get; set; }
        public long ExtentCx { get; set; }
        public long ExtentCy { get; set; }
    }
}
