namespace Persistence.Helpers
{
    // Static class providing extension methods for working with TimeSpans.
    public static class TimeSpanExtensions
    {
        // Extension method to round a TimeSpan to the nearest specified number of minutes.
        public static TimeSpan RoundToNearestMinutes(this TimeSpan input, int minutes)
        {
            // Calculate the total minutes after rounding.
            var totalMinutes = (int)(input + new TimeSpan(0, minutes / 2, 0)).TotalMinutes;

            // Create a new TimeSpan with rounded total minutes.
            return new TimeSpan(0, totalMinutes - totalMinutes % minutes, 0);
        }
    }
}