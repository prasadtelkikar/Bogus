using System;

namespace Bogus.DataSets
{
   /// <summary>
   /// Methods for generating dates
   /// </summary>
   public class Date : DataSet
   {
      private bool hasMonthWideContext;
      private bool hasMonthAbbrContext;
      private bool hasWeekdayWideContext;
      private bool hasWeekdayAbbrContext;

      /// <summary>
      /// Sets the system clock time Bogus uses for date calculations.
      /// This value is normally <seealso cref="DateTime.Now"/>. If deterministic times are desired,
      /// set the <seealso cref="SystemClock"/> to a single instance in time.
      /// IE: () => new DateTime(2018, 4, 23);
      /// Setting this static value will effect and apply to all Faker[T], Faker,
      /// and new Date() datasets instances.
      /// </summary>
      public static Func<DateTime> SystemClock = () => DateTime.Now;

      /// <summary>
      /// Create a Date dataset
      /// </summary>
      public Date(string locale = "en") : base(locale)
      {
         this.hasMonthWideContext = HasKey("month.wide_context", false);
         this.hasMonthAbbrContext = HasKey("month.abbr_context", false);
         this.hasWeekdayWideContext = HasKey("weekday.wide_context", false);
         this.hasWeekdayAbbrContext = HasKey("weekday.abbr_context", false);
      }

      /// <summary>
      /// Get a <see cref="DateTime"/> in the past between <paramref name="refDate"/> and <paramref name="yearsToGoBack"/>.
      /// </summary>
      /// <param name="yearsToGoBack">Years to go back from <paramref name="refDate"/>. Default is 1 year.</param>
      /// <param name="refDate">The date to start calculations. Default is <see cref="DateTime.Now"/>.</param>
      public DateTime Past(int yearsToGoBack = 1, DateTime? refDate = null)
      {
         var maxDate = refDate ?? SystemClock();

         var minDate = maxDate.AddYears(-yearsToGoBack);

         var totalTimeSpanTicks = (maxDate - minDate).Ticks;

         var partTimeSpan = RandomTimeSpanFromTicks(totalTimeSpanTicks);

         return maxDate - partTimeSpan;
      }

      /// <summary>
      /// Get a <see cref="DateTimeOffset"/> in the past between <paramref name="refDate"/> and <paramref name="yearsToGoBack"/>.
      /// </summary>
      /// <param name="yearsToGoBack">Years to go back from <paramref name="refDate"/>. Default is 1 year.</param>
      /// <param name="refDate">The date to start calculations. Default is <see cref="DateTimeOffset.Now"/>.</param>
      public DateTimeOffset PastOffset(int yearsToGoBack = 1, DateTimeOffset? refDate = null)
      {
         var maxDate = refDate ?? SystemClock();

         var minDate = maxDate.AddYears(-yearsToGoBack);

         var totalTimeSpanTicks = (maxDate - minDate).Ticks;

         var partTimeSpan = RandomTimeSpanFromTicks(totalTimeSpanTicks);

         return maxDate - partTimeSpan;
      }

      /// <summary>
      /// Gets an random timespan from ticks.
      /// </summary>
      protected internal TimeSpan RandomTimeSpanFromTicks(long totalTimeSpanTicks)
      {
         //find % of the timespan
         var partTimeSpanTicks = Random.Double() * totalTimeSpanTicks;
         return TimeSpan.FromTicks(Convert.ToInt64(partTimeSpanTicks));
      }

      /// <summary>
      /// Get a <see cref="DateTime"/> that will happen soon.
      /// </summary>
      /// <param name="days">A date no more than <paramref name="days"/> ahead.</param>
      /// <param name="refDate">The date to start calculations. Default is <see cref="DateTimeOffset.Now"/>.</param>
      public DateTime Soon(int days = 1, DateTime? refDate = null)
      {
         var dt = refDate ?? SystemClock();
         return Between(dt, dt.AddDays(days));
      }

      /// <summary>
      /// Get a <see cref="DateTimeOffset"/> that will happen soon.
      /// </summary>
      /// <param name="days">A date no more than <paramref name="days"/> ahead.</param>
      /// <param name="refDate">The date to start calculations. Default is <see cref="DateTimeOffset.Now"/>.</param>
      public DateTimeOffset SoonOffset(int days = 1, DateTimeOffset? refDate = null)
      {
         var dt = refDate ?? SystemClock();
         return BetweenOffset(dt, dt.AddDays(days));
      }

