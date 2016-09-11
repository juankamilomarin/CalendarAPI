using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

namespace Calendar.Models
{
    public class UserViewModel
    {
        public int UserID { get; set; }
        public string Name { get; set; }

        public UserViewModel(User user)
        {
            this.UserID = user.UserID;
            this.Name = user.Name;
        }

    }
}