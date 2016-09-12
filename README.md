# CalendarAPI
This is a ASP.NET Web API project that handles calendar events of users. The main resourse is users, and you can create events for each user

## Motivation

First coding challenge for Azend application

## Installation

1. Download the project and open it on Visual Studio 2015
2. Test!

## Tests

<b>How to get the registered users:</b>
<br/>
Action: GET<br/>
URI: /api/users<br/>
<br/>
<b>How to create a user:</b>
<br/>
Action: POST<br/>
URI: /api/users<br/>
BODY: <br/>
```javascript
  {
    "Name": "Jeff"
  }
```
<b>How to create a new event that is repeted every week:</b>
<br/>
Action: POST<br/>
URI: /api/users/{UserID}/events<br/>
BODY:<br/>
```javascript
{
  "Name": "Test post 2",
  "Location": "Via Skype",
  "Notes": "Some notes",
  "StartDate": "2016-09-12T17:00:00",
  "EndDate": "2016-09-12T18:00:00",
  "Recurrent": true,
  "EndBy": "2016-10-12T18:00:00",
  "FrequencyRule": 1,
  "Frequency": 1,
  "DayOfWeek": 3
}
```
<b>How to get the events of a specific user</b>
<br/>
Action: GET<br/>
URI: /api/users/{UserID}/events<br/>
<br/>
<b>How to get a specific events from an user</b>
<br/>
Action: GET<br/>
URI: /api/users/{UserID}/events/{EventID}<br/>
<br/>
<b>You may find more examples exporting this postman collection (<a href="https://www.getpostman.com/" target="_blank">Get postman</a>):</b>
<br/>
https://www.getpostman.com/collections/32cbe58c505c14ff2996

## Contributor

Juan Camilo Marin
