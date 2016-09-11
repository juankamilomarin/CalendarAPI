namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Event")]
    public partial class Event
    {
        public int EventID { get; set; }

        public int UserID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Location { get; set; }

        [StringLength(800)]
        public string Notes { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool? Recurrence { get; set; }

        public int? FrequencyRule { get; set; }

        public int? Frequency { get; set; }

        public DateTime? EndBy { get; set; }

        [StringLength(100)]
        public string DaysOfWeek { get; set; }

        public int? OrdinalDayOfTheWeek { get; set; }

        [StringLength(50)]
        public string CronExpression { get; set; }

        public bool State { get; set; }

        [Newtonsoft.Json.JsonIgnoreAttribute]
        public virtual User User { get; set; }
    }
}
