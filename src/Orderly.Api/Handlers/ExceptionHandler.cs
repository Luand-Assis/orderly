using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Orderly.Application.Exceptions;
using Orderly.Domain.Exceptions;

namespace Orderly.Api.Handlers
{



    public class ExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is OperationCanceledException)
            {
                httpContext.Response.StatusCode = 499;
                return true;
            }

            var (status, titulo) = exception switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, "Recurso nao encontrado"),

                ExceptionProdutoDomain or ExceptionPedidoDomain
                    or ExceptionProdutoApplication or ExceptionPedidoApplication
                    => (StatusCodes.Status400BadRequest, "Regra de negocio violada"),

                _ => (StatusCodes.Status500InternalServerError, "Erro interno")
            };

            httpContext.Response.StatusCode = status;

            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = status,
                Title = titulo,
                Detail = status == StatusCodes.Status500InternalServerError ? null : exception.Message
            }, cancellationToken);

            return true;
        }
    }
}