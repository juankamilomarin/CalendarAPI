using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;
using System.ComponentModel.DataAnnotations;

namespace Calendar.Models
{
    public class UserViewModel
    {
        /// <summary>
        /// Unique number that identifies the user </summary>
        /// <value>6</value>
        public int UserID { get; set; }
        /// <summary>
        /// Name of the user </summary>
        /// <value></value>
        [Required]
        public string Name { get; set; }

        public UserViewModel(User user)
        {
            this.UserID = user.UserID;
            this.Name = user.Name;
        }

        public UserViewModel()
        {

        }

    }
}