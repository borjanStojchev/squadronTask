I've created this small server game using .Net Core Web Api and React as a frontend JS framework. Also I used SQLServer database. 
I've used EntityFramework framework and Repository pattern.

Steps for starting the application:
- Create a SQLServer database and provide the connection string in webApi/GameServerAPI/GameServerApi/appSettings.json
- navigate from terminal in the webApi/GameServerAPI/DataAccess and execute this command:
dotnet ef --startup-project ../GameServerAPI migrations add Init   
dotnet ef --startup-project ../GameServerAPI database update       
This will create the tables in the database.

After this the web api can be started.
For starting the react application navigate to the reactGame/mathgame and execute 'npm start'.

All the players can access the web entering the local Ip address on the 3000 port.

All the functions in the .Net Core API are documented.