# How to run:
- Note all paths reference Windows directories. If on Linux, adjust accordingly.
- After navigating to the folder `MVC-ExcelParsing-App`
1. **Ensure .NET 8 SDK is Installed by running (if on Windows dotnet must be in your [PATH](https://learn.microsoft.com/en-us/previous-versions/office/developer/sharepoint-2010/ee537574(v=office.14))):** 
    ```
    dotnet --list-sdks
    ```
	- If it is not installed you can [download .NET8.0 from the website](https://dotnet.microsoft.com/download)
2. **From `..\MVC-ExcelParsing-App\` run:**
	```
	dotnet build
	```
	- This will produce some build output for both the main application and the Unit Tests
3. **To run the app navigate to `..\MVC-ExcelParsing-App\ExcelParsing\` and run:**
	```
	dotnet run
	```
	- The output will look similar to the following: 
		```
			Building...
			info: Microsoft.Hosting.Lifetime[14]
			      Now listening on: http://localhost:5009
			info: Microsoft.Hosting.Lifetime[0]
			      Application started. Press Ctrl+C to shut down.
			info: Microsoft.Hosting.Lifetime[0]
			      Hosting environment: Development
			info: Microsoft.Hosting.Lifetime[0]
			      Content root path: C:<Path-To-Project>\MVC-ExcelParsing-App\ExcelParsing
			info: Microsoft.Hosting.Lifetime[0]
		```
	- you can `Ctrl + Click` on the URL provided in the first `info` label. 
	- In this case the URL is: `http://localhost:5009`
	- This will open the application on your default browser
		- As long as your terminal is open and you don't `Ctrl + C` the program will run.
4. **To run the Unit Tests navigate to `..\MVC-ExcelParsing-App\ExcelParsing.Tests\` and run:**
	```
	dotnet test
	```
	- The output will look similar to the following:
		```
		Determining projects to restore...
		All projects are up-to-date for restore.
		ExcelParsing -> C:<Path-To-Project>\MVC-ExcelParsing-App\ExcelParsing\bin\Debug\net8.0\ExcelParsing.dll
		ExcelParsing.Tests -> C:<Path-To-Project>\MVC-ExcelParsing-App\ExcelParsing.Tests\bin\Debug\net8.0\ExcelParsing.Tests.dll
		Test run for C:<Path-To-Project>\MVC-ExcelParsing-App\ExcelParsing.Tests\bin\Debug\net8.0\ExcelParsing.Tests.dll (.NETCoreApp,Version=v8.0)
		Microsoft (R) Test Execution Command Line Tool Version 17.10.0 (x64)
		Copyright (c) Microsoft Corporation.  All rights reserved.
		
		Starting test execution, please wait...
		A total of 1 test files matched the specified pattern.
		
		Passed!  - Failed:     0, Passed:     7, Skipped:     0, Total:     7, Duration: 540 ms - ExcelParsing.Tests.dll (net8.0)
		
		Workload updates are available. Run `dotnet workload list` for more information.
		```
# Sample Files: 
- The foler `..\ExcelParsing.Tests\Data\` contains three Excel Files: 
1. ValidData.xlsx
2. InvalidData.xlsx
3. Sample Data.xlsx

- ## ValidData.xlsx:
	- Used in Unit Testing
	- Has 5 rows/entries
	- Data tries to use all values for variety
- ## InvalidData.xlsx:
	- Used in Unit Testing
	- Same data as in ValidData.xlsx but some fields contain invalid data that will produce errors
- ## Sample Data.xlsx:
	- ~2000 entries that allow for the full testing of the appliaction with a larger data set
	- When prompted for a file navigate to this folder and select the file

# Accesibility checked through WCAG AInspector Extension on Firefox