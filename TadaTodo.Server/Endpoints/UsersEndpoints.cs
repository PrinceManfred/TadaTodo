using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TadaTodo.Server.Helpers;
using TadaTodo.Server.Data;
using TadaTodo.Server.Dtos;
using TadaTodo.Server.Models;

namespace TadaTodo.Server.Endpoints;

public static class UsersEndpoints
{
    public static void MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/users")
            .WithTags("Users");

        group.MapPost("", Register)
            .WithName("Register");

        group.MapPost("login", Login)
            .WithName("Login");

        group.MapGet("logout", (Delegate)Logout)
            .WithName("Logout");

        group.MapGet("me", Me)
            .RequireAuthorization()
            .WithName("Me");
    }
    
    private static async Task<Results<Created<UserDto>, BadRequest<string>, Conflict<string>>> Register(UserLoginDto userLogin, HttpContext httpContext, TadaTodoContext context, IValidator<UserLoginDto> validator)
    {
        userLogin = userLogin with { Username = userLogin.Username.Trim() };
        
        var validation = await validator.ValidateAsync(userLogin);
        if (!validation.IsValid) return TypedResults.BadRequest(validation.Errors.FirstOrDefault()?.ErrorMessage ?? "Invalid username or password");
        
        var existingUser = await context.Users.Where(u => u.Username == userLogin.Username).FirstOrDefaultAsync();
        if (existingUser is not null) return TypedResults.Conflict("Username already taken.");
        
        var user = new User(userLogin.Username, userLogin.Password);
        context.Add(user);
        await context.SaveChangesAsync();
        
        await LoginUser(user, httpContext);
        
        return TypedResults.Created("api/users/me", new UserDto(user.Id, user.Username));
    }
    
    private static async Task<Results<Ok<UserDto>, BadRequest>> Login(UserLoginDto userLogin, HttpContext httpContext, TadaTodoContext context)
    {
        userLogin = userLogin with { Username = userLogin.Username.Trim() };
        
        var user = await context.Users
            .Where(u => u.Username == userLogin.Username)
            .FirstOrDefaultAsync();
        
        if (user is null || !user.VerifyPassword(userLogin.Password)) return TypedResults.BadRequest();

        await LoginUser(user, httpContext);
        
        return TypedResults.Ok(new UserDto(user.Id, user.Username));
    }
    
    private static async Task<Ok> Logout(HttpContext context)
    {
        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return TypedResults.Ok();
    }
    
    private static Results<Ok<UserDto>, BadRequest> Me(HttpContext context)
    {
        var id = context.User.GetUserId();

        var username = context.User.GetUserName();
        if(username is null) return TypedResults.BadRequest();
        
        return TypedResults.Ok(new UserDto(id, username));
    }

    private static async Task LoginUser(User user, HttpContext context)
    {
        List<Claim> claims = [
            new Claim("Id", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)];

        var principal =
            new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
        
        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

    }
}