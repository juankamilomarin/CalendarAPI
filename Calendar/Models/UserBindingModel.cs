using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Calendar.Models
{
    public class UserBindingModel
    {
        /// <summary>
        /// Name of the user </summary>
        /// <value></value>
        [Required]
        public string Name { get; set; }
    }
}