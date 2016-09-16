using Calendar.Models;
using Model;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Calendar.Controllers
{
    public class UsersController : ApiController
    {
        private CalendarContext _calendarDBContext = null;

        public CalendarContext DbContext
        {
            get
            {
                if (_calendarDBContext == null)
                {
                    _calendarDBContext = new CalendarContext();
                }

                _calendarDBContext.Configuration.ProxyCreationEnabled = false;
                return _calendarDBContext;
            }
        }

        /// <summary>
        /// Gets the list of existing registered users
        /// </summary>
        [Route("api/users")]
        [HttpGet, ActionName("GetAllUsers")]
        [ResponseType(typeof(IEnumerable<UserViewModel>))]
        public IHttpActionResult GetUsers()
        {
            IEnumerable<UserViewModel> usersVM = DbContext.Users.ToList().
                Select(user => new UserViewModel
                {
                    UserID = user.UserID,
                    Name = user.Name
                });
            return Ok(usersVM);
        }


        /// <summary>
        /// Gets the properties of a specific user
        /// </summary>
        /// <param name="UserID">ID of the user</param>
        [Route("api/users/{userID}")]
        [HttpGet, ActionName("GetUser")]
        [ResponseType(typeof(UserViewModel))]
        public IHttpActionResult GetUser(int userID)
        {
            User user = DbContext.Users.Find(userID);
            if (user != null)
                return Ok(new UserViewModel
                {
                    UserID = user.UserID,
                    Name = user.Name
                });
            return NotFound();
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="userBindingModel"></param>
        [Route("api/users")]
        [HttpPost, ActionName("PostUser")]
        [ResponseType(typeof(UserViewModel))]
        public IHttpActionResult PostUser([FromBody] UserBindingModel userBindingModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User user = new User { Name = userBindingModel.Name };
            DbContext.Users.Add(user);
            DbContext.SaveChanges();
            return Ok(new UserViewModel { UserID = user.UserID, Name = user.Name });
        }

        /// <summary>
        /// Gets the events of a given user
        /// </summary>
        /// <param name="userID">ID of the user</param>
        [Route("api/users/{userID}/events")]
        [HttpGet, ActionName("GetAllUserEvents")]
        [ResponseType(typeof(IEnumerable<EventViewModel>))]
        public IHttpActionResult GetUserEvents(int userID)
        {
            IEnumerable<EventViewModel> events = DbContext.Events.Where(e => e.UserID == userID).ToList().Select(e => new EventViewModel(e));
            return Ok(events);
        }

        /// <summary>
        /// Gets a specific event of a given user
        /// </summary>
        /// <param name="userID">ID of the user</param>
        /// <param name="eventID">ID of the specific event</param>
        [Route("api/users/{userID}/events/{eventID}")]
        [HttpGet, ActionName("GetUserEvent")]
        [ResponseType(typeof(EventViewModel))]
        public IHttpActionResult GetUserEvents(int userID, int eventID)
        {
            Event userEvent = DbContext.Events.FirstOrDefault(e => e.UserID == userID && e.EventID == eventID);
            if (userEvent != null) return Ok(new EventViewModel(userEvent));
            return NotFound();
        }


        /// <summary>
        /// Creates a new event for a specific user
        /// </summary>
        /// <param name="userID">ID of the user</param>
        /// <param name="eventBindingModel">Object with the information of the event</param>
        [Route("api/users/{userID}/events")]
        [HttpPost, ActionName("PostUserEvent")]
        public IHttpActionResult PostUserEvent(int userID, [FromBody]EventBindingModel eventBindingModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string recurrenceValidationMessage = ValidateBindingProperties(eventBindingModel);
            if (recurrenceValidationMessage != null)
            {
                return BadRequest(recurrenceValidationMessage);
            }

            Event userEvent = new Event
            {
                UserID = userID,
                Name = eventBindingModel.Name,
                Location = eventBindingModel.Location,
                Notes = eventBindingModel.Notes,
                StartDate = eventBindingModel.StartDate,
                EndDate = eventBindingModel.EndDate,
                Recurrence = eventBindingModel.Recurrent,
                FrequencyRule = eventBindingModel.FrequencyRule,
                Frequency = eventBindingModel.Frequency,
                EndBy = eventBindingModel.EndBy,
                DaysOfWeek = eventBindingModel.DayOfWeek.ToString(),
                OrdinalDayOfTheWeek = eventBindingModel.OrdinalDayOfTheWeek,
                CronExpression = eventBindingModel.GetCronExpression(),
                State = true

            };

            DbContext.Events.Add(userEvent);
            DbContext.SaveChanges();

            return Ok(new { EventID = userEvent.EventID });
        }

        /// <summary>
        /// Updates a user event
        /// </summary>
        /// <param name="userID">ID of the user</param>
        /// <param name="eventID">ID of the specific event to update. The API ignores the EventID property</param>
        /// specified in the UverViewModel</param>
        /// <param name="userEventViewModel">Object with the information of the event</param>
        [Route("api/users/{userID}/events/{eventID}")]
        [HttpPut, ActionName("PuttUserEvent")]
        public IHttpActionResult PutUserEvent(int userID, int eventID, [FromBody]EventBindingModel userEventViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string recurrenceValidationMessage = ValidateBindingProperties(userEventViewModel);
            if (recurrenceValidationMessage != null)
            {
                return BadRequest(recurrenceValidationMessage);
            }

            Event userEvent = DbContext.Events.FirstOrDefault(e => e.UserID == userID && e.EventID == eventID);
            if (userEvent == null) return NotFound();

            userEvent.Name = userEventViewModel.Name;
            userEvent.Location = userEventViewModel.Location;
            userEvent.Notes = userEventViewModel.Notes;
            userEvent.StartDate = userEventViewModel.StartDate;
            userEvent.EndDate = userEventViewModel.EndDate;
            userEvent.Recurrence = userEventViewModel.Recurrent;
            userEvent.FrequencyRule = userEventViewModel.FrequencyRule;
            userEvent.Frequency = userEventViewModel.Frequency ?? 1;
            userEvent.DaysOfWeek = userEventViewModel.DayOfWeek.ToString(); //It's store as string so in the future one could extend to use multiple days of the week
            userEvent.OrdinalDayOfTheWeek = userEventViewModel.OrdinalDayOfTheWeek;
            userEvent.CronExpression = userEventViewModel.GetCronExpression();
            userEvent.EndBy = userEventViewModel.EndBy;

            DbContext.SaveChanges();
            return Ok();
        }


        /// <summary>
        /// Deletes a specific user event
        /// </summary>
        /// <param name="userID">ID of the user</param>
        /// <param name="eventID">ID of the specific event</param>
        [Route("api/users/{userID}/events/{eventID}")]
        [HttpDelete]
        public IHttpActionResult DeleteUserEvent(int userID, int eventID)
        {
            Event userEvent = DbContext.Events.FirstOrDefault(e => e.UserID == userID && e.EventID == eventID);
            if (userEvent == null) return NotFound();
            userEvent.State = false;
            DbContext.SaveChanges();
            return Ok();
        }

        //TODO: migrate this to model state valiations
        private string ValidateBindingProperties(EventBindingModel userEventViewModel)
        {
            if(userEventViewModel.StartDate > userEventViewModel.EndDate)
            {
                return "StartDate is bigger then EndDate";
            }
            if(userEventViewModel.EndBy < userEventViewModel.EndDate)
            {
                return "EndDate is bigger than EndBy";
            }
            if (userEventViewModel.Recurrent)
            {
                if (userEventViewModel.FrequencyRule == null) return "FrequencyRule is not specified";

                //Frequency property could be null (default=1). This means that it repeats every period (specified in FrequencyRule).
                //i.e. WEEKLY with Frequency null (or 1) means that it happens every week. WEEKLY with Frequency 2 means every other week
                //if (userEventViewModel.Frequency == null) return "Frequency is not specified";

                if (userEventViewModel.EndBy == null) return "EndBy is not specified";

                if (userEventViewModel.EndBy < userEventViewModel.EndDate) return "EndBy Date is earlier than EndDate";

                if (userEventViewModel.FrequencyRule == EventViewModel.WEEKLY
                    || userEventViewModel.FrequencyRule == EventViewModel.MONTHLY_EVERY_DAY_OF_THE_WEEK
                    || userEventViewModel.FrequencyRule == EventViewModel.ANUALLY_EVERY_DAY_OF_THE_WEEK)
                {
                    if (userEventViewModel.DayOfWeek == null) return "DaysOfWeek not specified";

                    if (userEventViewModel.FrequencyRule == EventViewModel.MONTHLY_EVERY_DAY_OF_THE_WEEK
                    || userEventViewModel.FrequencyRule == EventViewModel.ANUALLY_EVERY_DAY_OF_THE_WEEK)
                    {

                        if (userEventViewModel.OrdinalDayOfTheWeek == null) return "OrdinalDayOfTheWeek not specified";
                    }
                }
            }
            return null;
        }
    }
}
