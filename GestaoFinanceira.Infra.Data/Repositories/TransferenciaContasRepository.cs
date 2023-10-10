using Dapper;
using GestaoFinanceira.Domain.Interfaces.Repositories;
using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace GestaoFinanceira.Infra.Data.Repositories
{
    public class TransferenciaContasRepository: ITransferenciaContasRepository
    {
        protected IDbConnection connection;

        public TransferenciaContasRepository(IDbConnection connection)
        {
            this.connection = connection;

        }

        public IEnumerable<dynamic> Executar(TransferenciaContas transferenciaContas)
        {
            
            try
            {

                var sqlText = "PRC_TRANSFERENCIA_ENTRE_CONTAS";
                var sqlParams = new {@Id_Cont =  transferenciaContas.IdConta,
                                     @Id_Cont_Destino= transferenciaContas.IdContaDestino,
                                     @Data_More = transferenciaContas.DataMovimentacaoRealizada,
                                     @Valor_More= transferenciaContas.Valor};

                var result = connection.Query(sqlText, sqlParams, commandType: CommandType.StoredProcedure);               

                return result;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);

            }
            finally 
            { 
                connection.Close(); 
            }
            
        }

      
    }
}