using Azure.Storage.Blobs;
using AzureCrud.Models;
using AzureCrud.Repository;
using System.Runtime.Intrinsics.X86;

var builder = WebApplication.CreateBuilder(args);

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Add services to the container.

builder.Services.AddScoped<IRepository<MyDataEntity>,Repository<MyDataEntity>>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Use CORS
app.UseCors("AllowAll");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




app.MapGet("/getentityasync", async (string fileName, string id, IRepository<MyDataEntity> service) =>
{
    return Results.Ok(await service.GetEntityAsync(fileName, id));
})
    .WithName("GetData");

app.MapGet("/getAllData", async (IRepository<MyDataEntity> service) =>
{
    return Results.Ok(await service.GetAllEntityAsync());
});


app.MapGet("/download/{containerName}/{blobName}", async (HttpContext context, string containerName, string blobName) =>
{
    var blobServiceClient = context.RequestServices.GetService<BlobServiceClient>();
    var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
    var blobClient = containerClient.GetBlobClient(blobName);

    if (!await blobClient.ExistsAsync())
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync($"Blob '{blobName}' not found in container '{containerName}'.");
        return;
    }

    var response = await blobClient.DownloadAsync();
    context.Response.ContentType = response.Value.ContentType;
    await response.Value.Content.CopyToAsync(context.Response.Body);
});

app.MapPost("/Createentityasync", async (MyDataEntity entity, IRepository<MyDataEntity> service) =>
{
    entity.PartitionKey = entity.FileName;
    string Id = Guid.NewGuid().ToString();
    entity.Id = Id;
    entity.RowKey = Id;
    var createdEntity = await service.CreateEntityAsync(entity);
    return createdEntity;
})
.WithName("PostData");


app.MapPut("/updateEntityasync", async (MyDataEntity entity, IRepository<MyDataEntity> service) =>
{
    entity.PartitionKey = entity.FileName;
    string Id = Guid.NewGuid().ToString();
    entity.Id = Id; entity.RowKey = Id;
    var createdEntity = await service.UpdateEntityAsync(entity);
    return createdEntity;
}).WithName("PutData");

app.MapDelete("/deleteentityasync", async (string fileName, string id, IRepository<MyDataEntity> service) =>
{
    await service.DeleteEntityAsync(fileName, id);
    return Results.NoContent();
})
    .WithName("DeleteData");

app.UseHttpsRedirection();


app.UseCors("THISMyPolicy");


app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}