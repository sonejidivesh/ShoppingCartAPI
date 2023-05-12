# Intution_API

# Setting database code first approach 


Modifyy or add connections string in appsetting prefered modify the default connection string 

Install  dotnet ef tool
 Coomand: dotnet tool install --global dotnet-ef


install entityframework design
 Command : dotnet add package Microsoft.EntityFrameworkCore.Design

 

Run : dotnet ef



if any Migration folder present you delete that and follow these commands

dotnet ef migrations add "initialMigration"

dotnet ef database update