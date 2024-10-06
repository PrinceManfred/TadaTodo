using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TadaTodo.Server.Helpers;
using TadaTodo.Server.Data;
using TadaTodo.Server.Dtos;
using TadaTodo.Server.Models;
using HttpContext = Microsoft.AspNetCore.Http.HttpContext;

namespace TadaTodo.Server.Endpoints;

public static class TodosEndpoint
{
    public static void MapTodosEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/todos")
            .RequireAuthorization()
            .WithTags("Todos");

        group.MapGet("", GetAllTodoLists)
            .WithName("GetAllTodoLists");

        group.MapGet("{id:int}", GetTodoList)
            .WithName("GetTodoList");

        group.MapDelete("{id:int}", DeleteTodoList)
            .WithName("DeleteTodoList");

        group.MapPost("", CreateTodoList)
            .WithName("CreateTodoList");

        group.MapPatch("{id:int}", UpdateTodoList)
            .WithName("UpdateTodoList");
    }

    private static async Task<Results<Ok<List<TodoList>>, BadRequest>> GetAllTodoLists([FromQuery] string? search, HttpContext httpContext,
        TadaTodoContext context)
    {
        var userId = httpContext.User.GetUserId();
        IQueryable<User> userQuery;
        if (search is null)
        {
            userQuery = context.Users
                .Include(u => u.TodoLists)
                .ThenInclude(tl => tl.TodoItems)
                .AsNoTracking()
                .Where(u => u.Id == userId);
        }
        else
        {
            userQuery = context.Users
                .AsNoTracking()
                .Include(u => u.TodoLists)
                // ReSharper disable once EntityFramework.ClientSideDbFunctionCall - incorrectly flagging the `Like` method as not in a DB query
                .ThenInclude(tl => tl.TodoItems.Where(ti => EF.Functions.Like(ti.Value, $"%{search}%")))
                .Where(u => u.Id == userId);
        }
        
        var user = await userQuery.FirstOrDefaultAsync();

        if (user is null) return TypedResults.BadRequest();

        var todoLists = search is null ? user.TodoLists : user.TodoLists.Where(tl => tl.TodoItems.Count > 0).ToList();
        return TypedResults.Ok(todoLists);
    }

    private static async Task<Results<Ok<TodoList>, NotFound, BadRequest>> GetTodoList(int id, HttpContext httpContext,
        TadaTodoContext context)
    {
        var userId = httpContext.User.GetUserId();
        var userQuery = context.Users
            .Include(u => u.TodoLists.Where(tl => tl.Id == id))
            .ThenInclude(tl => tl.TodoItems)
            .AsNoTracking()
            .Where(u => u.Id == userId);

        var user = await userQuery.FirstOrDefaultAsync();

        if (user is null) return TypedResults.BadRequest();

        if (user.TodoLists.Count == 0) return TypedResults.NotFound();

        return TypedResults.Ok(user.TodoLists[0]);
    }

    private static async Task<Results<Ok, NotFound, BadRequest>> DeleteTodoList(int id, HttpContext httpContext,
        TadaTodoContext context)
    {
        var userId = httpContext.User.GetUserId();
        var userQuery = context.Users
            .Include(u => u.TodoLists.Where(tl => tl.Id == id))
            .AsNoTracking()
            .Where(u => u.Id == userId);

        var user = await userQuery.FirstOrDefaultAsync();

        if (user is null) return TypedResults.BadRequest();

        if (user.TodoLists.Count == 0) return TypedResults.NotFound();

        context.TodoLists.Remove(user.TodoLists[0]);
        await context.SaveChangesAsync();

        return TypedResults.Ok();
    }

    private static async Task<Results<Created<TodoList>, BadRequest, BadRequest<string>>> CreateTodoList(CreateTodoListDto newTodoList,
        IValidator<CreateTodoListDto> validator, HttpContext httpContext, TadaTodoContext context)
    {
        var validation = await validator.ValidateAsync(newTodoList);
        if (!validation.IsValid) return TypedResults.BadRequest(validation.Errors.FirstOrDefault()?.ErrorMessage ?? "New list failed validation.");
        
        var userId = httpContext.User.GetUserId();
        var user = await context.Users.FindAsync(userId);
        if (user is null) return TypedResults.BadRequest();

        var todoList = new TodoList(newTodoList.Name);
        
        if(newTodoList.TodoItems is not null)
            foreach (var item in newTodoList.TodoItems) todoList.TodoItems.Add(new TodoItem(item.Value, item.IsCompleted));

        user.TodoLists.Add(todoList);

        await context.SaveChangesAsync();
        return TypedResults.Created($"/api/todos/{todoList.Id}", todoList);
    }

    private static async Task<Results<Ok<TodoList>, NotFound, BadRequest, BadRequest<string>>> UpdateTodoList(int id,
        UpdateTodoListDto updateTodoList, IValidator<UpdateTodoListDto> validator, HttpContext httpContext, TadaTodoContext context)
    {
        var validation = await validator.ValidateAsync(updateTodoList);
        if (!validation.IsValid) return TypedResults.BadRequest(validation.Errors.FirstOrDefault()?.ErrorMessage ?? "Update failed validation.");
        
        var userId = httpContext.User.GetUserId();
        var user = await context.Users
            .Include(u => u.TodoLists.Where(tl => tl.Id == id))
            .ThenInclude(tl => tl.TodoItems)
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync();

        if (user is null) return TypedResults.BadRequest();
        if (user.TodoLists.Count == 0) TypedResults.NotFound();

        var todoList = user.TodoLists[0];

        if (updateTodoList.Name is not null) todoList.Name = updateTodoList.Name;

        foreach (var deletedId in updateTodoList.DeletedItems ?? [])
        {
            var index = todoList.TodoItems.FindIndex(i => i.Id == deletedId);
            if (index < 0) continue;
            todoList.TodoItems.RemoveAt(index);
        }

        foreach (var updatedItem in updateTodoList.UpdatedItems ?? [])
        {
            var item = todoList.TodoItems.Find(i => i.Id == updatedItem.Id);
            if(item is null) return TypedResults.BadRequest();
            item.Value = updatedItem.Value;
            item.IsCompleted = updatedItem.IsCompleted;
        }

        foreach (var newItem in updateTodoList.NewItems ?? [])
        {
            todoList.TodoItems.Add(new TodoItem(newItem.Value, newItem.IsCompleted));
        }
        
        await context.SaveChangesAsync();

        return TypedResults.Ok(todoList);
    }
}