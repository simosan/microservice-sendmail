using Microsoft.EntityFrameworkCore;
using SimUserManager.Models;
using SimUserManager.Services;
using Amazon.XRay.Recorder.Core;

var builder = WebApplication.CreateBuilder(args);

// Initialization of AWS X-Ray Trace
AWSXRayRecorder.InitializeInstance();

// Add services to the container.
builder.Services.AddControllersWithViews();
// Add Injection of RepositoryClass by sim
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPositionService, PositionService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
// Configure IConfiguration for dependency injection
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// DB Connection
if (builder.Environment.IsDevelopment())
{
    // Development -> Sqlite
    builder.Services.AddDbContext<UsermanagerContext>(  
        options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
    );
} else if (builder.Environment.IsProduction())
{
    // Production -> Postgresql
    builder.Services.AddDbContext<UsermanagerContext>(   
        options => options.UseNpgsql(builder.Configuration
            .GetConnectionString("DefaultConnection"))
            .AddXRayInterceptor(true)
    );
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseXRay("SimUserManager");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users}/{action=Index}/{id?}" 
);
app.MapControllerRoute(
    name: "Position",
    pattern: "{controller=Positions}/{action=Index}/{id?}"
);
app.MapControllerRoute(
    name: "Department",
    pattern: "{controller=Departments}/{action=Index}/{id?}"
);

app.Run();
