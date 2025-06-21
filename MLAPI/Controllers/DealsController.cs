using Microsoft.AspNetCore.Mvc;
using MLAPI.Services;
using MLAPI.Dtos;

namespace MLAPI.Controllers;

[ApiController]
[Route("api/v1/deals")]
public class DealsController : ControllerBase
{
    private readonly MercadoLibreApiClient _mercadoLibreApiClient;
    private static readonly Dictionary<string, string> CategoryMappings = new()
    {
        { "laptops", "MLM1650" },
        { "celulares", "MLM1051" },
        { "videojuegos", "MLM1144" },
        { "monitores", "MLM1652" }
    };

    public DealsController(MercadoLibreApiClient mercadoLibreApiClient)
    {
        _mercadoLibreApiClient = mercadoLibreApiClient;
    }

    [HttpGet("{categoryAlias}")]
    public async Task<ActionResult<IEnumerable<MercadoLibreItemDto>>> GetDeals(string categoryAlias)
    {
        if (string.IsNullOrWhiteSpace(categoryAlias) || !CategoryMappings.TryGetValue(categoryAlias.ToLowerInvariant(), out var categoryId))
        {
            return BadRequest($"Invalid or unsupported category alias: {categoryAlias}. Supported aliases are: {string.Join(", ", CategoryMappings.Keys)}");
        }

        try
        {
            var deals = await _mercadoLibreApiClient.GetDealsByCategoryAsync(categoryId);
            // Por ahora, DealDto es el mismo que MercadoLibreItemDto.
            // Si se necesitara un mapeo a un DealDto específico para la respuesta, se haría aquí.
            return Ok(deals);
        }
        catch (HttpRequestException ex)
        {
            // Log the exception (aquí solo lo imprimimos en consola)
            Console.WriteLine($"Error calling MercadoLibre API: {ex.Message}");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Error communicating with external service.");
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
}
