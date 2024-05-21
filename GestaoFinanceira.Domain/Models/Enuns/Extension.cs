using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GestaoFinanceira.Domain.Models.Enuns
{
    public static class ExtensionEnum
    {
        private static T ObterAtributoDoTipo<T>(this Enum valorEnum) where T : System.Attribute
        {
            var type = valorEnum.GetType();
            var menInfo = type.GetMember(valorEnum.ToString());
            var atributtes = menInfo[0].GetCustomAttributes(typeof(T), false);
            return (atributtes.Length > 0) ? (T)atributtes[0] : null;
        }

        public static string ObterDescricao(this Enum valorEnum)
        {
            return valorEnum.ObterAtributoDoTipo<DescriptionAttribute>().Description;
        }

        public static List<GenericEnum> Listar(Type tipo)
        {
            List<GenericEnum> lista = new List<GenericEnum>();

            if (tipo != null)
            {
                Array enumValores = Enum.GetValues(tipo);
                foreach (Enum valor in enumValores)
                {
                    //valor=Id
                    //ObterDescricao=Descrição
                    var e = new GenericEnum(valor.ToString(), ObterDescricao(valor));
                    lista.Add(e);

                    //lista.Add(new KeyValuePair<string, string>(valor.ToString(), ObterDescricao(valor)));
                }
            }

            return lista;
        }
    }
}