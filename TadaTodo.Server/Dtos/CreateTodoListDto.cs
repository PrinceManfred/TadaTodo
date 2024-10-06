namespace TadaTodo.Server.Dtos;

public record CreateTodoListDto(string Name, List<CreateTodoItemDto>? TodoItems);