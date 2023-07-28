using System.Security.Claims;
using UserAuthCaller.Controllers;
using UserAuthCaller.Models;
using UserAuthCaller.Repository;

var builder = WebApplication.CreateBuilder(args);

System.Net.ServicePointManager.ServerCertificateValidationCallback +=
    (sender, certificate, chain, sslPolicyErrors) => true;

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");

builder.Services.AddHttpClient("MyApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:44325/");
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Accept.
    Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue
    ("application/json"));
});

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

builder.Services.AddAuthentication("MyAuthScheme").
    AddCookie("MyAuthScheme", options =>
{
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "MyCookie";
    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest;

});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
});

builder.Services.AddTransient<ILoginController, LoginController>();
builder.Services.AddTransient<ICreateController, CreateController>();
builder.Services.AddTransient<IUpdateController, UpdateController>();
builder.Services.AddTransient<IDeleteController, DeleteController>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/UserDataMvc/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHttpsRedirection();
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        if (!context.Response.Headers.ContainsKey("Cache-Control"))
        {
            // Set cache control to no-cache, no-store, must-revalidate
            context.Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            // Set expires to 0 to force revalidation
            context.Response.Headers.Add("Expires", "0");
            // Set Pragma to no-cache for backwards compatibility with HTTP/1.0
            context.Response.Headers.Add("Pragma", "no-cache");
        }
        return Task.FromResult(0);
    });
    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();


app.MapFallbackToFile("Login.cshtml");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
