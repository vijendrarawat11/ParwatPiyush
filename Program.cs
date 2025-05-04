using ParwatPiyushNewsPortal.Models;
using ParwatPiyushNewsPortal.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Diagnostics;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ParwatPiyushDB>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Cookie expires in 30 min
        options.SlidingExpiration = true; // Renew cookie on activity
        options.Cookie.HttpOnly = true; // Prevent JavaScript access (more secure)
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Use only HTTPS
        options.Cookie.SameSite = SameSiteMode.Strict; // Prevent cross-site access
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("AuthorOnly", policy => policy.RequireRole("Author"));
});
builder.Services.AddSession();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//builder.Services.AddAuthentication().AddCookie();
//builder.Services.AddAuthorization();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.Use(async (context, next) =>
{
    if (context.User.Identity.IsAuthenticated)
    {
        if (!context.User.Identity.IsAuthenticated && context.Request.Path.StartsWithSegments("/Author"))
        {
            Debug.WriteLine("Unauthorized access attempt detected!");
            context.Response.Redirect("/Account/Login");
            return;
        }
        string sessionIdFromCookie = context.User.FindFirst("SessionId")?.Value;
        string sessionIdFromSession = context.Session.GetString("SessionId");

        // If session ID does not match, log the user out
        if (sessionIdFromSession == null || sessionIdFromSession != sessionIdFromCookie)
        {
            Debug.WriteLine("Session mismatch! Logging out user.");
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            context.Response.Redirect("/Account/Login");
            return;
        }
    }

    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ParwatPiyushDB>();
    //dbContext.Topics.RemoveRange(dbContext.Topics);
    //dbContext.SaveChanges();
    //if (!dbContext.Topics.Any())
    //{
    var existingTopics = dbContext.Topics.Select(t => t.Name).ToHashSet();
    //dbContext.Topics.AddRange(
    var newTopics = new List<Topics>
      {
            new Topics { Name = "उत्तराखण्ड" },
            new Topics { Name = "देश-विदेश" },
            new Topics { Name = "खेल" },
            new Topics { Name = "धर्म" },
            new Topics { Name = "फैशन" },
            /*new Topics { Name = "राजनीति" },
            new Topics { Name = "पर्यावरण" },
            new Topics { Name = "अपराध" },
            new Topics { Name = "विरासत" },
            new Topics { Name = "नौकरशाही" }, */

    };
        //);
    // Add only if the topic does not already exist
    foreach (var topic in newTopics)
    {
        if (!existingTopics.Contains(topic.Name))
        {
            dbContext.Topics.Add(topic);
        }
    }
    dbContext.SaveChanges();
    //}
}


app.Run();
