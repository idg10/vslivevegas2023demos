using CrashEarlyCrashOften;

var builder = WebApplication.CreateBuilder(args);

MyAppSettings settings = builder.Configuration.GetSection("MyApp").Get<MyAppSettings>();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

app.UseExceptionHandler(settings.Errors.ErrorPage);
app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
