using Orderly.Application.Interfaces;
using Orderly.Application.UseCases.Pedidos;
using Orderly.Application.UseCases.Produtos;
using Orderly.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IProdutoRepository, ProdutoRepositoryMemoria>();
builder.Services.AddSingleton<IPedidoRepository, PedidoRepositoryMemoria>();

builder.Services.AddScoped<CadastrarProdutoUseCase>();
builder.Services.AddScoped<CriarPedidoUseCase>();
builder.Services.AddScoped<PagarPedidoUseCase>();
builder.Services.AddScoped<CancelarPedidoUseCase>();
builder.Services.AddScoped<ConsultarPedidoUseCase>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/openapi/v1.json",  "Orderly.Api v1"));
}

app.MapControllers();

app.Run();