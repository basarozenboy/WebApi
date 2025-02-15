using AutoMapper.Internal;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using WebApi.Helpers;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// add services to DI container
{
    var services = builder.Services;
    var env = builder.Environment;
 
    services.AddDbContext<DataContext>();
    services.AddCors();
    services.AddControllers().AddJsonOptions(x =>
    {
        // serialize enums as strings in api responses (e.g. Role)
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        // ignore omitted parameters on models to enable optional params (e.g. User update)
        x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
    services.AddAutoMapper(cfg => cfg.Internal().MethodMappingEnabled = false, AppDomain.CurrentDomain.GetAssemblies());

    builder.Services.AddSwaggerGen();

    // configure DI for application services
    services.AddScoped<IUserService, UserService>();
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:4000") // Change this to your frontend URL
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); // Needed for cookies
        });
});

// 🔹 Read JWT Key from `appsettings.json`
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new Exception("JWT Key is missing!");
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

// 🔹 Add authentication and specify JWT as the default scheme
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // 🔹 Read token from HTTP-only cookie
                var token = context.Request.Cookies["jwt"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            }
        };
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateIssuer = true,
            ValidateAudience = true, // 🔹 Ensure audience validation is enabled
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"], // 🔹 Must match token audience
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization(); // 🔹 Enable Authorization

var app = builder.Build();

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}


// configure HTTP request pipeline
{
    // global cors policy
    //app.UseCors(x => x
    ////.WithOrigins("")
    //    .AllowAnyOrigin()
    //    .AllowAnyMethod()
    //    .AllowAnyHeader());

    // global error handler
    app.UseMiddleware<ErrorHandlerMiddleware>();

    app.MapControllers();
}

app.Run("http://localhost:9080");