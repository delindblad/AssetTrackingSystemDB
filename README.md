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
## Installation
Clone the repository:

	git clone https://github.com/delindblad/AssetTrackingSystem.git


In the project sub directory use "dotnet run", it should build automatically:
    Change to the project directory(its under the "solution directory":
    
    >cd .\AssetTrackingSystem\AssetTrackingSystem\R
    
  Run the project directly:
  
    >dotnet run AssetTrackingSystem.csproj
		

The resulting executable will be under "\bin\Release\net10.0\".

## How to run
Will run automatically as described above, or just run the executable. And follow the instructions.

## Screenshots

<img width="2299" height="1053" alt="image" src="https://github.com/user-attachments/assets/715d3574-0a7b-48ab-ad7f-1c33b21ff90f" />
<img width="2041" height="1059" alt="image" src="https://github.com/user-attachments/assets/b6a459f6-3a67-4b5d-a9f1-91e8576edb2d" />




## Team members
Only me.

## Notes
Serialization to .json of abstract classes seems to be unsupported, so I have not used polymorphism for the "Asset" type. I have however used it in other parts of the application related to the interface.

## Future improvements
Support for removing and modifying assets. Automatic purge after a certain amount of time. Option to mark as broken/missing.






  
