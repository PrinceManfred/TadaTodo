namespace TadaTodo.Server.Dtos;

public record UpdateTodoListDto(int Id, string? Name, List<CreateTodoItemDto>? NewItems, List<UpdateTodoItemDto>? UpdatedItems, List<int>? DeletedItems);