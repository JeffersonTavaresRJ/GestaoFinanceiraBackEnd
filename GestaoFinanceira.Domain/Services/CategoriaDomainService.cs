using GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework;
using GestaoFinanceira.Domain.Interfaces.Services;
using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestaoFinanceira.Domain.Services
{
    public class CategoriaDomainService : GenericDomainService<Categoria>, ICategoriaDomainService
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoriaDomainService( IUnitOfWork unitOfWork):base(unitOfWork.ICategoriaRepository)
        {
            this.unitOfWork = unitOfWork;
        }

        public override List<Categoria> GetAll(int idUsuario)
        {
            return unitOfWork.ICategoriaRepository.GetAll(idUsuario).ToList();
        }        
    }
}
