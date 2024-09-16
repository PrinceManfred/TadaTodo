namespace TadaTodo.Server.Dtos;

public record UpdateTodoListDto(string? Name, List<CreateTodoItemDto>? NewItems, List<UpdateTodoItemDto>? UpdatedItems, List<int>? DeletedItems);