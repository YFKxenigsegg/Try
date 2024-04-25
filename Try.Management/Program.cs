using Microsoft.OpenApi.Models;
using System.Reflection;
using Try.Management.Grpc;
using Try.Management.Infrastructure;
using Try.Management.Infrastructure.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

builder.Services.Configure<MongoConnectionOptions>(builder.Configuration.GetSection("MongoOptions:Client"));
builder.Services.AddSingleton<IMongoClientFactory, MongoClientFactory>();

builder.Services.Configure<ManagementDbOptions>(builder.Configuration.GetSection("MongoOptions:Databases:0"));
builder.Services.Configure<PacketsCollectionOptions>(builder.Configuration.GetSection("MongoOptions:Databases:0:Collections:0"));
builder.Services.AddSingleton<IManagementDbFactory, ManagementDbFactory>();
builder.Services.AddSingleton<IPacketsCollectionFactory, PacketsCollectionFactory>();

builder.Services.AddSingleton<IMapper>(new Mapper(new MapperConfiguration(config => config.AddMaps(Assembly.GetExecutingAssembly()))));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapGrpcService<PacketsService>();

app.Run();