      /// <summary>
      /// Get a <see cref="DateTime"/> in the future between <paramref name="refDate"/> and <paramref name="yearsToGoForward"/>.
      /// </summary>
      /// <param name="yearsToGoForward">Years to go forward from <paramref name="refDate"/>. Default is 1 year.</param>
      /// <param name="refDate">The date to start calculations. Default is <see cref="DateTime.Now"/>.</param>
      public DateTime Future(int yearsToGoForward = 1, DateTime? refDate = null)
      {
         var minDate = refDate ?? SystemClock();

         var maxDate = minDate.AddYears(yearsToGoForward);

         var totalTimeSpanTicks = (maxDate - minDate).Ticks;

         var partTimeSpan = RandomTimeSpanFromTicks(totalTimeSpanTicks);

         return minDate + partTimeSpan;
      }

      /// <summary>
      /// Get a <see cref="DateTimeOffset"/> in the future between <paramref name="refDate"/> and <paramref name="yearsToGoForward"/>.
      /// </summary>
      /// <param name="yearsToGoForward">Years to go forward from <paramref name="refDate"/>. Default is 1 year.</param>
      /// <param name="refDate">The date to start calculations. Default is <see cref="DateTimeOffset.Now"/>.</param>
      public DateTimeOffset FutureOffset(int yearsToGoForward = 1, DateTimeOffset? refDate = null)
      {
         var minDate = refDate ?? SystemClock();

         var maxDate = minDate.AddYears(yearsToGoForward);

         var totalTimeSpanTicks = (maxDate - minDate).Ticks;

         var partTimeSpan = RandomTimeSpanFromTicks(totalTimeSpanTicks);

         return minDate + partTimeSpan;
      }

      /// <summary>
      /// Get a random <see cref="DateTime"/> between <paramref name="start"/> and <paramref name="end"/>.
      /// </summary>
      /// <param name="start">Start time - The returned <seealso cref="DateTimeKind"/> is used from this parameter.</param>
      /// <param name="end">End time</param>
      public DateTime Between(DateTime start, DateTime end)
      {
         var minTicks = Math.Min(start.Ticks, end.Ticks);
         var maxTicks = Math.Max(start.Ticks, end.Ticks);

         var totalTimeSpanTicks = maxTicks - minTicks;

         var partTimeSpan = RandomTimeSpanFromTicks(totalTimeSpanTicks);

         return new DateTime(minTicks, start.Kind) + partTimeSpan;
      }

      /// <summary>
      /// Get a random <see cref="DateTimeOffset"/> between <paramref name="start"/> and <paramref name="end"/>.
      /// </summary>
      /// <param name="start">Start time - The returned <seealso cref="DateTimeOffset"/> offset value is used from this parameter</param>
      /// <param name="end">End time</param>
      public DateTimeOffset BetweenOffset(DateTimeOffset start, DateTimeOffset end)
      {
         var minTicks = Math.Min(start.Ticks, end.Ticks);
         var maxTicks = Math.Max(start.Ticks, end.Ticks);

         var totalTimeSpanTicks = maxTicks - minTicks;

         var partTimeSpan = RandomTimeSpanFromTicks(totalTimeSpanTicks);

         return new DateTimeOffset(minTicks, start.Offset) + partTimeSpan;
      }

      /// <summary>
      /// Get a random <see cref="DateTime"/> within the last few days.
      /// </summary>
      /// <param name="days">Number of days to go back.</param>
      /// <param name="refDate">The date to start calculations. Default is <see cref="DateTime.Now"/>.</param>
      public DateTime Recent(int days = 1, DateTime? refDate = null)
      {
         var maxDate = refDate ?? SystemClock();

         var minDate = days == 0 ? SystemClock().Date : maxDate.AddDays(-days);

         var totalTimeSpanTicks = (maxDate - minDate).Ticks;

         var partTimeSpan = RandomTimeSpanFromTicks(totalTimeSpanTicks);

         return maxDate - partTimeSpan;
      }

