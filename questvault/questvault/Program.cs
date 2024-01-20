using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using questvault.Data;
using questvault.Models;
using questvault.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Google Authentication
builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
  googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
  googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
});

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<User, IdentityRole>(options =>
                              options.SignIn.RequireConfirmedAccount = true)
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddDefaultUI();

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IEmailSender, EmailSender>(i =>
  new EmailSender(
      builder.Configuration["EmailSender:Host"],
      builder.Configuration.GetValue<int>("EmailSender:Port"),
      builder.Configuration.GetValue<bool>("EmailSender:EnableSSL"),
      builder.Configuration["EmailSender:UserName"],
      builder.Configuration["EmailSender:Password"]
  )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseMigrationsEndPoint();
}
else
{
  app.UseExceptionHandler("/Home/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();


app.Run();
