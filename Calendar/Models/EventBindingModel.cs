using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace Calendar.Models
{

    public class EventBindingModel : EventBaseModel
    {
        /// <summary>
        /// This is the end date for the repetitions when the event is recurrent. 
        /// The iterator to generate repetitions will go on until it exceeds this date
        /// </summary>
        public DateTime? EndBy { get; set; }

        /// <summary>
        /// Indicates the frequency rule used to calculate future repetitions. It's mandatory when the event is recurrent <br /><br/>
        /// <b>1 - WEEKLY</b> <br/><br/>
        /// The event is repeated weekly (Frequency=1) or every certain weeks (Frenquency=n). When using this rule
        /// DayOfWeek property must be specified
        /// <br/>
        /// Example 1: Every Wednesday <br/>
        ///   "Recurrent": true <br/>
        ///   "FrequencyRule": 1 <br/>
        ///   "Frequency": 1 <br/>
        ///   "DayOfWeek":3 <br/>
        ///<br/>
        /// Example 2: Every other Tuesday <br/>
        ///   "Recurrent": true <br/>
        ///   "FrequencyRule": 1 <br/>
        ///   "Frequency": 2 <br/>
        ///   "DayOfWeek":2 <br/>
        ///---------------------------------
        ///<br/><br/>
        /// <b>2 - MONTHLY EVERY DAY OF THE MONTH</b> <br/><br/>
        /// The event is repeated every day of a month, every month (Frequency=1) or 
        /// every certain months (Frenquency=n). The day and day time are taken from StarDate property
        /// <br/><br/>
        /// Example 1: Every 5th of every month at 1 pm <br/>
        ///   "StarDate": "2016-09-05T13:00:00" <br/>
        ///   "Recurrent": true <br/>
        ///   "FrequencyRule": 2 <br/>
        ///   "Frequency": 1 <br/>
        ///<br/>
        /// Example 2: Every 15th of the month, every 3 months <br/>
        ///   "StarDate": "2016-09-15" <br/>
        ///   "Recurrent": true <br/>
        ///   "FrequencyRule": 1 <br/>
        ///   "Frequency": 3 <br></br>
        ///---------------------------------
        ///<br/><br/>
        /// <b>3 - MONTHLY EVERY DAY OF THE WEEK </b> <br />  
        /// The event is repeated every specific day of the week, every month (Frequency=1) or 
        /// every certain months (Frenquency=n). When using this rule DayOfWeek and OrdinalDayOfTheWeek properties
        /// must be specified.
        /// <br/><br/>
        /// Example 1: The first Tuesday of every month<br/>
        ///   "Recurrent": true <br/>
        ///   "FrequencyRule": 3 <br/>
        ///   "Frequency": 1 <br/>
        ///   "DayOfWeek": 2  <br/>
        ///   "OrdinalDayOfTheWeek": 1 <br/>
        ///<br/>
        /// Example 2: The last Saturday of every month <br/>
        ///   "Recurrent": true <br/>
        ///   "FrequencyRule": 3 <br/>
        ///   "Frequency": 1 <br/>
        ///   "DayOfWeek": 6  <br/>
        ///   "OrdinalDayOfTheWeek": 6 <br/>
        ///<br/>
        /// Example 3: The 2nd Friday of the month, every 3 months <br/>
        ///   "Recurrent": true <br/>
        ///   "FrequencyRule": 3 <br/>
        ///   "Frequency": 3 <br/>
        ///   "DayOfWeek": 5  <br/>
        ///   "OrdinalDayOfTheWeek": 2 <br/>
        ///---------------------------------
        ///<br/><br/>
        /// <b>4 - ANUALLY EVERY DAY OF THE MONTH</b> <br/><br/>
        /// The event is repeated every day of a month, every year (Frequency=1) or 
        /// every certain years (Frenquency=n). The month, day and day time are taken from StarDate property
        /// <br/><br/>
        /// Example 1: The 23rd of February every year<br/>
        ///   "StarDate": "2016-02-23" <br/>
        ///   "Recurrent": true <br/>
        ///   "FrequencyRule": 4 <br/>
        ///   "Frequency": 1 <br/>
        ///<br/>
        /// Example 2: Every 15th of April at 9 am, every 2 years <br/>
        ///   "StarDate": "2016-04-15T09:00:00" <br/>
        ///   "Recurrent": true <br/>
        ///   "FrequencyRule": 1 <br/>
        ///   "Frequency": 2 <br></br>
        ///---------------------------------
        ///<br/><br/>
        /// <b>5 - ANUALLY EVERY DAY OF THE WEEK </b> <br />  
        /// The event is repeated every specific day of the week, every year (Frequency=1) or 
        /// every certain years (Frenquency=n). When using this rule DayOfWeek and OrdinalDayOfTheWeek properties
        /// must be specified. The month is taken from StartDate month
        /// <br/><br/>
        /// Example 1: The third Tuesday in February every year<br/>
        ///   "StarDate": "2016-02-28" <br/>
        ///   "Recurrent": true <br/>
        ///   "FrequencyRule": 5 <br/>
        ///   "Frequency": 1 <br/>
        ///   "DayOfWeek": 2  <br/>
        ///   "OrdinalDayOfTheWeek": 3 <br/>
        ///<br/>
        /// Example 2: The last Thurday in November, every 3 years <br/>
        ///   "StarDate": "2016-11-01" <br/>
        ///   "Recurrent": true <br/>
        ///   "FrequencyRule": 5 <br/>
        ///   "Frequency": 3 <br/>
        ///   "DayOfWeek": 4  <br/>
        ///   "OrdinalDayOfTheWeek": 6 <br/>
        ///<br/>
        ///</summary>
        [Range(1, 5)]
        public int? FrequencyRule { get; set; }

        /// <summary>
        /// Frequency in which repetitions will be generated. If it's not specified it assumes 1 by default <br/> <br/> 
        /// Exmaple 1: if the FrequencyRule=1 (WEEKLY) and Frequency=2 means that the event is 
        /// repeated every 2 weeks<br/><br/>
        /// Exmaple 2: if the FrequencyRule=3 (MONTHLY EVERY DAY OF THE MONTH) and Frequency=6 
        /// means that the event is repeated every 6 months<br/><br/>
        /// Exmaple 3: if the FrequencyRule=4 (ANUALLY EVERY DAY OF THE MONTH) and Frequency=3 
        /// means that the event is repeated every 3 years<br/><br/>
        /// </summary>
        [Range(0, Int32.MaxValue)]
        public int? Frequency { get; set; }


        /// <summary>
        /// Indicates the day of the week when the frequency rule is WEEKLY, MONTHLY EVERY DAY OF THE WEEK
        /// or ANUALLY EVERY DAY OF THE WEEK. Values:<br/><br/>
        /// 0 = Sunday <br/>
        /// 1 = Monday <br/> 
        /// 2 = Tuesday <br/>
        /// 3 = Wednesday <br/>
        /// 4 = Thursday <br/>
        /// 5 = Friday <br/>
        /// 6 = Saturday
        /// </summary>
        [Range(0, 6)]
        public int? DayOfWeek { get; set; }

        /// <summary>
        /// Indicates ordinal day of week within a month when the frequency rule is MONTHLY EVERY DAY OF THE WEEK
        /// or ANUALLY EVERY DAY OF THE WEEK. Values:<br/><br/>
        /// 1 = First day of week<br/>
        /// 2 = Second day of week<br/> 
        /// 3 = Third day of week <br/>
        /// 4 = Fourth day of week <br/>
        /// 5 = Fifth day of week<br/>
        /// 6 = Last day of week<br/><br/>
        /// Example 1: fist Tuesday of the month <br/>
        ///     "DayOfWeek":2, <br/>
        ///     "OrdinalDayOfTheWeek":1 <br/>
        /// <br/>    
        ///Example 2: last Monday of the month <br/>
        ///     "DayOfWeek":1, <br/>
        ///     "OrdinalDayOfTheWeek":6 <br/>
        /// <br/>
        ///Example 3: third Friday of the month <br/>
        ///     "DayOfWeek":5, <br/>
        ///     "OrdinalDayOfTheWeek":3 <br/>
        /// <br/>
        /// </summary>
        [Range(1, 6)]
        public int? OrdinalDayOfTheWeek { get; set; }


        public string GetCronExpression()
        {
            //Test http://www.cronmaker.com/
            if (Recurrent)
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
                        cronExpression.Append(Enum.GetName(typeof(DayOfWeek), DayOfWeek).Substring(0, 3).ToUpper());
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
                        cronExpression.Append(Enum.GetName(typeof(DayOfWeek), DayOfWeek).Substring(0, 3).ToUpper());
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
                        cronExpression.Append(StartDate.Month);
                        cronExpression.Append(BLANK + NO_SPECIFIC_VALUE + BLANK);
                        cronExpression.Append(ASTERISK + PATH_SEPARATOR + frequency);

                        break;

                    case ANUALLY_EVERY_DAY_OF_THE_WEEK:
                        cronExpression.Append(NO_SPECIFIC_VALUE);
                        cronExpression.Append(BLANK);
                        cronExpression.Append(StartDate.Month);
                        cronExpression.Append(BLANK);

                        //TODO: this has to be changed in order to support multiple days of week
                        cronExpression.Append(Enum.GetName(typeof(DayOfWeek), DayOfWeek).Substring(0, 3).ToUpper());
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

    }
}