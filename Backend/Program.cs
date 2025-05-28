using Swashbuckle.AspNetCore.Annotations;
using Backend.Data;
using Dapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Database Connection
builder.Services.AddScoped<DbConnection>(_ => new DbConnection(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

// Add OpenAPI/Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "File Upload API",
        Version = "v1",
        Description = "A simple file upload API with support for images and videos"
    });
    
    c.EnableAnnotations();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();