# StaffCircle


To create the database run the SQL file SMS.sql in RepositoryCore in SQL-Server (min version 2014)

RepositoryCore contains the code to access the SMS database.

Within the api project the SMSController contains the code necessary to post an SMS to the required number.

I tried doing this from the Vue application using aa AJAX call to the Post method on SMSController.  I had problems with CORS and 
WDS headers which I was unable to resolve.  To test the WebApi and used the PostMan application and did a post to:

https://localhost:44391/api/sms

The body of the request was set up as follows:

{
    "number": "07xxxxxxxxx",
    "message": "Message Here"
}