      /// <summary>
      /// Get a random <see cref="DateTimeOffset"/> within the last few days.
      /// </summary>
      /// <param name="days">Number of days to go back.</param>
      /// <param name="refDate">The date to start calculations. Default is <see cref="DateTimeOffset.Now"/>.</param>
      public DateTimeOffset RecentOffset(int days = 1, DateTimeOffset? refDate = null)
      {
         var maxDate = refDate ?? SystemClock();

         var minDate = days == 0 ? SystemClock().Date : maxDate.AddDays(-days);

         var totalTimeSpanTicks = (maxDate - minDate).Ticks;

         var partTimeSpan = RandomTimeSpanFromTicks(totalTimeSpanTicks);

         return maxDate - partTimeSpan;
      }

      /// <summary>
      /// Get a random <see cref="TimeSpan"/>.
      /// </summary>
      /// <param name="maxSpan">Maximum of time to span. Default 1 week/7 days.</param>
      public TimeSpan Timespan(TimeSpan? maxSpan = null)
      {
         var span = maxSpan ?? TimeSpan.FromDays(7);

         return RandomTimeSpanFromTicks(span.Ticks);
      }

      /// <summary>
      /// Get a random month.
      /// </summary>
      public string Month(bool abbreviation = false, bool useContext = false)
      {
         var type = "wide";
         if( abbreviation )
            type = "abbr";

         if( useContext &&
             (type == "wide" && hasMonthWideContext) ||
             (type == "abbr" && hasMonthAbbrContext) )
         {
            type += "_context";
         }

         return GetRandomArrayItem("month." + type);
      }

      /// <summary>
      /// Get a random weekday.
      /// </summary>
      public string Weekday(bool abbreviation = false, bool useContext = false)
      {
         var type = "wide";
         if( abbreviation )
            type = "abbr";

         if( useContext &&
             (type == "wide" && hasWeekdayWideContext) ||
             (type == "abbr" && hasWeekdayAbbrContext) )
         {
            type += "_context";
         }

         return GetRandomArrayItem("weekday." + type);
      }

      /// <summary>
      /// Get a timezone string. Eg: America/Los_Angeles
      /// </summary>
      public string TimeZoneString()
      {
         return GetRandomArrayItem("address", "time_zone");
      }
#if NET6_0
      /// <summary>
      /// Get a <see cref="DateOnly"/> in the past between <paramref name="refDateOnly"/> and <paramref name="yearsToGoBack"/>.
      /// </summary>
      /// <param name="yearsToGoBack">Years to go back from <paramref name="refDateOnly"/>. Default is 1 year.</param>
      /// <param name="refDateOnly">The only date to start calculations. Default is the only Date from <see cref="DateTime.Now"/>.</param>
      public DateOnly PastDateOnly(int yearsToGoBack = 1, DateOnly? refDateOnly = null)
      {
         var dateTime = refDateOnly?.ToDateTime(default) ?? SystemClock();
         
         var dtTimePast = Past(yearsToGoBack, dateTime);
         
         return DateOnly.FromDateTime(dtTimePast);
      }

      /// <summary>
      /// Get a <see cref="DateOnly"/> that will happen soon.
      /// </summary>
      /// <param name="days">A date no more than <paramref name="days"/> ahead.</param>
      /// <param name="refDateOnly">The date to start calculations. Default is the only date from <see cref="DateTimeOffset.Now"/>.</param>
      public DateOnly SoonDateOnly(int days = 1, DateOnly? refDateOnly = null)
      {
         var dateTime = refDateOnly?.ToDateTime(default) ?? SystemClock();

         var dtTimePast = Soon(days, dateTime);

         return DateOnly.FromDateTime(dtTimePast);
      }

      /// <summary>
      /// Get a <see cref="DateOnly"/> in the future between <paramref name="refDateOnly"/> and <paramref name="yearsToGoForward"/>.
      /// </summary>
      /// <param name="yearsToGoForward">Years to go forward from <paramref name="refDateOnly"/>. Default is 1 year.</param>
      /// <param name="refDateOnly">The only date to start calculations. Default is the only date from <see cref="DateTimeOffset.Now"/>.</param>
      public DateOnly FutureDateOnly(int yearsToGoForward = 1, DateOnly? refDateOnly = null)
      {
         var dateTime = refDateOnly?.ToDateTime(default) ?? SystemClock();

         var dtTimePast = Future(yearsToGoForward, dateTime);

         return DateOnly.FromDateTime(dtTimePast);
      }

