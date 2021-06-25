using GestaoFinanceira.Domain.Interfaces.Repositories;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestaoFinanceira.Infra.Data.Repositories
{
    public class MovimentacaoRealizadaRepository : GenericRepository<MovimentacaoRealizada>, IMovimentacaoRealizadaRepository
    {
        public MovimentacaoRealizadaRepository(SqlContext sqlContext):base(sqlContext)
        {

        }
        
        public override IEnumerable<MovimentacaoRealizada> GetAll(int idUsuario)
        {
            return dbset.Where(mr=>mr.Conta.IdUsuario == idUsuario);
        }

        public override void Delete(int idUsuario)
        {
            dbset.RemoveRange(dbset.Where(mr => mr.Conta.IdUsuario == idUsuario));
        }

        public IEnumerable<MovimentacaoRealizada> GetByDataReferencia(int? idItemMovimentacao, int idUsuario, DateTime dataRefIni, DateTime dataRefFim)
        {
            return dbset.Where(mr =>  mr.DataReferencia >= dataRefIni &&
                                      mr.DataReferencia <= dataRefFim &&
                                      mr.Conta.IdUsuario == idUsuario &&
                                     (mr.IdItemMovimentacao == idItemMovimentacao || idItemMovimentacao == null));
        }
    }
}
