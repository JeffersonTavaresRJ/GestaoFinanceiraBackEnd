using GestaoFinanceira.Domain.Interfaces.Repositories;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestaoFinanceira.Infra.Data.Repositories
{
    public class MovimentacaoPrevistaRepository: GenericRepository<MovimentacaoPrevista>,IMovimentacaoPrevistaRepository
    {
        public MovimentacaoPrevistaRepository(SqlContext sqlContext):base(sqlContext)
        {

        }
        
        public IEnumerable<MovimentacaoPrevista> GetByDataReferencia(int idUsuario,
                                                                     int? idItemMovimentacao, 
                                                                     DateTime dataReferenciaInicial, 
                                                                     DateTime dataReferenciaFinal)
        {
            return this.dbset.Where(mp => mp.FormaPagamento.IdUsuario == idUsuario &&
                                          (mp.IdItemMovimentacao == idItemMovimentacao || 
                                          idItemMovimentacao == null) &&
                                          mp.DataReferencia >= dataReferenciaInicial &&
                                          mp.DataReferencia <= dataReferenciaFinal).ToList();
        }

        public override void Delete(int idUsuario)
        {
            this.dbset.RemoveRange(dbset.Where(mp=>mp.FormaPagamento.IdUsuario == idUsuario));
        }

        public override IEnumerable<MovimentacaoPrevista> GetAll(int idUsuario)
        {
            return dbset.Where(mp=>mp.FormaPagamento.IdUsuario==idUsuario);
        }
    }
}
