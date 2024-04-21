using GestaoFinanceira.Domain.Interfaces.Repositories.Dapper;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Domain.Models.Enuns;
using System;
using System.Collections.Generic;
using System.Data;

namespace GestaoFinanceira.Infra.Data.Repositories.Dapper
{
    public class TransferenciaContasRepository : GenericRepository<TransferenciaContas>, ITransferenciaContasRepository
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

                return ExecuteStoredProcedure(sqlText, sqlParams);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);

            }

        }


    }
}