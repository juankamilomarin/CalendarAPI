using Model;
using Quartz;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Calendar.Models
{

    public class EventViewModel
    {

        //public const int DAYLY = 0;
        public const int WEEKLY = 1;
        public const int MONTHLY_EVERY_DAY_OF_THE_MONTH = 2;
        public const int MONTHLY_EVERY_DAY_OF_THE_WEEK = 3;
        public const int ANUALLY_EVERY_DAY_OF_THE_MONTH = 4;
        public const int ANUALLY_EVERY_DAY_OF_THE_WEEK = 5;

        private const int WEEK_DAYS = 7;
        private const int YEAR_MONTHS = 12;

        private const string NO_SPECIFIC_VALUE = "?";
        private const string PATH_SEPARATOR = "/";
        private const string ASTERISK = "*";
        private const string BLANK = " ";


        public int EventID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        public string Notes { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public bool Recurrence { get; set; }

        [Range(1, 5)]
        public int? FrequencyRule { get; set; }

        [Range(0, Int32.MaxValue)]
        public int? Frequency { get; set; }

        public DateTime? EndBy { get; set; }

        [Range(0, 4)]
        public int? DaysOfWeek { get; set; }

        [Range(1, 6)]
        public int? OrdinalDayOfTheWeek { get; set; }

        public string CronExpression { get; set; }

        public List<EventRepetition> Repetitions { get; set; }

        public EventViewModel()
        {
        }

        public EventViewModel(Event ev)
        {
            EventID = ev.EventID;
            UserID = ev.UserID;
            Name = ev.Name;
            Location = ev.Location;
            Notes = ev.Notes;
            StartDate = ev.StartDate;
            EndDate = ev.EndDate;
            Recurrence = ev.Recurrence ?? false;

            if (Recurrence)
            {
                FrequencyRule = ev.FrequencyRule;
                Frequency = ev.Frequency ?? 1;
                try
                {
                    DaysOfWeek = Convert.ToInt32(ev.DaysOfWeek);
                }
                catch (Exception e)
                {
                    DaysOfWeek = null;
                }
                OrdinalDayOfTheWeek = ev.OrdinalDayOfTheWeek;

                EndBy = ev.EndBy;
                CronExpression = ev.CronExpression;

                Repetitions = GetRepetitions(this);

            }
        }

        public string GetCronExpression()
        {
            //Test http://www.cronmaker.com/
            if (Recurrence)
            {
                var cronExpression = new StringBuilder();

                //Seconds
                cronExpression.Append("0");
                cronExpression.Append(BLANK);

                //Minutes
                cronExpression.Append(StartDate.Minute);
                cronExpression.Append(BLANK);

                //Houres
                cronExpression.Append(StartDate.Hour);
                cronExpression.Append(BLANK);

                double frequency = Frequency ?? 1;
                switch (FrequencyRule)
                {
                    /*
                    case DAYLY:
                        cronJob.Append("1" + PATH_SEPARATOR + Frequency);
                        cronJob.Append(BLANK);
                        cronJob.Append("* ? *");
                        break;
                    */
                    case WEEKLY:
                        //Frequency in weekly events is handle in the presententation (when repetitions of events are created)
                        cronExpression.Append("? *");
                        cronExpression.Append(BLANK);
                        //TODO: this has to be changed in order to support multiple days of week
                        cronExpression.Append(Enum.GetName(typeof(DayOfWeek), DaysOfWeek).Substring(0, 3).ToUpper());
                        cronExpression.Append(BLANK);
                        cronExpression.Append(ASTERISK);

                        break;

                    case MONTHLY_EVERY_DAY_OF_THE_MONTH:
                        cronExpression.Append(StartDate.Day);
                        cronExpression.Append(BLANK);
                        cronExpression.Append("1" + PATH_SEPARATOR + frequency);
                        cronExpression.Append(BLANK);
                        cronExpression.Append("? *");

                        break;
                    case MONTHLY_EVERY_DAY_OF_THE_WEEK:
                        cronExpression.Append(NO_SPECIFIC_VALUE);
                        cronExpression.Append(BLANK);
                        cronExpression.Append("1" + PATH_SEPARATOR + frequency);
                        cronExpression.Append(BLANK);
                        //TODO: this has to be changed in order to support multiple days of week
                        cronExpression.Append(Enum.GetName(typeof(DayOfWeek), DaysOfWeek).Substring(0, 3).ToUpper());
                        if (OrdinalDayOfTheWeek == 6)
                        {
                            cronExpression.Append("L");
                        }
                        else
                        {
                            cronExpression.Append("#");
                            cronExpression.Append(OrdinalDayOfTheWeek);
                        }

                        cronExpression.Append(BLANK);
                        cronExpression.Append(ASTERISK);

                        break;

                    case ANUALLY_EVERY_DAY_OF_THE_MONTH:
                        cronExpression.Append(StartDate.Day);
                        cronExpression.Append(BLANK);
                        cronExpression.Append(frequency);
                        cronExpression.Append(BLANK);
                        cronExpression.Append("? *");

                        break;

                    case ANUALLY_EVERY_DAY_OF_THE_WEEK:
                        cronExpression.Append(NO_SPECIFIC_VALUE);
                        cronExpression.Append(BLANK);
                        cronExpression.Append(StartDate.Month);
                        cronExpression.Append(BLANK);

                        //TODO: this has to be changed in order to support multiple days of week
                        cronExpression.Append(Enum.GetName(typeof(DayOfWeek), DaysOfWeek).Substring(0, 3).ToUpper());
                        if (OrdinalDayOfTheWeek == 6)
                        {
                            cronExpression.Append("L");
                        }
                        else
                        {
                            cronExpression.Append("#");
                            cronExpression.Append(OrdinalDayOfTheWeek);
                        }

                        cronExpression.Append(BLANK);
                        cronExpression.Append(ASTERISK + PATH_SEPARATOR + frequency);

                        break;
                }

                return cronExpression.ToString();
            }

            return "";
        }

        public List<EventRepetition> GetRepetitions(EventViewModel eventViewModel)
        {
            //Quartz setup
            var tempTriggerName = "TEMP:" + new Random().NextDouble();
            var cronTrigerEnd = TriggerBuilder.Create()
                .WithIdentity(
                    new TriggerKey(tempTriggerName, eventViewModel.UserID.ToString()))
                .WithSchedule(
                    CronScheduleBuilder.CronSchedule(eventViewModel.CronExpression))
                .StartAt(StartDate);
            var trigger2 = cronTrigerEnd.EndAt(((DateTime)eventViewModel.EndBy).AddDays(1)).Build();


            var repetitions = new List<EventRepetition>();
            DateTime dtNext = eventViewModel.StartDate;
            var eventDuration = eventViewModel.EndDate.TimeOfDay -
                                eventViewModel.StartDate.TimeOfDay;

            //Special case for Weekly with certain frequency. 
            //Example 1: every other Tuesday
            //Example 2, every 3 weeks, on Wednesdays
            if (eventViewModel.FrequencyRule == WEEKLY && eventViewModel.Frequency > 0)
            {
                int iteration = 1;
                while (dtNext < eventViewModel.EndBy)
                {
                    var dateTimeOffset = trigger2.GetFireTimeAfter(dtNext);

                    if (dateTimeOffset != null)
                    {
                        dtNext = ((DateTimeOffset)dateTimeOffset).DateTime;

                        if (iteration % Frequency == 0)
                        {
                            var start = new DateTime(
                                dtNext.Year,
                                dtNext.Month,
                                dtNext.Day,
                                eventViewModel.StartDate.Hour,
                                eventViewModel.StartDate.Minute,
                                eventViewModel.StartDate.Second);

                            var end = start.AddTicks(eventDuration.Ticks);

                            repetitions.Add(new EventRepetition
                            {
                                StartDate = dtNext,
                                EndDate = end
                            });
                        }
                        iteration++;
                        continue;
                    }
                    break;
                }
            }
            else
            {
                while (dtNext < EndBy)
                {
                    var dateTimeOffset = trigger2.GetFireTimeAfter(dtNext);

                    if (dateTimeOffset != null)
                    {
                        dtNext = ((DateTimeOffset)dateTimeOffset).DateTime;

                        var start = new DateTime(
                            dtNext.Year,
                            dtNext.Month,
                            dtNext.Day,
                            eventViewModel.StartDate.Hour,
                            eventViewModel.StartDate.Minute,
                            eventViewModel.StartDate.Second);

                        var end = start.AddTicks(eventDuration.Ticks);

                        repetitions.Add(new EventRepetition
                        {
                            StartDate = start,
                            EndDate = end
                        });

                        continue;
                    }
                    break;
                }
            }

            return repetitions;
        }

    }
}