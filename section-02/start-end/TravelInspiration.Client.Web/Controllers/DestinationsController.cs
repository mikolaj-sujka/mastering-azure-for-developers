using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using TravelInspiration.Client.Web.Models.Dto;

namespace TravelInspiration.Client.Web.Controllers;

public class DestinationsController(IHttpClientFactory httpClientFactory, ITokenAcquisition tokenAcquisition, 
    IConfiguration configuration) : Controller
{
    public IActionResult Index()
    { 
        return View(new List<DestinationDto>());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SearchDestinations(string searchFor)
    {
        var scope = configuration["DestinationsApi:Scopes"] ?? throw new InvalidOperationException("DestinationsApi:Scope is not configured.");

        var accessToken = await tokenAcquisition.GetAccessTokenForAppAsync(scope);

        var destinationsApiClient = httpClientFactory.CreateClient("DestinationsApiClient");
        var destinations = await destinationsApiClient
            .GetFromJsonAsync<List<DestinationDto>>($"api/destinations?searchFor={searchFor}");

        return View("Index", destinations);
    }
}
