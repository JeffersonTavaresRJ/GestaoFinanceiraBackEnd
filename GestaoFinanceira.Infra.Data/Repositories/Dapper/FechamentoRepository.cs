using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework;
using GestaoFinanceira.Domain.Models.Enuns;
using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
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

                Execute(sqlText, sqlParams, TipoExecucao.StoredProcedure);

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

                var sqlText = "SELECT * FROM VW_SADI_FECH";

                object sqlParams = null;

                return Execute(sqlText, sqlParams, TipoExecucao.CommandTex).ToList();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);

            }
        }
    }
}
