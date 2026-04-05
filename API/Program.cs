using TeaShop.Data;
using TeaShop.Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = builder.Configuration["JWT:ValidIssuer"],
            ValidAudience            = builder.Configuration["JWT:ValidAudience"],
            IssuerSigningKey         = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(
                    builder.Configuration["JWT:SecretKey"]))
        };
    });

    builder.Services.AddScoped<IAuthService,     AuthService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService,  UsuarioService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IProductoRepository, ProductoEFRepository>();
builder.Services.AddScoped<IPedidoService,   PedidoService>();
builder.Services.AddScoped<IPedidoRepository,  PedidoEFRepository>();

var connectionString = builder.Configuration.GetConnectionString("ServerDB_localhost");
builder.Services.AddDbContext<TeashopContext>(options =>
    options.UseSqlServer(connectionString)
);    

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "TeaShop API", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Introduzca el token JWT.",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }    
    });
});

var app = builder.Build();

{   
    app.UseSwagger(); 
    app.UseSwaggerUI(); 
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();