using EforWebApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Servisleri ekleyelim (DbContext, Controller vb.)
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AppDbContext")));
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware'leri yapýlandýralým
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EforWebApi v1"));
}

app.UseRouting();
app.UseAuthorization();

app.MapControllers(); // Controller endpointlerini haritala

//app.UseHttpsRedirection();
//app.UseCors("AllowAllOrigins");

app.Run();
