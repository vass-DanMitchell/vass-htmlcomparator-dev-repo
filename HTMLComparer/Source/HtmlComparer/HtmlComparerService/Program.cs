using HtmlComparerService;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    EnvironmentName = Environments.Staging
});

// Add services to the container.
Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production");
builder.Services.AddControllersWithViews();
builder.Services.AddLocalization();
var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var settings = builder.Configuration.GetSection("AppSettingsVariables").Get<AppSettingsVariables>();

app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();;
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Demo}/{action=Demo}/{id?}");

app.Run();
 