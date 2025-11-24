using SmartLibraryManagementSystemWebApp;

var builder = WebApplication.CreateBuilder(args);
string redisConn = "localhost:6379";

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddHttpClient("LibraryApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5138");
});
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
app.UseExceptionHandler("/Error");
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapRazorPages();

app.UseMiddleware<CheckOverdueReservationMiddleware>();

app.Run();
