"# ClientManagement" 
ApplicationCore   Project contains the interfaces and Data Classess

Infrastructure project contains the data contexts and CRUD logic( sql server based)

ClientsWeb  is the UI project has one controller that handles CRUD operations and a file download

ClientsWeb Assets folder has the DB  Backup and also the required scripts

Restore the bak file into your sqlserver and change the connection string in the appsettings.json and you should be good to go

This is a .net core web app 6.0 framework
