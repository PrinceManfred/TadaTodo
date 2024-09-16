namespace TadaTodo.Server.Dtos;

public record UpdateTodoItemDto(int Id, string Value, bool IsCompleted);