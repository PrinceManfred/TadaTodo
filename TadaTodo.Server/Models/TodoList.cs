using JetBrains.Annotations;

namespace TadaTodo.Server.Models;

[PublicAPI]
public class TodoList
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    // EF Core Constructor
    private TodoList()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    public TodoList(string name)
    {
        Name = name;
    }

    public int Id { get; private set; }
    public string Name { get; set; }

    public List<TodoItem> TodoItems { get; private set; } = [];
}