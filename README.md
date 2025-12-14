# Restaurant Order Manager — Backend

[![CI](https://github.com/sebashepin/restaurant-order-manager-backend/actions/workflows/ci.yml/badge.svg)](https://github.com/sebashepin/restaurant-order-manager-backend/actions/workflows/ci.yml)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)]()

Basic ASP.NET Core gRPC backend for a restaurant ordering system. gRPC-Web is enabled for browser clients.

## High-level architecture

```mermaid
flowchart LR
  A["Web / Mobile Client"] -- gRPC-Web / gRPC --> B["ASP.NET Core Backend"]
  B --> C["MenuService"]
  C --> D["Menu Repository"]
  D --> E["Data Store"]

  subgraph Backend
    B
    C
    D
  end
```

## Endpoints (gRPC)

Service: `menu.Menu` (see `src\RestaurantOrderManager.Backend.Proto\Proto\menu.proto`)

- `GetMenu(GetMenuRequest) returns (GetMenuResponse)` — currently implemented.
  - Returns a list of `MenuItem` and a server `Timestamp`.
- `PlaceOrder(PlaceOrderRequest) returns (PlaceOrderResponse)` — defined in proto (not yet implemented).
- `GetOrderStatus(GetOrderStatusRequest) returns (GetOrderStatusResponse)` — defined in proto (not yet implemented).
- `GetLiveOrdersForTable(GetLiveOrdersForTableRequest) returns (GetLiveOrdersForTableResponse)` — defined in proto (not yet implemented).

Transport notes:
- gRPC-Web is enabled, and a permissive CORS policy is configured for development.
- Default dev URLs (from `launchSettings.json`): `http://localhost:5134`, `https://localhost:7088`.
- A plain GET to `/` returns a help message indicating gRPC usage.