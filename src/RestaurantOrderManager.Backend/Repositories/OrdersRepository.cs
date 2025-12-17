using System.Collections.Concurrent;
using RestaurantOrderManager.Backend.Grpc;

namespace RestaurantOrderManager.Backend.Repositories;

public interface IOrdersRepository
{
    Task<Order?> GetOrderContents(Guid orderId, CancellationToken cancellationToken = default);
    Task<OrderStatus> GetOrderStatus(Guid orderId, CancellationToken cancellationToken = default);
    Task<OrderStatus> ChangeOrderStatus(Guid orderId, OrderStatus newStatus, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Order>> GetLiveOrdersForTable(string tableId, CancellationToken cancellationToken = default);
    Task<PlaceOrderResponse> PlaceOrder(PlaceOrderRequest request, CancellationToken cancellationToken = default);
}

public partial class OrdersRepository(ILogger<OrdersRepository> logger) : IOrdersRepository
{
    private readonly ConcurrentDictionary<Guid, Order> _orders = new();

    public Task<Order?> GetOrderContents(Guid orderId, CancellationToken cancellationToken = default)
    {
        if (_orders.TryGetValue(orderId, out var order))
        {
            return Task.FromResult<Order?>(order);
        }

        return Task.FromResult<Order?>(null);
    }

    public Task<OrderStatus> GetOrderStatus(Guid orderId, CancellationToken cancellationToken = default)
    {
        if (_orders.TryGetValue(orderId, out var order))
        {
            return Task.FromResult(order.Status);
        }
        return Task.FromResult(OrderStatus.Unspecified);
    }

    public Task<OrderStatus> ChangeOrderStatus(Guid orderId, OrderStatus newStatus, CancellationToken cancellationToken = default)
    {
        if (_orders.TryGetValue(orderId, out var order))
        {
            order.Status = newStatus;
            return Task.FromResult(order.Status);
        }
        return Task.FromResult(OrderStatus.Unspecified);
    }

    public Task<IReadOnlyList<Order>> GetLiveOrdersForTable(string tableId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<PlaceOrderResponse> PlaceOrder(PlaceOrderRequest request, CancellationToken cancellationToken = default)
    {
        var orderGuid = Guid.Parse(request.Order.OrderId);
        request.Order.Status = OrderStatus.Sent;
        _orders.TryAdd(orderGuid, request.Order);
        LogDebug(logger, orderGuid);
        return Task.FromResult(new PlaceOrderResponse {OrderId = orderGuid.ToString()});
    }

    [LoggerMessage(LogLevel.Debug, "added order {OrderId}")]
    static partial void LogDebug(ILogger<OrdersRepository> logger, Guid OrderId);
}