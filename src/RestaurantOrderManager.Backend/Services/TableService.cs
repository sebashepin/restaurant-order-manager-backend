using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using RestaurantOrderManager.Backend.Grpc;

namespace RestaurantOrderManager.Backend.Services;

public class TableService(ILogger<TableService> logger): Table.TableBase
{
    public override Task<GetTableListResponse> GetTableList(GetTableListRequest request, ServerCallContext context)
    {
        var listResponse = new GetTableListResponse
        {
            Timestamp = new Timestamp()
        };
        listResponse.Tables.AddRange(Enumerable.Range(1, 10).Select(i => new TableInfo{Description = $"Table {i}", Seats = 4, TableId = Guid.NewGuid().ToString()}));
        return Task.FromResult(listResponse);
    }
}