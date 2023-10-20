using GestaoFinanceira.Infra.CrossCutting.ValidationAdapters.Models;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace GestaoFinanceira.Infra.CrossCutting.ValidationAdapters
{
    public static class ValidationAdapter
    {
        /// <summary>
        /// Método para conversão de ValidationFailure em ValidationErrorModel
        /// </summary>
        /// <param name="errors">Lista de erros do FluentValidation</param>
        /// <returns>Lista de erros da classe modelo do projeto API</returns>
        public static List<ValidationErrorModel> Parse(IEnumerable<ValidationFailure> errors)
        {
            List<ValidationErrorModel> result = errors.Select
                (er => new { er.PropertyName, Errors = er.ErrorMessage })
                .GroupBy(g => g.PropertyName).ToList()
                .Select(s => new ValidationErrorModel
                {
                    PropertyName = s.Key,
                    Errors = s.Select(m => m.Errors).ToList()
                })
                .ToList();
            return result;

        }
    }
}
