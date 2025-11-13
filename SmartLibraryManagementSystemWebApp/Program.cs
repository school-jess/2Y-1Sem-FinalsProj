using System.Text;
using SmartLibraryManagementSystemClassLibrary.Dtos;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession();
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

app.Use(async (context, next) =>
{
    if (context.Session.GetString("IsLoggedIn") != "true")
    {
        await next(context);
        return;
    }

    using (var httpClient = new HttpClient())
    {
        var getUser =
            await httpClient.GetAsync(
                $"http://localhost:5138/api/User/{context.Session.GetString("UserId")}?withReservation=true");
        if (!getUser.IsSuccessStatusCode)
        {
            await next(context);
            return;
        }

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        string getUserCont = await getUser.Content.ReadAsStringAsync();
        UserWithReservationsDto user = JsonSerializer.Deserialize<UserWithReservationsDto>(getUserCont, options);
        foreach (var reservation in user.Reservations)
        {
            if (reservation.ReservationReturnDateTime < DateTime.UtcNow)
            {
                // create a new Fine
                FineCreationDto fine = new FineCreationDto(10, reservation.UserId, false, reservation.ReservationId);
                string fineSerialized = JsonSerializer.Serialize(fine);
                var fineHttpCont = new StringContent(fineSerialized, Encoding.UTF8, "application/json");
                var newFine = await httpClient.PostAsync("http://localhost:5138/api/Fine", fineHttpCont);
                if (!newFine.IsSuccessStatusCode) throw new InvalidOperationException("couldn't create new user");
                // update user.HasFine to true
                UserUpdateDto userToUpdate =
                    new UserUpdateDto(user.UserId, user.UserName, true, user.HasLoan, user.IsAdmin);
                string userToUpdateSerialized = JsonSerializer.Serialize(userToUpdate);
                var userToUpdateHttpCont = new StringContent(userToUpdateSerialized, Encoding.UTF8, "application/json");
                var updateUser = await httpClient.PutAsync($"http://localhost:5138/api/User/", userToUpdateHttpCont);
                if (!updateUser.IsSuccessStatusCode) throw new InvalidOperationException("error updating user");
            }
        }
    }

    await next(context);
});

app.Run();
