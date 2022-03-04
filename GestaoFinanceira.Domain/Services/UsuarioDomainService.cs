using GestaoFinanceira.Domain.Interfaces.Cryptography;
using GestaoFinanceira.Domain.Interfaces.Repositories;
using GestaoFinanceira.Domain.Interfaces.Services;
using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestaoFinanceira.Domain.Services
{
    public class UsuarioDomainService : GenericDomainService<Usuario>, IUsuarioDomainService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMD5Service mD5Service;

        public UsuarioDomainService(IUnitOfWork unitOfWork, IMD5Service mD5Service) : base(unitOfWork.IUsuarioRepository)
        {
            this.unitOfWork = unitOfWork;
            this.mD5Service = mD5Service;
        }

        public override int Add(Usuario obj)
        {
            try
            {
                obj.Senha = mD5Service.Encrypt(obj.Senha);
                base.Add(obj);
            }
            catch (Exception e)
            {

                throw new Exception(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
            return obj.Id;
        }


        public override void Update(Usuario obj)
        {
            try
            {
                obj.Senha = mD5Service.Encrypt(obj.Senha);
                base.Update(obj);
            }
            catch (Exception e)
            {

                throw new Exception(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        public void TrocaSenha(Usuario obj)
        {
            try
            {
                obj.Senha = mD5Service.Encrypt(obj.Senha);
                unitOfWork.BeginTransaction();
                unitOfWork.IUsuarioRepository.TrocaSenha(obj);
                unitOfWork.Commit();
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
            finally
            {
                unitOfWork.Dispose();
            }
        }

        public override void Delete(Usuario obj)
        {
            try
            {
                unitOfWork.BeginTransaction(); 
                unitOfWork.IMovimentacaoRealizadaRepository.Delete(obj.Id);
                unitOfWork.IMovimentacaoPrevistaRepository.Delete(obj.Id);
                unitOfWork.IMovimentacaoRepository.Delete(obj.Id);
                unitOfWork.IItemMovimentacaoRepository.Delete(obj.Id);
                unitOfWork.ICategoriaRepository.Delete(obj.Id);
                unitOfWork.IContaRepository.Delete(obj.Id);
                unitOfWork.IFormaPagamentoRepository.Delete(obj.Id);
                unitOfWork.IUsuarioRepository.Delete(obj);
                unitOfWork.Commit();
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
            finally
            {
                unitOfWork.Dispose();
            }


        }

        public Usuario Get(string email)
        {
            return unitOfWork.IUsuarioRepository.Get(email);
        }

        public Usuario Get(string email, string senha)
        {
            return unitOfWork.IUsuarioRepository.Get(email, mD5Service.Encrypt(senha));
        }

        public override List<Usuario> GetAll(int idUsuario)
        {
            return unitOfWork.IUsuarioRepository.GetAll(idUsuario).ToList();
        }
    }
}
