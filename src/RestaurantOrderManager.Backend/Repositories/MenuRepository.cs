using System.Text.Json;
using RestaurantOrderManager.Backend.Grpc;

namespace RestaurantOrderManager.Backend.Repositories;

public interface IMenuRepository
{
    Task<IReadOnlyList<MenuItem>> GetMenuItems(CancellationToken cancellationToken = default);
}

public class MenuRepository(ILogger<MenuRepository> logger) : IMenuRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    private const string MenuRelativePath = "Resources\\menu.json";

    public async Task<IReadOnlyList<MenuItem>> GetMenuItems(CancellationToken cancellationToken = default)
    {
        try
        {
            var baseDir = AppContext.BaseDirectory;
            var path = Path.Combine(baseDir, MenuRelativePath);

            if (!File.Exists(path))
            {
                logger.LogWarning("Menu file not found at {Path}", path);
                return [];
            }

            await using var stream = File.OpenRead(path);
            var items = await JsonSerializer.DeserializeAsync<List<MenuItemJson>>(stream, JsonOptions, cancellationToken)
                        ?? new List<MenuItemJson>();

            return items.Select(x => new MenuItem
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                Category = x.Category,
                MainImageUrl = x.MainImageUrl ?? string.Empty,
                ThumbnailImageUrl = x.ThumbnailImageUrl ?? string.Empty
            }).ToList();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to read or parse menu.json");
            return Array.Empty<MenuItem>();
        }
    }

    private sealed class MenuItemJson
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required double Price { get; set; }
        public required string Category { get; set; }
        public string? MainImageUrl { get; set; }
        public string? ThumbnailImageUrl { get; set; }
    }
}