using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Repositories.Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GestaoFinanceira.Infra.Data.Repositories.Dapper
{
    public class FechamentoRepository : GenericRepository<FechamentoMensalDTO>, IFechamentoRepository
    {
        public FechamentoRepository(IDbConnection connection) : base(connection)
        {
        }

        public void Executar(int idUsuario, DateTime dataReferencia, string status)
        {
            try
            {

                var sqlText = "PRC_GRAVA_FECHAMENTO";

                object sqlParams = new
                {
                    @Id_Usua = idUsuario,
                    @Data_Referencia = dataReferencia,
                    @Status = status
                };

                ExecuteStoredProcedure(sqlText, sqlParams);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);

            }
        }

        public List<FechamentoMensalDTO> GetAll()
        {
            try
            {
                var sqlText =  "SELECT " +
                               "MES_ANO as MesAno," +
                               "DATA_REFERENCIA as DataReferencia," +
                               "STATUS_SADI as Status," +
                               "DESCRICAO_STATUS as DescricaoStatus " +
                               "FROM VW_SADI_FECH";

                return ExecuteQuery(sqlText, null).ToList();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);

            }
        }
    }
}
