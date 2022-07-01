namespace Toolsfactory.Home.Adapters.Garbage.Awido
{
    public record CalendarLoaderOptions
    {
        public string DownloadUrlTemplate { get; set; }
        public byte YearDigits { get; init; } = 4;
        public byte LoadNextYearErliestMonth { get; init; } = 12;
        public bool PreloadNextYear { get; init; } = true;
    }
}
