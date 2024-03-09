using BLL.Authentication;
using BLL.FluentValidation;
using BLL.Interfaces;
using BLL.IRepository;
using BLL.IServices;
using BLL.Services;
using BLL.Validations;
using DAL.Data;
using DAL.FluentApi;
using DAL.Repository;
using DAL.UnitOfWork;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SMSAPI.Helpers;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped(typeof(IUnitOfWork<>), 
                            typeof(UnitOfWork<>));

builder.Services.AddScoped(typeof(IGenericRepository<>), 
                            typeof(GenericRepository<>));

builder.Services.AddScoped(typeof(ICampaignRepository), 
                            typeof(CampaignRepository));

builder.Services.AddScoped(typeof(ISMSNumberRepository), 
                            typeof(SMSNumberRepository));

builder.Services.AddTransient(typeof(ICampaignService), 
                            typeof(CampaignService));

builder.Services.AddTransient(typeof(ISubscriptionService), 
                            typeof(SubscriptionService));

builder.Services.AddTransient(typeof(ISMSNumberService), 
                            typeof(SMSNumberService));

builder.Services.AddTransient(typeof(IAuthenService), 
                            typeof(AuthenService));


// Fluent Validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CampaignAddDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ChangePasswordValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<SubscriptionAddDTOValidator>();

// Validation for check if ModelState.IsValid or not
builder.Services.AddScoped<ModelValidationAttribute>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<ApplicationUser>>();

builder.Services.Configure<JWTService>(builder.Configuration.GetSection("JWT"));


//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Allow CORS => Cross Origin Resource Sharing to consume my API
builder.Services.AddCors();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
    // Add Definition for APIs

    options.SwaggerDoc("CampaignAPIv1", new OpenApiInfo
    {
        Title = "Campaign",
        Version = "v1",
        Description = "Campaign API Endpoint",
        //Contact = new OpenApiContact
        //{
        //    Name = "Islam Ismail",
        //    Email = "islam.ismail.ali@icloud.com",
        //    Url = new Uri("https://www.linkedin.com/in/islam-ismail-ali/")
        //},
        //License = new OpenApiLicense
        //{
        //    Name = "My Lincese",
        //    Url = new Uri("https://www.linkedin.com/in/islam-ismail-ali/")
        //}
    });

    options.SwaggerDoc("AuthenticationAPIv1", new OpenApiInfo
    {
        Title = "Authentication",
        Version = "v1",
        Description = "Authentication API Endpoint",
    });
    
    options.SwaggerDoc("SMSNumbersAPIv1", new OpenApiInfo
    {
        Title = "SMSNumbers",
        Version = "v1",
        Description = "SMSNumbers API Endpoint",
    });
    
    options.SwaggerDoc("SubscriptionAPIv1", new OpenApiInfo
    {
        Title = "Subscription",
        Version = "v1",
        Description = "Subscription API Endpoint",
    });

    // For Authorize the API with JWT Bearer Tokens

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT API KEY"
    });

    // For Authorize the End Points such as GET,POST 

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

//builder.Services.AddAuthorization();
// Add configuration from appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        //    options.InjectStylesheet("/swagger-ui/custom.css");

        options.SwaggerEndpoint("/swagger/CampaignAPIv1/swagger.json", "CampaignAPI");
        options.SwaggerEndpoint("/swagger/AuthenticationAPIv1/swagger.json", "AuthenticationAPI");
        options.SwaggerEndpoint("/swagger/SMSNumbersAPIv1/swagger.json", "SMSNumbersAPI");
        options.SwaggerEndpoint("/swagger/SubscriptionAPIv1/swagger.json", "SubscriptionAPI");
    }
);
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.MapControllers();

app.Run();
