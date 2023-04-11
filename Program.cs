var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var userService = new UserService();

// Serve static files
app.UseStaticFiles();


// Endpoint for getting all users
app.MapGet("/api/users", async (HttpContext context) =>
{
    await Get.GetAllUsers(context.Response, userService);
});

// Endpoint for getting a specific user
app.MapGet("/api/users/{id}", async (HttpContext context, string id) =>{
     await Get.GetUser(id,context.Response,userService);
});

// Endpoint for creating a user
app.MapPost("/api/users", async (HttpContext context) =>{
    await Post.CreateUser(context.Request, context.Response, userService);
});

// Endpoint for editing a user
 app.MapPut("/api/users", async (HttpContext context) => {
     await Put.EditUser(context.Request, context.Response, userService);
});

// Endpoint for deleting a user
app.MapDelete("/api/users/{id}", async (HttpContext context, string id) => {
    await Delete.DeleteUser(id,context.Response, userService);
});


// The first middleware executed
app.Use(async (HttpContext context, Func<Task> next) =>
{
    var request = context.Request;
    var response = context.Response;
    var path = request.Path;

    // If the request does not start with "/api", send the index.html file
    if (!path.StartsWithSegments("/api"))
    {
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("index.html");
        return;
    }

    // Call the next middleware or endpoint in the pipeline
    await next.Invoke();
});

app.Run();