      /// <summary>
      /// Get a random <see cref="DateOnly"/> between <paramref name="start"/> and <paramref name="end"/>.
      /// </summary>
      /// <param name="start">Start Date - The returned <seealso cref="DateOnly"/> is used from this parameter.</param>
      /// <param name="end">End Date</param>
      public DateOnly BetweenDateOnly(DateOnly start, DateOnly end)
      {
         var startDateTime = start.ToDateTime(default);
         var endDateTime = end.ToDateTime(default);

         var resultantDateTime = Between(startDateTime, endDateTime);

         return DateOnly.FromDateTime(resultantDateTime);
      }

      /// <summary>
      /// Get a random <see cref="DateOnly"/> within the last few days.
      /// </summary>
      /// <param name="days">Number of days to go back.</param>
      /// <param name="refDateOnly">The date to start calculations. Default is the only date from <see cref="DateTimeOffset.Now"/>.</param>
      public DateOnly RecentDateOnly(int days = 1, DateOnly? refDateOnly = null)
      {
         var dateTime = refDateOnly?.ToDateTime(default) ?? SystemClock();

         var resultantDateTime = Recent(days, dateTime);
         
         return DateOnly.FromDateTime(resultantDateTime);
      }

      /// <summary>
      /// Get a random <see cref="TimeOnly"/> between <paramref name="start"/> and <paramref name="end"/>.
      /// </summary>
      /// <param name="start">Start time</param>
      /// <param name="end">End time</param>
      /// <param name="dateTimeKind">Representation of DateTime. Default is <seealso cref="DateTimeKind.Local"/></param>
      public TimeOnly BetweenTimeOnly(TimeOnly start, TimeOnly end, DateTimeKind dateTimeKind = DateTimeKind.Local)
      {
         var minTicks = Math.Min(start.Ticks, end.Ticks);
         var maxTicks = Math.Max(start.Ticks, end.Ticks);

         var totalTimeSpanTicks = maxTicks - minTicks;

         var partTimeSpan = RandomTimeSpanFromTicks(totalTimeSpanTicks);

         var resultantDate = new DateTime(minTicks, dateTimeKind) + partTimeSpan;

         return TimeOnly.FromDateTime(resultantDate);
      }

      /// <summary>
      /// Get a <see cref="TimeOnly"/> that will happen soon.
      /// </summary>
      /// <param name="mins">Minutes no more than <paramref name="mins"/> ahead.</param>
      /// <param name="refTimeOnly">The Time to start calculations. Default is the only time from <see cref="DateTime.Now"/>.</param>
      public TimeOnly SoonTimeOnly(int mins = 1, TimeOnly? refTimeOnly = null)
      {
         var timeOnly = refTimeOnly ?? TimeOnly.FromDateTime(DateTime.Now);

         var dateTime = DateTime.Now.Date + timeOnly.ToTimeSpan();
         
         var dtTimeSoon = Between(dateTime, dateTime.AddMinutes(mins));
         
         return TimeOnly.FromDateTime(dtTimeSoon);
      }

      /// <summary>
      /// Get a random <see cref="TimeOnly"/> within the last few Minutes.
      /// </summary>
      /// <param name="mins">Minutes <paramref name="mins"/> of the day to go back.</param>
      /// <param name="refTimeOnly">The Time to start calculations. Default is the time from <see cref="DateTime.Now"/>.</param>
      public TimeOnly RecentTimeOnly(int mins = 1, TimeOnly? refTimeOnly = null)
      {
         var maxDate = SystemClock();

         var minDate = maxDate.AddMinutes(-mins);

         var totalTimeSpanTicks = (maxDate - minDate).Ticks;

         var partTimeSpan = RandomTimeSpanFromTicks(totalTimeSpanTicks);
         
         var pastTimeCal = maxDate - partTimeSpan;
         
         return TimeOnly.FromDateTime(pastTimeCal);
      }
#endif
   }
}