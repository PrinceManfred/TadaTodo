using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using TadaTodo.Server.Data;
using TadaTodo.Server.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
        
        // Override redirects since we are handling authentication in API endpoints
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
        
        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SupportNonNullableReferenceTypes();
    options.NonNullableReferenceTypesAsRequired();
});

builder.Services.AddDbContext<TadaTodoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDataProtection()
    .PersistKeysToDbContext<TadaTodoContext>();

builder.Services.Configure<StaticFileOptions>(options =>
{
    options.OnPrepareResponse = responseContext =>
    {
        if (responseContext.File.Name != "index.html") return;
        
        responseContext.Context.Response.Headers.CacheControl = "no-store";
        responseContext.Context.Response.Headers.Expires = "0";
    };
});

var app = builder.Build();

#if DEBUG
// Apply migrations to the database automatically during development
using (var serviceScope = app.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<TadaTodoContext>();
    dbContext.Database.Migrate();
}
#endif

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapUsersEndpoints();
app.MapTodosEndpoints();

app.MapFallbackToFile("index.html");

app.Run();
