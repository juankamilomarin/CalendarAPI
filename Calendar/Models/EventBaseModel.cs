using System;
using System.ComponentModel.DataAnnotations;

namespace Calendar.Models
{
    public abstract class EventBaseModel
    {

        //public const int DAYLY = 0;
        public const int WEEKLY = 1;
        public const int MONTHLY_EVERY_DAY_OF_THE_MONTH = 2;
        public const int MONTHLY_EVERY_DAY_OF_THE_WEEK = 3;
        public const int ANUALLY_EVERY_DAY_OF_THE_MONTH = 4;
        public const int ANUALLY_EVERY_DAY_OF_THE_WEEK = 5;

        protected const int WEEK_DAYS = 7;
        protected const int YEAR_MONTHS = 12;

        protected const string NO_SPECIFIC_VALUE = "?";
        protected const string PATH_SEPARATOR = "/";
        protected const string ASTERISK = "*";
        protected const string BLANK = " ";


        /// <summary>
        /// Name of the event 
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Location of the event
        /// </summary>
        [Required]
        public string Location { get; set; }

        /// <summary>
        /// Notes of the event
        /// </summary>
        public string Notes { get; set; }


        /// <summary>
        /// Start Date of the event
        /// </summary>
        /// <value>2016-09-12T17:00:00</value>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End Date of the event
        /// </summary>
        /// <value>2016-09-12T18:00:00</value>
        [Required]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Indicates if the event is recurrent (has repetitions in the future).
        /// <br/><br/>
        /// When an event is not recurrent FrequencyRule, Frequency, EndBy, DayOfWeek and OrdinalDayOfWeek
        /// properties are ignored.
        /// <br/><br/>
        /// When an event is recurrent the API calculates the repetitions of such event based on a FrequencyRule.
        /// It stars to itarate from StartDate until EndBy and creates a list of EventRepetition. Quartz library
        /// is used to accomplished this purpose. <br/>
        /// More info: <a href="http://www.quartz-scheduler.net/" target="_blank">Quartz.NET - Quartz Enterprise Scheduler.NET</a>
        /// </summary>
        [Required]
        public bool Recurrent { get; set; }

        

    }
}