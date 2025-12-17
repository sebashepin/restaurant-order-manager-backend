using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using RestaurantOrderManager.Backend.Grpc;
using RestaurantOrderManager.Backend.Repositories;

namespace RestaurantOrderManager.Backend.Services;

public class MenuService(ILogger<MenuService> logger, IMenuRepository menuRepository) : Menu.MenuBase
{
    public override async Task<GetMenuResponse> GetMenu(GetMenuRequest request, ServerCallContext context)
    {
        var items = await menuRepository.GetMenuItems(context.CancellationToken);
        return new GetMenuResponse
        {
            Items = { items },
            Timestamp = Timestamp.FromDateTime(DateTime.UtcNow)
        };
    }
}