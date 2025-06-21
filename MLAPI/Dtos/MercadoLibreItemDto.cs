namespace MLAPI.Dtos;

public record MercadoLibreItemDto(
    string Id,
    string Title,
    decimal Price,
    string CurrencyId,
    string Thumbnail,
    string Permalink
);
