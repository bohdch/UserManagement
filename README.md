# UserManagementApp

UserManagementApp is a basic CRUD (Create, Read, Update, Delete) app for managing user data. It provides endpoints for performing basic CRUD operations on user data, including retrieving user information, adding new users, updating existing users, and deleting users.

## Project Structure

The project structure of UserManagementApp is as follows:
- `DataAccess/`: This directory contains the classes responsible for accessing and interacting with the database, including `Delete.cs`, `Get.cs`, `Post.cs`, and `Put.cs`.
- `Model/`: This directory contains the `User.cs` class that defines the model for the user data.
- `ServiceFolder/`: This directory contains the `DatabaseService.cs` class that provides the connection to the database.
- `Properties/`: This directory contains the `launchSettings.json` file that specifies the settings for launching the app.
- `wwwroot/`: This directory contains the static files for the app, including `site.css` in the css/ subfolder, and `site.js` in the js/ subfolder.

## Endpoints

The following endpoints are available in UserManagementApp:

- `GET /users`: Retrieve all users.
- `GET /users/{id}`: Retrieve a specific user by ID.
- `POST /users`: Add a new user.
- `PUT /users/{id}`: Update an existing user by ID.
- `DELETE /users/{id}`: Delete a user by ID.

## Running the Project

In order to run the project, follow these steps:

1. Ensure that you have .NET 7.0 SDK installed on your machine.
2. Clone the project repository from the version control system of your choice.
3. Open the project in your preferred IDE.
4. Run the provided SQL script `db_setup.sql` using SSMS or CLI to create the database, tables, and procedures.
5. Build the project using the command `dotnet build` to restore dependencies and compile the source code.
6. Start the application using the command `dotnet run`

## Technologies Used

UserManagementApp is built using the following technologies:

- .NET 7.0
- ASP.NET Core
- C#
- HTML 
- CSS 
- JavaScript
- ADO.NET
- NuGet
- RESTful API principles 
