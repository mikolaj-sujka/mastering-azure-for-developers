using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using TravelInspiration.Client.Web.Models.Dto;

namespace TravelInspiration.Client.Web.Controllers;

public class ItinerariesController(IHttpClientFactory httpClientFactory, ITokenAcquisition tokenAcquisition,
    IConfiguration configuration) : Controller
{

    public IActionResult Index()
    {      
        return View(new List<ItineraryDto>());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SearchItineraries(string searchFor)
    {
        var scope = configuration["ItinerariesApi:Scopes"] 
                    ?? throw new InvalidOperationException("Missing configuration value: ItinerariesApi:Scopes");

        var accessToken = await tokenAcquisition.GetAccessTokenForAppAsync(scope);
        var itinerariesApiClient = httpClientFactory.CreateClient("ItinerariesApiClient");
        var itineraries = await itinerariesApiClient.
            GetFromJsonAsync<List<ItineraryDto>>($"api/itineraries?searchFor={searchFor}");
        return View("Index", itineraries);
    }
}
