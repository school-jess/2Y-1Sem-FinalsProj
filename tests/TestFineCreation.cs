using System.Net;
using System.Text;
using SmartLibraryManagementSystemWebApp;
using SmartLibraryManagementSystemClassLibrary.Dtos;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Text.Json;
using Moq.Protected;

namespace tests;

public class TestFineCreation
{
    [Fact]
    public async Task TestReservationLateReturn()
    {
        var contextMock = new Mock<HttpContext>();
        var sessionMock = new Mock<ISession>();
        var nextMock = new Mock<RequestDelegate>();

        var loggedInBytes = Encoding.UTF8.GetBytes("true");
        var userIdBytes = Encoding.UTF8.GetBytes("123");
        sessionMock.Setup(s => s.TryGetValue("IsLoggedIn", out loggedInBytes)).Returns(true);
        sessionMock.Setup(s => s.TryGetValue("UserId", out userIdBytes)).Returns(true);
        contextMock.Setup(c => c.Session).Returns(sessionMock.Object);

        var userWithReservations =
            new UserWithReservationsDto(
                1,
                "Test User",
                null,
                null,
                false,
                false,
                [
                    new ReservationUserDto(
                        1,
                        1,
                        new BookUpdateDto(
                            1,
                            "",
                            "",
                            DateTime.Now,
                            "",
                            ""),
                        DateTime.UtcNow.AddDays(-2),
                        new CatalogUpdateDto(
                            1,
                            1,
                            1,
                            "",
                            "",
                            1),
                        null,
                        null,
                        DateTime.UtcNow.AddDays(-1),
                        false,
                        false,
                        false
                    )
                ],
                [],
                [],
                false);
        var handlerMock = new Mock<HttpMessageHandler>();
        string userJson = JsonSerializer.Serialize(userWithReservations);
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(userJson, Encoding.UTF8, "application/json")
            });

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(m =>
                    m.RequestUri.ToString() == "http://localhost:5138/api/Fine" && m.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            });

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(m =>
                    m.RequestUri.ToString() == "http://localhost:5138/api/User" && m.Method == HttpMethod.Put),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            });

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(m =>
                    m.RequestUri.ToString() == "http://localhost:5138/api/Reservation" && m.Method == HttpMethod.Put),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            });

        var httpClient = new HttpClient(handlerMock.Object);
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var middleware = new CheckOverdueReservationMiddleware(nextMock.Object, httpClientFactoryMock.Object);

        await middleware.InvokeAsync(contextMock.Object);

        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Post && req.RequestUri.ToString().Contains("/api/Fine")),
            ItExpr.IsAny<CancellationToken>()
        );

        nextMock.Verify(n => n(contextMock.Object), Times.Once);
    }
}
