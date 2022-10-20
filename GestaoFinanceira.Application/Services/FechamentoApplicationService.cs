using GestaoFinanceira.Application.Commands.Fechamento;
using GestaoFinanceira.Application.Interfaces;
using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using GestaoFinanceira.Infra.CrossCutting.Security;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.Services
{
    public class FechamentoApplicationService : IFechamentoApplicationService
    {
        private readonly IMediator mediator;
        private readonly ISaldoDiarioCaching saldoDiarioCaching;

        public FechamentoApplicationService(IMediator mediator, ISaldoDiarioCaching saldoDiarioCaching)
        {
            this.mediator = mediator;
            this.saldoDiarioCaching = saldoDiarioCaching;
        }

        public async Task Executar(CreateFechamentoCommand fechamentoCreateCommand)
        {
            await mediator.Send(fechamentoCreateCommand);
        }

        public List<FechamentoMensalDTO> GetAll()
        {
            List<SaldoDiarioDTO> saldosDiariosDTO = saldoDiarioCaching.GetAll().OrderByDescending(x => x.DataSaldo).ToList();
            List<FechamentoMensalDTO> fechamentosmensais = new List<FechamentoMensalDTO>();

            foreach (var item in saldosDiariosDTO)
            {
                if (!fechamentosmensais.Exists(f => f.MesAno.Equals(item.DataSaldo.ToString("MM/yyyy")))){
                    fechamentosmensais.Add(new FechamentoMensalDTO
                    {
                        MesAno = item.DataSaldo.ToString("MM/yyyy"),
                        Status = item.Status,
                        DescricaoStatus = item.Status == "A" ? "Aberto" : "Fechado"
                    }); 
                }
            }
            return fechamentosmensais;

        }
    }
}
