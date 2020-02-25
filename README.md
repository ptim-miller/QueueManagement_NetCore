# Queue Management System
(Functional prototype of queue management system written in C# .Net Core 2.0)
2016/17 project - requires library updates

## Emergency-Department-Check-In
Emergency Dept Kiosk Application with Admin Station

## Technical  
C#/.Net Core 2.0  
PostgreSQL 9.6 (backup in Data)  
 
**Required**  
FHIR API (setup in appsettings.json)  
Mail Server (setup in appsettings.json)  
PostgreSQL 9.6 (setup in appsettings.json)  
 
**IDE**  
In Visual Studio 2017, load project with QMS.sln. Packages managed with Nuget and Bower.  
 
 **Migration**  
Restore QMS.tar to empty QMS database or create new tables with migration commands:  
* update-database -c ApplicationDbContext
* update-database -c PatientDbContext

If changes are made to patient or encounters classes, use:  
* Add-Migration Next -c PatientDbContext
* update-database -c PatientDbContext

**.Net Core Setup**
* See: https://www.microsoft.com/net/core#linuxubuntu
* Other OS instructions are available through link
* In desired OS, install .Net Core Version 2
* From QMS folder enter: dotnet publish
* Enter: dotnet run

**TODO Tasks**
* Add paging to Lobby Screen, Search and Patient Status.  
* Change authorization process to applicable organization or create roles for admin control of user additions.  
* Modify presentation layer to match host organization theme/graphics.  
* Modify FHIR server link and parameters in appsettings.json to applicable server.  
* Change state entry on patient contact screen to dropdown list.  

## Check-in and Waitlist Management
 
**Introduction**  
The check-in waitlist management system was designed to provide kiosk check-in, patient status on a lobby display and patient waitlist management for the Emergency Room. Patients do not login or have access to any data once they complete the initial check-in process. They simply provide enough data, described in more detail below, to secure a place on the waitlist. The patient is notified on their smart device when they successfully check-in, once their room is ready, and once their visit is complete.  

**Kiosk Check-In**  
The kiosk can be equipped with a tablet, Chromebook or small laptop to allow low acuity patients a self-service style check-in. The kiosk screen is best displayed in full-screen mode (F11 Firefox/Chrome).  
 
 **Lobby Screen**  
Lobby screen functionality is available in the system if a large screen or other display is available. The purpose of the lobby screen is to allow patients to determine their status with their patient number. No personal details are displayed on this screen.  When used, lobby screen is best displayed in full-screen mode (F11 Firefox/Chrome).  Ctrl+- allows screen resizing as well. Patients are listed by their patient ID to protect their privacy. Only active patients (waiting, notified or arrived) are shown on the lobby screen.  
 
 **Registration Desk**  
The registration desk is for use by authorized personnel only. It requires a login to access any patient data. A login is also required to add users to the system.  Once logged into the system, additional menu items are visible. Company personnel can also add patients to the queue through this system, and they can add new visits for previous patients as well.  
 
Patient status can be updated to reflect several different status items from the Edit screen. For example, if a patient decides to go to an alternative department, such as a Walk-In center, the registration desk can go to the Edit Screen and modify the patient status to cancelled. This removes them from the active queue. If they return, their status can be updated to waiting if desired. It is important to note that patients are removed from patient status if finished or cancelled, so their patient information must be searched from the Search item on the top menu.  
 
Through “Msg”, authenticated users can send notifications to waiting patients or patients who are about to leave the hospital. If a patient is in “waiting status”, once the message button is selected, the authenticated user is presented with a prepopulated notification screen. The message text can be modified if needed. If the Send button is click, the patient is notified that their room is ready and their status is automatically changed to notified.  
 
Once the patient arrives, their status should manually be changed to “Arrived” from the Edit screen. This will make the Msg button visible again. Just before the patient leaves the hospital, the message button can be clicked which will present the authenticated user with a final notification screen. This sends the extended final message to the patient. Once the message is sent, the patient is automatically updated to “Finished” status and removed from the Patient Status screen.  
 
**Search**  
Previous patient visits can be located in the system through the Search item on the top menu. Enter part or all of the patient's last name, first name or email address. The search items are not case sensitive. Matching items are presented on the screen with applicable option button. Dates can be searched on their own or as part of the other criteria. Date entry items all contain calendar pop-ups for ease of date selection. When searching by date, several more items are listed on the bottom section of the screen. If a patient has a finalized date listed, this means that they had a previous visit and were subsequently moved to cancelled or finished status.  
 
Patients listed as cancelled or finished can have a new visit added to the system through the Edit button. On selection, the edit screen is shown with a new button item, "Create New Visit." Selecting the Create New Visit button brings up the patient registration screen with most data prepopulated. The authenticated user can update patient data manually as they enter the information on behalf of the patient. Patients are not allowed to retrieve their own data through the kiosk system. Only authenticated users have access to existing patient information.   
