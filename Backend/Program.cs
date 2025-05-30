using Swashbuckle.AspNetCore.Annotations;
using Backend.Data;
using Dapper;

var builder = WebApplication.CreateBuilder(args);

//Set the listening URLs
builder.WebHost.UseUrls("https://localhost:7181");

// Add services to the container.
builder.Services.AddControllers();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Vite frontend origin
              .AllowAnyMethod() // Allow GET, POST, etc.
              .AllowAnyHeader() // Allow any headers
              .AllowCredentials(); // Allow cookies or auth headers if needed
    });
});

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
app.UseCors("AllowViteFrontend"); // Apply CORS policy
app.UseAuthorization();
app.MapControllers();

app.Run();