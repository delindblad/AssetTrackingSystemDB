# AssetTrackingSystem
## Description
System for tracking assets in different locations
## Features
- Simple to use interactive interface
- Persistence, can save a list to a .json file and restore it on startup
- Automatic currency conversion via online API
## Technologies
- .NET
- C#
- LINQ
- JSON
- Free currency API
- Entity
- Spectre console: https://spectreconsole.net/
## Installation
Clone the repository:

	git clone https://github.com/delindblad/AssetTrackingSystemDB.git

Enter the project sub directory, it you use Windows:

	cd .\AssetTrackingSystemDB\AssetTrackingSystem

On MacOs or other UNIX-like systems:

	cd ./AssetTrackingSystemDB/AssetTrackingSystem 

You need to provide a connection string by setting the CONNECTION_STRING environment variable temporarily. The exact string depends on your particular setup, below is an example

If you use Windows Power Shell:

	setx CONNECTION_STRING "Server=myServerAddress;Database=myDatabase;User Id=myUsername;Password=myPassword;"

on MacOs or other UNIX-like systems, using sh, zsh or bash:

	export CONNECTION_STRING="Server=myServerAddress;Database=myDatabase;User Id=myUsername;Password=myPassword;"

or if you use csh or tcsh:

	setenv CONNECTION_STRING "Server=myServerAddress;Database=myDatabase;User Id=myUsername;Password=myPassword;"

set up migrations:

	dotnet ef migrations add InitialCreate
	dotnet ef database update

to create sample data run with the "init" parameter:

	dotnet run AssetTrackingSystem.csproj init

the program will exit after the data has been created, you can then run it with the following command:

	dotnet run AssetTrackingSystem.csproj
		


## How to run
Will run automatically as described above, or just run the executable after building it:

	dotnet build AssetTrackingSystem.csproj

the executable will be under:

	./bin/Debug/net10.0/
	

## Screenshots

<img width="858" height="366" alt="image" src="https://github.com/user-attachments/assets/5f990da7-c069-49f5-ae80-0373a61e3685" />
<img width="852" height="478" alt="image" src="https://github.com/user-attachments/assets/4acb632d-5cf4-437d-b01f-5e51357dbc9e" />




## Team members
Only me.


## Future improvements
Lots to be done, supporting users, stats, filtering...






  
