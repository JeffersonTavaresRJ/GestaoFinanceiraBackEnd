using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Repositories.Dapper;
using GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework;
using GestaoFinanceira.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestaoFinanceira.Domain.Services
{
    public  class FechamentoDomainService : IFechamentoDomainService
    {
        protected IUnitOfWork unitOfWork;
        protected IFechamentoRepository fechamentoRepository;

        public FechamentoDomainService(IUnitOfWork unitOfWork, IFechamentoRepository fechamentoRepository)
        {
            this.unitOfWork = unitOfWork;
            this.fechamentoRepository = fechamentoRepository;
        }

        public void Executar(int idUsuario, DateTime dataReferencia, string status)
        {
            try
            {
                //this.unitOfWork.BeginTransaction();
                //this.unitOfWork.IFechamentoRepository.Executar(idUsuario, dataReferencia, status);
                //this.unitOfWork.Commit();
                this.fechamentoRepository.Executar(idUsuario, dataReferencia, status);
            }
            catch (Exception e)
            {
                //this.unitOfWork.Rollback();
                throw new Exception(e.InnerException != null ? e.InnerException.Message : e.Message);
            }            

        }

        public List<FechamentoMensalDTO> GetAll()
        {
            return this.fechamentoRepository.GetAll().OrderByDescending(f=>f.DataReferencia).ToList();
        }
    }
}
