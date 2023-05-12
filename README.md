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
1. Please make use of the SwaggerUI to import your files.


#File Export/Download
1. In the FLM.LisoMbiza.UI, open the "appsettings.json" file.
2. Update the "BranchDownloadPath" for your exported ".xml" files.
2. Update the "ProductDownloadPath" for your exported ".xml" files.
