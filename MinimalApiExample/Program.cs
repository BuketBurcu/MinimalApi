var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Todo item model

// In-memory data storage (simulating database)
var todoItems = new List<TodoItem>
{
    new TodoItem(1, "Learn Minimal API", false),
    new TodoItem(2, "Write blog post", true)
};


// Routes
app.MapGet("/todos", () =>
{
    return todoItems;
});

app.MapGet("/todos/{id}", (int id) =>
{
    var todoItem = todoItems.Find(item => item.Id == id);
    return todoItem is not null ? Results.Ok(todoItem) : Results.NotFound();
});

app.MapPost("/todos", (TodoItem newItem) =>
{
    todoItems.Add(newItem);
    return Results.Created($"/todos/{newItem.Id}", newItem);
});

app.MapPut("/todos/{id}", (int id, TodoItem updatedItem) =>
{
    var todoIndex = todoItems.FindIndex(item => item.Id == id);
    if (todoIndex == -1)
        return Results.NotFound();
    todoItems[todoIndex] = updatedItem with { Id = id };
    return Results.Ok(updatedItem);
});

app.MapDelete("/todos/{id}", (int id) =>
{
    var todoIndex = todoItems.FindIndex(item => item.Id == id);
    if (todoIndex == -1)
        return Results.NotFound();
    todoItems.RemoveAt(todoIndex);
    return Results.NoContent();
});


app.Run();

public record TodoItem(int Id, string Task, bool IsCompleted);

