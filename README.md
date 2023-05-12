# Food Lover's Market - Liso Mbiza Assessment
Welcome to my assessment! This document will help with running the application to create, update, delete and list of branch and products

#Mutliple startup projects
1. Right on the solution "FLM.LisoMbiza.Api"
2. Click on "Set Startup Projects"
3. Click on the radio button "Multiple startup projects:"
4. Navigate to the project "FLM.LisoMbiza.Api" and set the action "Start"
5. Navigate to the project "FLM.LisoMbiza.UI" and set the action "Start"

#Start the solution
This will create the database "LisoFLM" in your SQL Express instance.

#Migration
1. Right click on the "FLM.LisoMbiza.Api" project and select "Set as Startup Project"
2. Click on the menu "Tools" => "NuGet Package Manager" => "Package Manager Console"
3. Set the "FLM.LisoMbiza.Data" projec to be the Default Project
4. EntityFrameworkCore\Add-Migration InitialMigration -context FLMContext -verbose
5. Run the solution to apply the database changes to you SQLExpress server

#File Import
1. Please make use of the SwaggerUI (https://localhost:44327/swagger/index.html) to import your files.
![image](https://github.com/mancoba-hub/FLM/assets/65565239/b51b01ea-b5c1-4a80-8ac3-8af393698afe)
![image](https://github.com/mancoba-hub/FLM/assets/65565239/cbb837ee-3180-43db-b7cc-5c43e9f7f26d)
![image](https://github.com/mancoba-hub/FLM/assets/65565239/5dc8d1d3-d677-4f5e-9e50-71ea2adf0e98)

#File Export/Download
1. In the FLM.LisoMbiza.UI, open the "appsettings.json" file.
2. Update the "BranchDownloadPath" for your exported ".xml, .csv & .json" files.
2. Update the "ProductDownloadPath" for your exported ".xml, .csv & .json" files.
