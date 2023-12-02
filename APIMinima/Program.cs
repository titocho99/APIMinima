using APIMinima;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();
//ejercicio 2
//var todoitems = app.MapGroup("/todoitems");


RouteGroupBuilder todoitems = app.MapGroup("/todoitems");

todoitems.MapGet("/", GetAllTodos);
todoitems.MapGet("/complete", GetCompleteTodos);
todoitems.MapGet("/{id}", GetTodo);
todoitems.MapPost("/", CreateTodo);
todoitems.MapPut("/{id}", UpdateTodo);
todoitems.MapDelete("/{id}", DeleteTodo);


app.Run();
//eje 2
//static async Task<IResult> GetAllTodos(TodoDb db)
//{
//    return TypedResults.Ok(await db.Todos.ToArrayAsync());
//}

static async Task<IResult> GetAllTodos(TodoDb db)
{
    return TypedResults.Ok(await db.Todos.Select(x => new TodoItemDTO(x)).ToArrayAsync());
}
// eje 2
//static async Task<IResult> GetCompleteTodos(TodoDb db)
//{
//    return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).ToArrayAsync());
//}

static async Task<IResult> GetCompleteTodos(TodoDb db)
{
    return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).Select(x => new TodoItemDTO(x)).ToArrayAsync());
}
//eje 2
//static async Task<IResult> GetTodo(int id, TodoDb db)
//{
//    return await db.Todos.FindAsync(id)
//        is Todo todo
//        ? TypedResults.Ok(todo)
//        : TypedResults.NotFound();
//}

static async Task<IResult> GetTodo(int id, TodoDb db)
{
    return await db.Todos.FindAsync(id)
        is Todo todo
        ? TypedResults.Ok(new TodoItemDTO(todo))
        : TypedResults.NotFound();
}
//eje 2
//static async Task<IResult> CreateTodo(Todo todo, TodoDb db)
//{
//    db.Todos.Add(todo);
//    await db.SaveChangesAsync();

//    return TypedResults.Created($"/todoitems/ {todo.Id}",todo);
//}

static async Task<IResult> CreateTodo(TodoItemDTO todoItemDto, TodoDb db)
{
    var todoItem = new Todo
    {
        IsComplete = todoItemDto.IsComplete,
        Name = todoItemDto.Name
    };
    db.Todos.Add(todoItem);
    await db.SaveChangesAsync();

    todoItemDto = new TodoItemDTO(todoItem);

    return TypedResults.Created($"/todoitems/ {todoItem.Id}", todoItemDto);
}

//eje 2
//static async Task<IResult> UpdateTodo(int id, Todo inputTodo, TodoDb db)
//{
//    var todo = await db.Todos.FindAsync(id);
//    if (todo == null)   return TypedResults.NotFound();

//    todo.Name = inputTodo.Name;
//    todo.IsComplete= inputTodo.IsComplete;

//    await db.SaveChangesAsync();

//    return TypedResults.NoContent();
//}

static async Task<IResult> UpdateTodo(int id, TodoItemDTO todoItemDto, TodoDb db)
{
    var todo = await db.Todos.FindAsync(id);
    if (todo == null) return TypedResults.NotFound();

    todo.Name = todoItemDto.Name;
    todo.IsComplete = todoItemDto.IsComplete;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

//eje 2
//static async Task<IResult> DeleteTodo(int id, TodoDb db)
//{
//    if(await db.Todos.FindAsync(id) is Todo todo)
//    {
//        db.Todos.Remove(todo);
//        await db.SaveChangesAsync();
//        return TypedResults.Ok(todo);

//    }

//    return TypedResults.NotFound();

//}

static async Task<IResult> DeleteTodo(int id, TodoDb db)
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();

        TodoItemDTO todoItemDTO = new TodoItemDTO(todo);

        return TypedResults.Ok(todoItemDTO);

    }

    return TypedResults.NotFound();

}

// eje 1

//todoitems.MapGet("/", async (TodoDb db) =>
//await db.Todos.ToListAsync()) ;

//todoitems.MapGet("/complete", async (TodoDb db) =>
//await db.Todos.Where(t => t.IsComplete).ToListAsync());


//todoitems.MapGet("/{id}",async (int id, TodoDb db)=>
//await db.Todos.FindAsync(id)
//is Todo todo
//? Results.Ok(todo)
//:Results.NotFound());

//todoitems.MapPost("/", async (Todo todo, TodoDb db) =>
//{
//    db.Todos.Add(todo);
//    await db.SaveChangesAsync();
//    return Results.Created($"/todoitems/{todo.Id}", todo);

//});

//todoitems.MapPut("/{id}", async (int id, Todo inputTodo, TodoDb db) =>
//{
//    var todo = await db.Todos.FindAsync(id);
//    if(todo == null) return Results.NotFound();

//    todo.Name = inputTodo.Name;
//    todo.IsComplete = inputTodo.IsComplete;

//    await db.SaveChangesAsync();

//    return Results.NoContent();

//});

//todoitems.MapDelete("/{id}", async (int id, TodoDb db) =>
//{
//    if (await db.Todos.FindAsync(id) is Todo todo)
//    {
//        db.Todos.Remove(todo);
//        await db.SaveChangesAsync();
//        return Results.Ok(todo);
//    }
//    return Results.NotFound();
//});

//app.Run();


