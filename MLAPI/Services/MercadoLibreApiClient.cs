using System.Net.Http.Json;
using MLAPI.Dtos;

namespace MLAPI.Services;

// DTO para la estructura de respuesta de la API de búsqueda de Mercado Libre
public class MercadoLibreSearchResponseDto
{
    public List<MercadoLibreItemDto> Results { get; set; } = new List<MercadoLibreItemDto>();
}

public class MercadoLibreApiClient
{
    private readonly HttpClient _httpClient;

    public MercadoLibreApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        // La BaseAddress ya debería estar configurada desde Program.cs
    }

    public async Task<IEnumerable<MercadoLibreItemDto>> GetDealsByCategoryAsync(string categoryId)
    {
        if (string.IsNullOrWhiteSpace(categoryId))
        {
            throw new ArgumentException("Category ID cannot be null or whitespace.", nameof(categoryId));
        }

        // Endpoint: GET /sites/MLM/search?deal=yes&category={categoryId}&sort=price_asc
        var requestUri = $"/sites/MLM/search?deal=yes&category={categoryId}&sort=price_asc";

        try
        {
            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode(); // Lanza una excepción si el código de estado no es exitoso.

            // Deserializa la respuesta completa que contiene la lista "results"
            var searchResponse = await response.Content.ReadFromJsonAsync<MercadoLibreSearchResponseDto>();

            return searchResponse?.Results ?? Enumerable.Empty<MercadoLibreItemDto>();
        }
        catch (HttpRequestException ex)
        {
            // Aquí se podría registrar el error con un logger
            Console.WriteLine($"Error fetching data from MercadoLibre API: {ex.Message}");
            throw; // Relanzar para que el controlador pueda manejarlo o devolver un error apropiado
        }
        catch (JsonException ex)
        {
            // Registrar el error de deserialización
            Console.WriteLine($"Error deserializing MercadoLibre API response: {ex.Message}");
            throw;
        }
    }
}
