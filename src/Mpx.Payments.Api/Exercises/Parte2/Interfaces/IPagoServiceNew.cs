using Mpx.Payments.Api.Domain.DTOs;

namespace Mpx.Payments.Api.Exercises.Parte2.Interfaces
{
    public interface IPagoServiceNew
    {
        Task<PagoResponse> RegistrarPagoAsync(AplicarPagoCommand command, CancellationToken ct = default);


        Task<PagoResponse> ReversarPagoAsync(Guid id, ReversarPagoRequest request);
        Task<PagoResponse?> ObtenerPagoPorIdAsync(Guid id);
    }
}
