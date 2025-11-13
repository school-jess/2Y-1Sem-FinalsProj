using SmartLibraryManagementSystemClassLibrary.Dtos;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;

namespace SmartLibraryManagementSystemWebApp.Pages;

public class BookModel : PageModel
{
    public BookGet1Dto Book { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        using (var httpClient = new HttpClient())
        {
            var resp = await httpClient.GetAsync($"http://localhost:5138/api/Book/{id}");
            if (resp.IsSuccessStatusCode)
            {
                var respCont = await resp.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                Book = JsonSerializer.Deserialize<BookGet1Dto>(respCont, options);
            }
            else return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPosAsync()
    {
        if (HttpContext.Session.GetString("IsLoggedIn") != "true") return Page();
        using (var httpClient = new HttpClient())
        {
            int bookId = int.Parse(RouteData.Values["id"].ToString());
            string userId = HttpContext.Session.GetString("UserId");
            var getUser = await httpClient.GetAsync($"http://localhost:5138/api/User/{userId}?withReservation=false");
            if (!getUser.IsSuccessStatusCode) throw new InvalidOperationException("couldn't get user");
            string getUserCont = await getUser.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            UserGet1Dto user = JsonSerializer.Deserialize<UserGet1Dto>(getUserCont, options);
            var getBook = await httpClient.GetAsync($"http://localhost:5138/api/Book/{bookId}");
            if (!getBook.IsSuccessStatusCode) throw new InvalidOperationException("couldn't get book");
            string getBookCont = await getBook.Content.ReadAsStringAsync();
            BookGet1Dto book = JsonSerializer.Deserialize<BookGet1Dto>(getBookCont, options);
            if (book.Catalog.Copies - book.Catalog.CopiesBorrowed < 0)
                return Page(); // todo return a reason why coudn't borrow
            ReservationCreationDto reservation = new ReservationCreationDto(
                user.UserId,
                bookId,
                DateTime.UtcNow,
                book.Catalog.CatalogId,
                DateTime.UtcNow.AddDays(7),
                false,
                false);
            string reservationSerialized = JsonSerializer.Serialize(reservation);
            var reservationHttpCont = new StringContent(reservationSerialized, Encoding.UTF8, "application/json");
            var newReservation =
                await httpClient.PostAsync("http://localhost:5138/api/Reservation", reservationHttpCont);
            if (!newReservation.IsSuccessStatusCode)
                throw new InvalidOperationException("error creating new reservation");
        }

        return Page();
    }
}

