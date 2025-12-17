using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using RestaurantOrderManager.Backend.Grpc;
using RestaurantOrderManager.Backend.Repositories;

namespace RestaurantOrderManager.Backend.Services;

public class OrdersService(ILogger<MenuService> logger, IOrdersRepository ordersRepository) : Orders.OrdersBase
{
    public override async Task<PlaceOrderResponse> PlaceOrder(PlaceOrderRequest request, ServerCallContext context)
    {
        var order = await ordersRepository.PlaceOrder(request, CancellationToken.None);
        return new PlaceOrderResponse
        {
            OrderId = order.OrderId
        };
    }

    public override async Task<GetOrderStatusResponse> GetOrderStatus(GetOrderStatusRequest request, ServerCallContext context)
    {
        var status = await ordersRepository.GetOrderStatus(Guid.Parse(request.OrderId), CancellationToken.None);
        return new GetOrderStatusResponse
        {
            OrderId = request.OrderId,
            Status = status
        };
    }

    public override async Task<GetLiveOrdersForTableResponse> GetLiveOrdersForTable(GetLiveOrdersForTableRequest request, ServerCallContext context)
    {
        var orders = await ordersRepository.GetLiveOrdersForTable(request.TableId, CancellationToken.None);
        var response =  new GetLiveOrdersForTableResponse
        {
            Timestamp = new Timestamp()
        };
        response.Orders.AddRange(orders);
        return response;
    }

    public override async Task<GetOrderContentsResponse> GetOrderContents(GetOrderContentsRequest request, ServerCallContext context)
    {
        var order = await ordersRepository.GetOrderContents(Guid.Parse(request.OrderId), CancellationToken.None);
        if (order == null) throw new RpcException(new Status(StatusCode.NotFound, $"Order with ID {request.OrderId} not found"));
        return new GetOrderContentsResponse
        {
            Order = order
        }; 
    }

    public override async Task<ChangeOrderStatusResponse> ChangeOrderStatus(ChangeOrderStatusRequest request, ServerCallContext context)
    {
        var newStatus = await ordersRepository.ChangeOrderStatus(Guid.Parse(request.OrderId), request.NewStatus, CancellationToken.None);
        return new ChangeOrderStatusResponse
        {
            OrderId = request.OrderId,
            Timestamp = new Timestamp(),
            UpdatedStatus = newStatus
        };
    }
}