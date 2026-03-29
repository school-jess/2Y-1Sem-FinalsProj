using System.Text;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SmartLibraryManagementSystemClassLibrary.Dtos;

namespace SmartLibraryManagementSystemWebApp.Pages;

public class UserModel : PageModel
{
    public UserWithReservationsDto User { get; set; }
    public bool IsFaculty { get; set; }
    private readonly IHttpClientFactory _httpClientFactory;
    [BindProperty] public InputModel Input { get; set; }

    public class InputModel
    {
        public int IsDeleting { get; set; }
        public int IsFine { get; set; }
        public int FineId { get; set; }
        public int LoanId { get; set; }
        public int IsReserve { get; set; }
        public int ReservationId { get; set; }
    }

    public UserModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        if (HttpContext.Session.GetString("IsLoggedIn") != "true") return NotFound();
        if (HttpContext.Session.GetString("UserId") != $"{id}") return NotFound();
        IsFaculty = HttpContext.Session.GetString("IsEducator") == "true";
        using (var httpClient = _httpClientFactory.CreateClient("LibraryApi"))
        {
            var getUser = await httpClient.GetAsync($"http://localhost:5138/api/User/{id}?withReservation=true");
            if (!getUser.IsSuccessStatusCode) throw new InvalidOperationException("error getting user");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var getUserContent = await getUser.Content.ReadAsStringAsync();
            User = JsonSerializer.Deserialize<UserWithReservationsDto>(getUserContent, options);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (!ModelState.IsValid) return RedirectToPage();
        if (HttpContext.Session.GetString("IsLoggedIn") != "true") return NotFound();
        using (var httpClient = _httpClientFactory.CreateClient("LibraryApi"))
        {
            if (Input.IsDeleting == 1)
            {
                var deleteUser =
                    await httpClient.DeleteAsync(
                        $"http://localhost:5138/api/User/{HttpContext.Session.GetString("UserId")}");
                if (!deleteUser.IsSuccessStatusCode) throw new InvalidOperationException("couldn't delete user");
            }
            else
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                if (Input.IsReserve == 1)
                {
                    var getReservation =
                        await httpClient.GetAsync($"http://localhost:5138/api/Reservation/{Input.ReservationId}");
                    if (!getReservation.IsSuccessStatusCode)
                        throw new InvalidOperationException("couldn't get reservation");
                    string getReservationContent = await getReservation.Content.ReadAsStringAsync();
                    ReservationGet1Dto reservation =
                        JsonSerializer.Deserialize<ReservationGet1Dto>(getReservationContent, options);
                    ReservationUpdateDto reservationToUpdate = new ReservationUpdateDto(
                        reservation.ReservationId,
                        reservation.User.UserId,
                        reservation.Book.BookId,
                        reservation.ReservationDateTime,
                        reservation.Catalog.CatalogId,
                        reservation.ReservationReturnDateTime,
                        reservation.HasFine,
                        true,
                        reservation.HasLoan);
                    string reservationToUpdateSerialized = JsonSerializer.Serialize(reservationToUpdate);
                    var reservationToUpdateHttpCont = new StringContent(reservationToUpdateSerialized, Encoding.UTF8,
                        "application/json");
                    var updateReservation = await httpClient.PutAsync(
                        $"http://localhost:5138/api/Reservation/{Input.ReservationId}", reservationToUpdateHttpCont);
                    if (!updateReservation.IsSuccessStatusCode) throw new InvalidOperationException("couldn't update reservation");
                    return Page();
                }

                if (Input.IsFine == 1)
                {
                    var getFine = await httpClient.GetAsync($"http://localhost:5138/api/Fine/{Input.FineId}");
                    if (!getFine.IsSuccessStatusCode) throw new InvalidOperationException("couldn't get fine");
                    string getFineContent = await getFine.Content.ReadAsStringAsync();
                    FineGet1Dto fine = JsonSerializer.Deserialize<FineGet1Dto>(getFineContent, options);
                    FineUpdateDto fineToUpdate = new FineUpdateDto(
                        fine.FineId,
                        fine.FineAmount,
                        fine.User.UserId,
                        fine.Reservation.ReservationId,
                        true);
                    string fineToUpdateSerialized = JsonSerializer.Serialize(fineToUpdate);
                    var fineToUpdateHttpCont =
                        new StringContent(fineToUpdateSerialized, Encoding.UTF8, "application/json");
                    var updateFine = await httpClient.PutAsync($"http://localhost:5138/api/Fine/{Input.FineId}",
                        fineToUpdateHttpCont);
                    if (!updateFine.IsSuccessStatusCode) throw new InvalidOperationException("couldn't update fine");
                }
                else
                {
                    var getLoan = await httpClient.GetAsync($"http://localhost:5138/api/Loan/{Input.LoanId}");
                    if (!getLoan.IsSuccessStatusCode) throw new InvalidOperationException("couldn't get loan");
                    string getLoanContent = await getLoan.Content.ReadAsStringAsync();
                    LoanGet1Dto loan = JsonSerializer.Deserialize<LoanGet1Dto>(getLoanContent, options);
                    LoanUpdateDto loanToUpdate = new LoanUpdateDto(
                        loan.LoanId,
                        loan.LoanAmount,
                        loan.User.UserId,
                        loan.Reservation.ReservationId,
                        loan.HasPayed);
                    string loanToUpdateSerialized = JsonSerializer.Serialize(loanToUpdate);
                    var loanToUpdateHttpCont =
                        new StringContent(loanToUpdateSerialized, Encoding.UTF8, "application/json");
                    var updateLoan = await httpClient.PutAsync($"http://localhost:5138/api/Loan/{Input.LoanId}",
                        loanToUpdateHttpCont);
                    if (!updateLoan.IsSuccessStatusCode) throw new InvalidOperationException("couldn't update loan");
                }
            }
        }

        return Page();
    }
}
