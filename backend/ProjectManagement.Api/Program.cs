<<<<<<< ours
<<<<<<< ours
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
=======
>>>>>>> theirs
=======
>>>>>>> theirs
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectManagement.Api.Data;
using ProjectManagement.Api.Models;
using ProjectManagement.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddScoped<IAuthService, AuthService>();
<<<<<<< ours
<<<<<<< ours
builder.Services.AddScoped<IAdminManagementService, AdminManagementService>();
=======
>>>>>>> theirs
=======
>>>>>>> theirs

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()!;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key))
        };
<<<<<<< ours
<<<<<<< ours

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var userIdValue = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? context.Principal?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

                if (!Guid.TryParse(userIdValue, out var userId))
                {
                    context.Fail("User token is invalid.");
                    return;
                }

                var db = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();
                var user = await db.Users.AsNoTracking().FirstOrDefaultAsync(item => item.Id == userId);

                if (user is null || !user.IsLoginAllowed)
                {
                    context.Fail("User login is blocked.");
                    return;
                }

                var company = await db.Companies.AsNoTracking().FirstOrDefaultAsync(item => item.Code == user.CompanyCode);
                if (company is null || !company.IsLoginAllowed)
                {
                    context.Fail("Company login is blocked.");
                }
            }
        };
=======
>>>>>>> theirs
=======
>>>>>>> theirs
    });

builder.Services.AddAuthorization(options =>
{
<<<<<<< ours
<<<<<<< ours
    options.AddPolicy("GlobalAdminOnly", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole(RoleNames.Admin) &&
            string.Equals(
                context.User.FindFirst(CustomClaimTypes.CompanyCode)?.Value,
                CompanyCodes.Global,
                StringComparison.OrdinalIgnoreCase)));

    options.AddPolicy("CanManagePortal", policy =>
        policy.RequireClaim(CustomClaimTypes.Permission, PermissionNames.ManagePortal));

    options.AddPolicy("CanManageEmployees", policy =>
        policy.RequireClaim(CustomClaimTypes.Permission, PermissionNames.ManageEmployees));

    options.AddPolicy("CanManageCustomers", policy =>
        policy.RequireClaim(CustomClaimTypes.Permission, PermissionNames.ManageCustomers));
=======
=======
>>>>>>> theirs
    options.AddPolicy("CanManagePortal", policy =>
        policy.RequireRole(RoleNames.Admin, RoleNames.PortalAdmin, RoleNames.CustomerEmployee));

    options.AddPolicy("CanManageEmployees", policy =>
        policy.RequireRole(RoleNames.Admin, RoleNames.CustomerAdmin));

    options.AddPolicy("CanManageCustomers", policy =>
        policy.RequireRole(RoleNames.Admin, RoleNames.PortalEmployee));
<<<<<<< ours
>>>>>>> theirs
=======
>>>>>>> theirs
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", cors =>
        cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("frontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
<<<<<<< ours
<<<<<<< ours
    DatabaseInitializer.EnsureDatabaseObjects(db);
=======
>>>>>>> theirs
=======
>>>>>>> theirs
    DbSeeder.Seed(db);
}

app.Run();
