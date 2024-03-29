using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using questvault.Data;
using questvault.Models;
using questvault.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Google Authentication
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
      IConfigurationSection googleAuthNSection = configuration.GetSection("Authentication:Google");
      options.ClientId = googleAuthNSection["ClientId"];
      options.ClientSecret = googleAuthNSection["ClientSecret"];
    });

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
  ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
  .AddEntityFrameworkStores<ApplicationDbContext>()
  .AddDefaultTokenProviders()
;

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddControllersWithViews();

// Emailing Service
builder.Services.AddTransient<IEmailSender, EmailSender>(i =>
  new EmailSender(
      configuration["EmailSender:Host"],
      configuration.GetValue<int>("EmailSender:Port"),
      configuration.GetValue<bool>("EmailSender:EnableSSL"),
      configuration["EmailSender:UserName"],
      configuration["EmailSender:Password"]
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
