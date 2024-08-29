# ASPNetRESTAPI01

This repository contains the source code for the ASP.NET REST API tutorial series by Julio Casal.  
You can follow the tutorial series on [Julio Casal ASP.NET REST Web API](https://youtu.be/AhAxLiGC7Pc?si=ItoWuPKVPbNJvG6u).

## Database Configuration

This project uses SQLite as the DBMS, and the database is stored in the project directory under the name `GameStore.db`. Install the "SQLite" extension in Visual Studio Code to view and query the SQLite database.

## Quick Start

Follow the steps below to get started quickly:

1. Open the source code in Visual Studio Code.
2. Open the terminal and execute the following command to initiate the database and run the project:
   ```bash
   dotnet run
   ```
3. Install the "REST Client" extension in Visual Studio Code. This extension is used to test HTTP requests.
4. Use the `games.http` or `genres.http` files to test the requests. Right-click on the request and select `Send Request`.
