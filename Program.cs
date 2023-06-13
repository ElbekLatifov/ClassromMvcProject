using C.Services;
using ClassRoom.Data.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options=>
{
    options.UseSqlServer("Server=sql.bsite.net\\MSSQL2016; Database=avotest_classroom_database; User Id=avotest_classroom_database; Password=123asd7");
});

builder.Services.AddIdentity<User, IdentityRole<Guid>>(options=>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<GetUserId>();
builder.Services.ConfigureApplicationCookie(options => 
{
    options.LoginPath = "/User/SignIn";
    options.LogoutPath = "/User/SignUp";
});

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
