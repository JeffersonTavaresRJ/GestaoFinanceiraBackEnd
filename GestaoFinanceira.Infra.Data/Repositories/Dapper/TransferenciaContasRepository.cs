using GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Domain.Models.Enuns;
using System;
using System.Collections.Generic;
using System.Data;

namespace GestaoFinanceira.Infra.Data.Repositories.Dapper
{
    public class TransferenciaContasRepository : GenericRepository<>, ITransferenciaContasRepository
    {

        public TransferenciaContasRepository(IDbConnection connection) : base(connection)
        {
        }

        public IEnumerable<dynamic> Execute(TransferenciaContas transferenciaContas)
        {

            try
            {

                var sqlText = "PRC_TRANSFERENCIA_ENTRE_CONTAS";

                object sqlParams = new
                {
                    @Id_Cont = transferenciaContas.IdConta,
                    @Id_Cont_Destino = transferenciaContas.IdContaDestino,
                    @Data_More = transferenciaContas.DataMovimentacaoRealizada,
                    @Valor_More = transferenciaContas.Valor
                };

                return Execute(sqlText, sqlParams, TipoExecucao.StoredProcedure);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);

            }

        }


    }
}