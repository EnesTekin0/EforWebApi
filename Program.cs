using EforWebApi.Models;
using Microsoft.AspNetCore.Identity; 
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens; 
using Microsoft.OpenApi.Models; 
using System.Text; 
using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Microsoft.OpenApi.Any;
using System;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AppDbContext")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>() 
    .AddEntityFrameworkStores<AppDbContext>() 
    .AddDefaultTokenProviders(); 

builder.Services.AddAuthentication(options => 
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => 
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) 
    };
});

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

AppContext.SetSwitch("System.Globalization.Invariant", true);
TimeZoneInfo turkishTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
DateTime turkishNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, turkishTimeZone);
Console.WriteLine($"Current time in Turkey: {turkishNow}");


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers()
      .AddJsonOptions(options =>
      {
          // Enum dönüþümü için JsonStringEnumConverter ekle
          options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
      });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>

{
     c.MapType<Role>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = Enum.GetValues(typeof(Role))
            .Cast<Role>()
            .Select(e => new OpenApiString(e.ToString()) as IOpenApiAny) // Dönüþüm burada yapýldý
            .ToList()
    });

    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Efor Web API", Version = "v1" }); 
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme 
    {
        In = ParameterLocation.Header,
        Description = "Please enter into field the word 'Bearer' following by space and JWT",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement { 
    {
        new OpenApiSecurityScheme {
        Reference = new OpenApiReference {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
        },
        new string[] { }
    }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Efor Web API v1"));
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAllOrigins");

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();


