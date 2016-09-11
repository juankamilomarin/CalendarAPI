using Calendar.Models;
using Model;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Calendar.Controllers
{
    public class UsersController : ApiController
    {
        private CalendarContext _calendarContext = null;

        public CalendarContext DbContext
        {
            get
            {
                if (_calendarContext == null)
                {
                    _calendarContext = new CalendarContext();
                }

                _calendarContext.Configuration.ProxyCreationEnabled = false;
                return _calendarContext;
            }
        }

        // GET api/users
        [Route("api/users")]
        [HttpGet]
        public IHttpActionResult GetUsers()
        {
            //TODO: get all users
            IEnumerable<UserViewModel> users = DbContext.Users.ToList().Select(u => new UserViewModel(u));
            if (users.Any()) return Ok(users);
            return NotFound();
        }



        // GET api/users/{userID}
        [Route("api/users/{userID}")]
        [HttpGet]
        public IHttpActionResult GetUser(int userID)
        {
            User user = DbContext.Users.Find(userID);
            if (user != null) return Ok(new UserViewModel(user));
            return NotFound();
        }

        // GET api/users/{userID}/events
        [Route("api/users/{userID}/events")]
        [HttpGet]
        public IHttpActionResult GetUserEvents(int userID)
        {
            IEnumerable<EventViewModel> events = DbContext.Events.Where(e => e.UserID == userID).ToList().Select(e => new EventViewModel(e));
            if (events.Any()) return Ok(events);
            return NotFound();
        }

        // GET api/users/{userID}/events/{eventId}
        [Route("api/users/{userID}/events/{eventID}")]
        [HttpGet]
        public IHttpActionResult GetUserEvents(int userID, int eventID)
        {
            Event userEvent = DbContext.Events.FirstOrDefault(e => e.UserID == userID && e.EventID == eventID);
            if (userEvent != null) return Ok(new EventViewModel(userEvent));
            return NotFound();
        }


        // POST api/users/{userID}/events
        [Route("api/users/{userID}/events")]
        public IHttpActionResult PostUserEvent(int userID, [FromBody]EventViewModel userEventViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string recurrenceValidationMessage = ValidateRecurrenceProperties(userEventViewModel);
            if (recurrenceValidationMessage != null)
            {
                return BadRequest(recurrenceValidationMessage);
            }

            Event userEvent = new Event
            {
                UserID = userID,
                Name = userEventViewModel.Name,
                Location = userEventViewModel.Location,
                Notes = userEventViewModel.Notes,
                StartDate = userEventViewModel.StartDate,
                EndDate = userEventViewModel.EndDate,
                Recurrence = userEventViewModel.Recurrence,
                FrequencyRule = userEventViewModel.FrequencyRule,
                Frequency = userEventViewModel.Frequency,
                EndBy = userEventViewModel.EndBy,
                DaysOfWeek = userEventViewModel.DaysOfWeek.ToString(),
                OrdinalDayOfTheWeek = userEventViewModel.OrdinalDayOfTheWeek,
                CronExpression = userEventViewModel.GetCronExpression(),
                State = true

            };

            DbContext.Events.Add(userEvent);
            DbContext.SaveChanges();

            return Ok(new { EventID = userEvent.EventID });
        }

        // PUT api/users/{id}/events
        [Route("api/users/{userID}/events/{eventID}")]
        public IHttpActionResult PutUserEvent(int userID, [FromBody]EventViewModel userEventViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string recurrenceValidationMessage = ValidateRecurrenceProperties(userEventViewModel);
            if (recurrenceValidationMessage != null)
            {
                return BadRequest(recurrenceValidationMessage);
            }

            Event userEvent = DbContext.Events.FirstOrDefault(e => e.UserID == userID && e.EventID == userEventViewModel.EventID);
            if (userEvent == null) return NotFound();

            userEvent.Name = userEventViewModel.Name;
            userEvent.Location = userEventViewModel.Location;
            userEvent.Notes = userEventViewModel.Notes;
            userEvent.StartDate = userEventViewModel.StartDate;
            userEvent.EndDate = userEventViewModel.EndDate;
            userEvent.Recurrence = userEventViewModel.Recurrence;
            userEvent.FrequencyRule = userEventViewModel.FrequencyRule;
            userEvent.Frequency = userEventViewModel.Frequency??1;
            userEvent.DaysOfWeek = userEventViewModel.DaysOfWeek.ToString(); //It's store as string so in the future one could extend to use multiple days of the week
            userEvent.OrdinalDayOfTheWeek = userEventViewModel.OrdinalDayOfTheWeek;
            userEvent.CronExpression = userEventViewModel.GetCronExpression();
            userEvent.EndBy = userEventViewModel.EndBy;

            DbContext.SaveChanges();
            return Ok();
        }


        // DELETE api/users/{id}/events
        [Route("api/users/{userID}/events/{eventID}")]
        public IHttpActionResult DeleteUserEvent(int userID, int eventID)
        {
            Event userEvent = DbContext.Events.FirstOrDefault(e => e.UserID == userID && e.EventID == eventID);
            if (userEvent == null) return NotFound();
            userEvent.State = false;
            DbContext.SaveChanges();
            return Ok();
        }

        private string ValidateRecurrenceProperties(EventViewModel userEventViewModel)
        {
            if (userEventViewModel.Recurrence)
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
                    if (userEventViewModel.DaysOfWeek == null) return "DaysOfWeek not specified";

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
