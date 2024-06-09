using AutoMapper;
using LifeEcommerce.Helpers;
using Microsoft.EntityFrameworkCore;
using MultiLanguageExamManagementSystem.Data;
using MultiLanguageExamManagementSystem.Data.UnitOfWork;
using MultiLanguageExamManagementSystem.Helpers;
using MultiLanguageExamManagementSystem.Services;
using MultiLanguageExamManagementSystem.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

var mapperConfiguration = new MapperConfiguration(mc => mc.AddProfile(new AutoMapperConfigurations()));
IMapper mapper = mapperConfiguration.CreateMapper();
builder.Services.AddSingleton(mapper);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICultureService, CultureService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register the ITranslationService with its implementation
builder.Services.AddScoped<ITranslationService, TranslationService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Add this line
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Customize this as needed
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseMiddleware<CultureMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.Run();
