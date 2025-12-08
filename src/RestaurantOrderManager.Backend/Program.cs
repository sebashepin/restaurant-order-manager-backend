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

await app.RunAsync();