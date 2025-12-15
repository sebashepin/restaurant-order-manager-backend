using Common.Logging;
using Common.Logging.Simple;
using Makaretu.Dns;
using RestaurantOrderManager.Backend;
using RestaurantOrderManager.Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
const string wasmCorsPolicyName = "AllowWasm";
builder.Services.AddCors(o =>
{
    o.AddPolicy(wasmCorsPolicyName, p => p
        //.WithOrigins("http://localhost:5271", "https://localhost:7822", "http://192.168.0.206:5271", "https://192.168.0.206:7822")
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
        /*.WithHeaders(
            "content-type",
            "x-grpc-web",
            "grpc-timeout",
            "authorization",
            "grpc-accept-encoding",
            "accept",
            "user-agent")*/
        //.WithExposedHeaders("grpc-status", "grpc-message", "grpc-encoding", "grpc-accept-encoding")
        .DisallowCredentials());
    o.AddDefaultPolicy( p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting();
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
app.UseCors(wasmCorsPolicyName);
app.MapGrpcService<MenuService>().EnableGrpcWeb().RequireCors(wasmCorsPolicyName);
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

if (app.Environment.IsDevelopment()) {
    var serviceDiscover = new ServiceDiscovery();
    SetupMdnsRecord(serviceDiscover, new ServiceProfile("restaurant", "orders._tcp", 7088));
    SetupMdnsRecord(serviceDiscover, new ServiceProfile("restaurant", "menu._tcp", 7288));
}


await app.RunAsync();
return;

void SetupMdnsRecord(ServiceDiscovery serviceDiscovery, ServiceProfile serviceProfile)
{
    if (serviceDiscovery.Probe(serviceProfile))
        throw new InvalidOperationException("Service is already running");
    //Begin responding to queries for this service
    serviceDiscovery.Advertise(serviceProfile);
    //Notify listeners that the service is now available
    serviceDiscovery.Announce(serviceProfile);
}