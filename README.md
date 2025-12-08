# Restaurant Order Manager – Backend

Simple ASP.NET Core gRPC backend for a restaurant order manager.

## What’s exposed
- gRPC (native) endpoints
- gRPC-Web endpoints for browser-based clients

The service contract is defined in the proto file:
- `src/RestaurantOrderManager.Backend/Protos/menu.proto`

## Build
Prerequisites:
- .NET SDK 10.0 or later installed

Commands (from repository root):
```bash
dotnet restore
dotnet build
```

## Run locally
Run the backend project:
```bash
dotnet run --project src/RestaurantOrderManager.Backend
```

By default, the app enables both gRPC and gRPC-Web middleware. The default CORS policy is configured in `Program.cs`.

### Important: HTTPS is required for gRPC-Web from browsers
- When calling via gRPC-Web from a browser, requests are subject to CORS and include preflight `OPTIONS` checks.
- Most browsers will block gRPC-Web cross-origin calls over HTTP. Use HTTPS to satisfy CORS and secure-context requirements.
- Make sure you have a valid dev certificate and the app listens on an HTTPS URL.

Quick tips for local HTTPS:
- Trust ASP.NET Core dev cert: `dotnet dev-certs https --trust`
- Use the HTTPS profile/URL defined in `src/RestaurantOrderManager.Backend/Properties/launchSettings.json`

## Clients
- Native gRPC clients can connect to the gRPC endpoint (HTTP/2 over TLS).
- Web clients (e.g., SPA/Blazor/React) should use a gRPC-Web compatible client (such as `Grpc.Net.Client.Web`) and connect over HTTPS.

## Reference
- Proto definitions: `src/RestaurantOrderManager.Backend/Protos/menu.proto`