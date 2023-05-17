using IMDbion_MovieHandlerService.DataContext;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using IMDbion_MovieHandlerService.ExceptionHandler;
using IMDbion_MovieHandlerService.Services;
using IMDbion_MovieHandlerService.Mappers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add contexts
//builder.Services.AddDbContext<MovieContext>(options =>
//                options.UseMySQL(builder.Configuration.GetConnectionString("MovieContext")));

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<MovieContext>(options =>
                    options.UseMySQL(builder.Configuration.GetConnectionString("MovieContext")));
}
else
{
    builder.Services.AddDbContext<MovieContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MovieContext")));
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddAutoMapper(typeof(MovieMapper));

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Cors configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// Ensure the database schema is created during startup
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetService<MovieContext>();
context.Database.EnsureCreated();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

// Add custom middleware
app.UseMiddleware<CustomExceptionHandler>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
