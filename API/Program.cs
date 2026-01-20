using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models; // <-- TO JEST WA¯NE DLA SWAGGERA
using API.Data; // Upewnij siê, ¿e namespace pasuje do Twojego projektu (np. Project.API.Data)

var builder = WebApplication.CreateBuilder(args);

// 1. Baza Danych
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllers();

// 2. Konfiguracja Swaggera z K³ódk¹ (JWT)
builder.Services.AddEndpointsApiExplorer();

// --- OD T¥D ZMIANA ---
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Distributed System API", Version = "v1" });

    // Definicja zabezpieczenia (¿e u¿ywamy Bearer Token)
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Wpisz 'Bearer [spacja] i twój token'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Wymaganie zabezpieczenia
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
// --- DO T¥D ZMIANA ---

// 3. Konfiguracja JWT (Autoryzacja)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetSection("JwtSettings:SecretKey").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 4. Kolejnoœæ Middleware (Wa¿ne!)
app.UseAuthentication(); // <-- SprawdŸ KIM jest u¿ytkownik
app.UseAuthorization();  // <-- SprawdŸ CO mo¿e zrobiæ

app.MapControllers();

app.Run();