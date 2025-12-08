using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace RestaurantOrderManager.Backend.Services;

public class MenuService(ILogger<MenuService> logger) : Menu.MenuBase 
{
    public override Task<GetMenuResponse> GetMenu(GetMenuRequest request, ServerCallContext context)
    {
        return Task.FromResult(new GetMenuResponse
        {
            Items = { new MenuItem { Name = "Pizza" } },
            Timestamp = new Timestamp()
        });
    }
}