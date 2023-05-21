var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


var dbService = new DatabaseService();

// Serve static files
app.UseStaticFiles();




// Endpoint for getting all users
app.MapGet("/users", async (HttpContext context) =>
{
    await Get.GetAllUsers(context.Response, dbService);
});

// Endpoint for getting a specific user
app.MapGet("/users/{id}", async (HttpContext context, string id) =>{
     await Get.GetUser(id,context.Response,dbService);
});

// Endpoint for creating a user
app.MapPost("/users", async (HttpContext context) =>{
    await Post.CreateUser(context.Request, context.Response, dbService);
});

// Endpoint for updating a user
app.MapPut("/users", async (HttpContext context) => {
    await Put.UpdateUser(context.Request, context.Response, dbService);
});

// Endpoint for deleting a user
app.MapDelete("/users/{id}", async (HttpContext context, string id) => {
    await Delete.DeleteUser(id,context.Response, dbService);
});


// The first middleware executed
app.Use(async (HttpContext context, Func<Task> next) =>
{
    var request = context.Request;
    var response = context.Response;
    var path = request.Path;

    // If the request starts with "/", send the index.html file
    if (path.StartsWithSegments("/"))
    {
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("index.html");
        return;
    }

    // Call the next middleware or endpoint in the pipeline
    await next.Invoke();
});

app.Run();