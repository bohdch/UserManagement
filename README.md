# UserManagementApp

UserManagementApp is a basic CRUD (Create, Read, Update, Delete) app for managing user data. It provides endpoints for performing basic CRUD operations on user data, including retrieving user information, adding new users, updating existing users, and deleting users.

## Project Structure

The project structure of UserManagementApp is as follows:
- `Controllers/`: This folder contains the controller classes that define the endpoints, including `Delete.cs`, `Get.cs`, `Post.cs`, and `Put.cs`.
- `Model/`: This folder contains the `User.cs` class that defines the model for the user data.
- `ServiceFolder/`: This folder contains the `UserService.cs` class that provides the logic for managing user data.
- `Properties/`: This folder contains the `launchSettings.json` file that specifies the settings for launching the app.
- `wwwroot/`: This folder contains the static files for the app, including `site.css` in the css/ subfolder, and `site.js` in the js/ subfolder.

## Endpoints

The following endpoints are available in UserManagementApp:

- `GET /users`: Retrieve all users.
- `GET /users/{id}`: Retrieve a specific user by ID.
- `POST /users`: Add a new user.
- `PUT /users/{id}`: Update an existing user by ID.
- `DELETE /users/{id}`: Delete a user by ID.

## Technologies Used

UserManagementApp is built using the following technologies:

- .NET 7.0
- ASP.NET Core
- C#
- HTML 
- CSS 
- JavaScript
- RESTful API principles 

