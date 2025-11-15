using SmartLibraryManagementSystemClassLibrary.Dtos;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;

namespace SmartLibraryManagementSystemWebApp.Pages;

public class BookModel : PageModel
{
    public BookGet1Dto Book { get; set; }
    public bool HasFine { get; set; }

    public class InputModel
    {
        public bool Borrow { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        using (var httpClient = new HttpClient())
        {
            var getBook = await httpClient.GetAsync($"http://localhost:5138/api/Book/{id}");
            if (!getBook.IsSuccessStatusCode) return NotFound();
            var getBookCont = await getBook.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Book = JsonSerializer.Deserialize<BookGet1Dto>(getBookCont, options);
            if (HttpContext.Session.GetString("IsLoggedIn") != "true") return Page();
            var getUser =
                await httpClient.GetAsync($"http://localhost:5138/api/User/{HttpContext.Session.GetString("UserId")}?withReservation=false");
            if (!getUser.IsSuccessStatusCode) return NotFound();
            var getUserCont = await getUser.Content.ReadAsStringAsync();
            UserGet1Dto user = JsonSerializer.Deserialize<UserGet1Dto>(getUserCont);
            HasFine = user.HasFine;
        }

        return Page();
    }

    public async Task<IActionResult> OnPosAsync()
    {
        if (HttpContext.Session.GetString("IsLoggedIn") != "true") return Page();
        using (var httpClient = new HttpClient())
        {
            int bookId = int.Parse(RouteData.Values["id"].ToString());
            int userId = int.Parse(HttpContext.Session.GetString("UserId"));
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var getBook = await httpClient.GetAsync($"http://localhost:5138/api/Book/{bookId}");
            if (!getBook.IsSuccessStatusCode) throw new InvalidOperationException("couldn't get book");
            string getBookCont = await getBook.Content.ReadAsStringAsync();
            BookGet1Dto book = JsonSerializer.Deserialize<BookGet1Dto>(getBookCont, options);
            if (book.Catalog.Copies - book.Catalog.CopiesBorrowed < 0)
                return Page(); // todo return a reason why coudn't borrow
            if (HasFine)
            {
                ReservationCreationDto reservation = new ReservationCreationDto(
                    userId,
                    bookId,
                    DateTime.UtcNow,
                    book.Catalog.CatalogId,
                    DateTime.UtcNow.AddDays(7),
                    false,
                    false,
                    true);
                string reservationSerialized = JsonSerializer.Serialize(reservation);
                var reservationHttpCont = new StringContent(reservationSerialized, Encoding.UTF8, "application/json");
                var newReservation =
                    await httpClient.PostAsync("http://localhost:5138/api/Reservation", reservationHttpCont);
                if (!newReservation.IsSuccessStatusCode)
                    throw new InvalidOperationException("error creating new reservation");
                string newReservationCont = await newReservation.Content.ReadAsStringAsync();
                ReservationUpdateDto insertedReservation = JsonSerializer.Deserialize<ReservationUpdateDto>(newReservationCont, options);
                var getUser = await httpClient.GetAsync($"http://localhost:5138/api/User/{userId}?withReservation=true");
                if (!getUser.IsSuccessStatusCode) throw new InvalidOperationException("couldn't get user");
                string getUserCont = await getUser.Content.ReadAsStringAsync();
                UserWithReservationsDto user = JsonSerializer.Deserialize<UserWithReservationsDto>(getUserCont, options);
                int loanAmt = 0;
                foreach (var userReservation in user.Reservations)
                {
                    if (userReservation.HasFine)
                    {
                        if (!userReservation.Fine.HasPayed) continue;
                        loanAmt += userReservation.Fine.FineAmount;
                    }
                    if (userReservation.HasLoan)
                    {
                        if (!userReservation.Loan.HasPayed) continue;
                        loanAmt += userReservation.Loan.LoanAmount;
                    }
                }
                LoanCreationDto loan = new LoanCreationDto(
                    loanAmt,
                    userId,
                    insertedReservation.ReservationId,
                    false);
                string loanSerialized = JsonSerializer.Serialize(loan);
                var loanHttpCont = new StringContent(loanSerialized, Encoding.UTF8, "application/json");
                var newLoan = await httpClient.PostAsync("http://localhost:5138/api/Loan", loanHttpCont);
                if (!newLoan.IsSuccessStatusCode) throw new InvalidOperationException("error when creating loan");
            }
            else
            {
                ReservationCreationDto reservation = new ReservationCreationDto(
                    userId,
                    bookId,
                    DateTime.UtcNow,
                    book.Catalog.CatalogId,
                    DateTime.UtcNow.AddDays(7),
                    false,
                    false,
                    false);
                string reservationSerialized = JsonSerializer.Serialize(reservation);
                var reservationHttpCont = new StringContent(reservationSerialized, Encoding.UTF8, "application/json");
                var newReservation =
                    await httpClient.PostAsync("http://localhost:5138/api/Reservation", reservationHttpCont);
                if (!newReservation.IsSuccessStatusCode)
                    throw new InvalidOperationException("error creating new reservation");
            }
        }

        return Page();
    }
}

