//using EforWebApi.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.OpenApi.Models;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllers();

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("AppDbContext")));

//AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
//AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAllOrigins", policy =>
//    {
//        policy.AllowAnyOrigin()
//              .AllowAnyMethod()
//              .AllowAnyHeader();
//    });
//});

//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();


//var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//    app.UseSwagger();
//    app.UseSwaggerUI();


//}

//app.UseRouting();
//app.UseAuthorization();
//app.MapControllers();
//app.UseHttpsRedirection();
//app.UseCors("AllowAllOrigins");

//app.Run();



using EforWebApi.Models;
using Microsoft.AspNetCore.Identity; // Eklendi
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens; // Eklendi
using Microsoft.OpenApi.Models; // Eklendi
using System.Text; // Eklendi
using Microsoft.AspNetCore.Authentication.JwtBearer; // Eklendi

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AppDbContext")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>() // Eklendi
    .AddEntityFrameworkStores<AppDbContext>() // Eklendi
    .AddDefaultTokenProviders(); // Eklendi

builder.Services.AddAuthentication(options => // Eklendi
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => // Eklendi
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Eklendi
    };
});

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Efor Web API", Version = "v1" }); // Eklendi
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme // Eklendi
    {
        In = ParameterLocation.Header,
        Description = "Please enter into field the word 'Bearer' following by space and JWT",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement { // Eklendi
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

app.UseAuthentication(); // Eklendi
app.UseAuthorization();

app.MapControllers();

app.Run();


