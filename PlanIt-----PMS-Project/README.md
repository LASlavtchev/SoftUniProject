# PlanIt-----PMS-Project
Web project for SoftUni defense of the C# Web course

Description - Project Management System: invite only.
Using Admin LTE template for UI, SendGrid /ApiKey has to be configured for security reasons/, Hangfire.

Functionality:
Areas - Administration, Client, Management
1. Identity: roles - Company owner, Administrator, Client and User.

	1.1. Company owner is first registered user upon initialization of the system /Super User/. He has access to all 	functionality.
	
	1.2. Administrator can manage only users and invites.
  
	1.3. Project Manager - can manage all projects and promote other users to have ability to be a project manager of a specific	projects, can assign tasks to other users for his projects.
  
	1.4. Users - if is promoted to manager, can manage all assigned to him projects, otherwise can manage only the tasks assigned to him.
  
	1.5. Client - can manage only his projects by approving budget and due date
	
2. Invites - users can register to the system only if he is invited. Upon invite request he gains the ability to register to the system only by email. The administrator gives him invite and promote him to the relevant role.

3. Projects - can be created by clients, managers and global managers. One project has many addtional costs and subprojects. Project`s budget is calculated on base of the budgets of all subprojects assigned.

4. SubProjects - can be created for current project by the assigned manager or global managers. One subproject has many problems and addirional costs.

5. AdditionalCosts - costs which are done for something other than problems.

6. Problems /Tasks/ - can be assigned to the users by the relevant manager or the global managers. On completion are calculated costs depending on the hourly rate and the total spent hours for the task.

7. TasksHour - gives information about how many hours are spent on given task for the relevant subproject of the project. Users can gain this information only for their assigned tasks, assigned manager only for his projects and global manager for all.

8. ProgressStatuses are informative.

	Workflow:
	To register to the system you have to send invite request with purpose. The request is valid 24h.
	The administrator approve your request and decide your role upon your purpose or you can have an invite by him.
	Upon approved invitation you recieve email with the registration form. Sent invatioan also is valid 24h.
	Administrator has rights to extend your invitation by 24h or to decline.
	
	As a client you make request for project with due date and budget.
	Global project manager assign a project manager to the project who make an offer which the client has to approve.
	The manager create all subprojects and assihn tasks to the users.
	
	When all tha tasks for the current subproject become completed the subproject gain status completed.
	A project is completed when all assigned subprojects are completed.
	
	On the dashboard relevent to the roles can be seen all the calculated information
	about the total count of the users, clients, projects, budgets, costs and profit.
