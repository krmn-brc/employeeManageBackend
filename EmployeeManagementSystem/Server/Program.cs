using System.Text;
using BaseLibrary.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServerLibrary.Data;
using ServerLibrary.Helpers;
using ServerLibrary.Repositories.Contracts;
using ServerLibrary.Repositories.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtSection>(builder.Configuration.GetSection("JwtSection"));
var jwtSection = builder.Configuration.GetSection(nameof(JwtSection)).Get<JwtSection>();
// Starting
builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection") ?? 
    throw new InvalidOperationException("Sorry, your connection is not found!"));
});
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSection!.Issuer,
        ValidAudience = jwtSection.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection.Key!))
    };
});

builder.Services.AddScoped<IUserAccount, UserAccountRepository>();
builder.Services.AddScoped<IGenericRepository<GeneralDepartment>, GeneralDepartmentRepository>();
builder.Services.AddScoped<IGenericRepository<Department>, DepartmentRepository>();
builder.Services.AddScoped<IGenericRepository<Branch>, BranchRepository>();

builder.Services.AddScoped<IGenericRepository<Country>, CountryRepository>();
builder.Services.AddScoped<IGenericRepository<City>, CityRepository>();
builder.Services.AddScoped<IGenericRepository<Town>, TownRepository>();
builder.Services.AddScoped<IGenericRepository<Employee>, EmployeeRepository>();


builder.Services.AddScoped<IGenericRepository<Doctor>, DoctorRepository>();
builder.Services.AddScoped<IGenericRepository<Overtime>, OvertimeRepository>();
builder.Services.AddScoped<IGenericRepository<Sanction>, SanctionRepository>();
builder.Services.AddScoped<IGenericRepository<Vacation>, VacationRepository>();

builder.Services.AddScoped<IGenericRepository<OvertimeType>, OvertimeTypeRepository>();
builder.Services.AddScoped<IGenericRepository<SanctionType>, SanctionTypeRepository>();
builder.Services.AddScoped<IGenericRepository<VacationType>, VacationTypeRepository>();




builder.Services.AddCors(options => {
    options.AddPolicy("AllowBlazorWasm", 
        builder => builder
        .WithOrigins("https://potential-orbit-6j66vrj47rx35wx4-3000.app.github.dev")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
    );
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowBlazorWasm");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


