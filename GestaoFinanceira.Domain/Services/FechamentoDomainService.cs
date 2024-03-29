﻿using GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework;
using GestaoFinanceira.Domain.Interfaces.Services;
using System;

namespace GestaoFinanceira.Domain.Services
{
    public  class FechamentoDomainService : IFechamentoDomainService
    {
        protected IUnitOfWork unitOfWork;

        public FechamentoDomainService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void Executar(int idUsuario, DateTime dataReferencia, string status)
        {
            try
            {
                this.unitOfWork.BeginTransaction();
                this.unitOfWork.IFechamentoRepository.Executar(idUsuario, dataReferencia, status);
                this.unitOfWork.Commit();
            }
            catch (Exception e)
            {
                this.unitOfWork.Rollback();
                throw new Exception(e.InnerException != null ? e.InnerException.Message : e.Message);
            }            

        }
    }
}
