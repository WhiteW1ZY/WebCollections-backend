using backend;
using backend.DB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using backend.Service;
using backend.Service.Fields;
using backend.Filters;
using backend.Filters.FieldFilters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ApplicationContext>(
    options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")
        ));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,

            ValidateLifetime = true,

            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddCors();
builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<CollectionAccessControllerFilter>();
builder.Services.AddScoped<CommentAccessControllerFilter>();
builder.Services.AddScoped<ItemAccessControllerFilter>();
builder.Services.AddScoped<UserAccessControllerFilter>();
builder.Services.AddScoped<ReactionAccessControllerFilter>();

builder.Services.AddScoped<BooleanFieldAccessControllerFilter>();
builder.Services.AddScoped<IntegerFieldAccessControllerFilter>();
builder.Services.AddScoped<StringFieldAccessControllerFilter>();
builder.Services.AddScoped<DateFieldAccessControllerFilter>();

builder.Services.AddScoped<ApplicationContext>();

builder.Services.AddScoped<AuthentificationService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<TagService>();

builder.Services.AddScoped<CollectionService>();

builder.Services.AddScoped<ItemService>();

builder.Services.AddScoped<CommentService>();
builder.Services.AddScoped<ReactionService>();

builder.Services.AddScoped<IntegerFieldService>();
builder.Services.AddScoped<DateFieldService>();
builder.Services.AddScoped<BooleanFieldService>();
builder.Services.AddScoped<StringFieldService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

/*
app.UseCors(policy => policy
    //.WithOrigins("http://localhost:3000", "http://172.20.10.2:3000")
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    );
*/

app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    );

app.UseRouting();

app.UseStaticFiles();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();
