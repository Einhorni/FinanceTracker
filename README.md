# Finance-Tracker C# Learning Project

There are lots of printables and excel tables for budgeting and keeping an overview of ones finances, incomes and expenses.
Although they are great, I want to write a budgeting app (not a banking app) that does the same but is way less complicated to maintain and use for user.

### Simple Finance Tracker (console app)
* Evolved from storing data in a text file to storing data in a local database
* Add accounts and saving them
* Transfer money between two accounts and save it as transactions 
* Make transcactions (expenses & income) and save them
* Load accounts and transactions and display them
* Negative Transaction validation (not below 0)

### Planned
* Write tests
* Seperate DAL from BLL
* Create a MVC web app for easier handling


### Programming concepts included 
* Basic sytnax and classes, inheritance
* Switches, if-statements and loops
* IO with try/catch blocks, repositories, interfaces
* Dependency injection, writing a Service
* Seperated UI from a combined Logic & DataAccess Layer which is only accessible through a service
* Implemented a database with async functionality
* Used extension methods for mapping
* Global Error Handling and Logger (last is overkill but I wanted to try it in a console application rather than a ASP.NET Core app)
