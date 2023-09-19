using GestaoFinanceira.Application.Commands.Categoria;
using GestaoFinanceira.Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.Interfaces
{
    public interface ICategoriaApplicationService 
    {
        Task Add(CreateCategoriaCommand command);
        Task Update(UpdateCategoriaCommand command);
        Task Delete(DeleteCategoriaCommand command);
        CategoriaDTO GetId(int id);
        List<CategoriaDTO> GetAll();
        byte[] GetAllReportExcel();
    }
}
