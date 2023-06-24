using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.EntityFrameworkCore;
using MudDataGridEfCore.Data;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddMudServices();

builder.Services.AddDbContextFactory<UserDbContext>(
    options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    },
    ServiceLifetime.Transient);
builder.Services.AddTransient<UserDbContext>(provider => 
    provider.GetRequiredService<IDbContextFactory<UserDbContext>>()
        .CreateDbContext());

builder.WebHost.UseUrls("http://*:5000");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

using var scope = app.Services.CreateScope();
await using var dataContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
dataContext.Database.EnsureDeleted();
dataContext.Database.Migrate();

var firstNames = new[]
{
    "James",
    "John",
    "Robert",
    "Michael",
    "William",
    "David",
    "Richard",
    "Charles",
    "Joseph",
    "Thomas"
};

var lastNames = new[]
{
    "Smith",
    "Johnson",
    "Brown",
    "Taylor",
    "Miller",
    "Anderson",
    "Thomas",
    "Moore",
    "Clark",
    "Lewis"
};

var random = new Random((int)DateTime.Now.Ticks);
var usersToCreate = new List<UserEntity>();
for (var i = 0; i < 1000; i++)
{
    usersToCreate.Add(
        new UserEntity
        {
            FirstName = firstNames[random.Next(0, firstNames.Length)],
            LastName = lastNames[random.Next(0, lastNames.Length)]
        });
}
await dataContext.Users.AddRangeAsync(usersToCreate);
await dataContext.SaveChangesAsync();

app.Run();