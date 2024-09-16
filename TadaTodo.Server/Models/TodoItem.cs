using JetBrains.Annotations;

namespace TadaTodo.Server.Models;

[PublicAPI]
public class TodoItem
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    // EF Core Constructor
    private TodoItem()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        
    }
    
    public TodoItem(string value, bool isCompleted)
    {
        Value = value;
        IsCompleted = isCompleted;
    }
    
    public int Id { get; private set; }
    public string Value { get; set; }
    public bool IsCompleted { get; set; }
}