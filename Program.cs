using Microsoft.EntityFrameworkCore;
using BackEnd.Data;
using BackEnd.Services;
using BackEnd.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar servicios
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ICitaService, CitaService>();
builder.Services.AddScoped<IDocumentoService, DocumentoService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();

// Configurar la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging() // Solo para desarrollo
           .LogTo(Console.WriteLine, LogLevel.Information));

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
