# UserManagementWebAPI

UserManagementWebAPI is a basic CRUD (Create, Read, Update, Delete) API for managing user data. It provides endpoints for performing basic CRUD operations on user data, including retrieving user information, adding new users, updating existing users, and deleting users.

## Project Structure

The project structure of UserManagementWebAPI is as follows:
- `Controllers/`: This folder contains the controller classes that define the endpoints for the API, including `Delete.cs`, `Get.cs`, `Post.cs`, and `Put.cs`.
- `Model/`: This folder contains the `Person.cs` class that defines the model for the user data.
- `ServiceFolder/`: This folder contains the `UserService.cs` class that provides the logic for managing user data.
- `Properties/`: This folder contains the `launchSettings.json` file that specifies the settings for launching the API.
- `wwwroot/`: This folder contains the static files for the API, including `site.css` in the css/ subfolder, and `site.js` in the js/ subfolder, which are used for front-end rendering.

## Endpoints

The following endpoints are available in UserManagementWebAPI:

- `GET /api/users`: Retrieve all users.
- `GET /api/users/{id}`: Retrieve a specific user by ID.
- `POST /api/users`: Add a new user.
- `PUT /api/users/{id}`: Update an existing user by ID.
- `DELETE /api/users/{id}`: Delete a user by ID.

## Technologies Used

UserManagementWebAPI is built using the following technologies:

- .NET 7.0
- ASP.NET Core
- C#
- HTML 
- CSS 
- JavaScript
- RESTful API principles 

