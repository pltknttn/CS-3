using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace WpfCalcMatrixAB
{
    public static class ArrayExtension
    {
        public static List<object> AsTupleList<T>(this T[,] matrix)
        {
            var col = matrix.GetLength(1);
            var result = new List<object>();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                T[] values = new T[col];

                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    values[j] = matrix[i, j];
                }

                result.Add(GetTuple(values));
            }

            return result;
        }

        private static object GetTuple<T>(params T[] values)
        { 
            Type genericType = Type.GetType("System.Tuple`" + values.Length);
            Type[] typeArgs = values.Select(_ => typeof(T)).ToArray();
            Type specificType = genericType.MakeGenericType(typeArgs);
            object[] constructorArguments = values.Cast<object>().ToArray();
            return Activator.CreateInstance(specificType, constructorArguments);
        }

        public static DataTable ToDataTable<T>(this T[,] matrix)
        {
            var res = new DataTable();

            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                res.Columns.Add((i+1).ToString(), typeof(T));
            }

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                var row = res.NewRow();

                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    row[j] = matrix[i, j];
                }

                res.Rows.Add(row);
            }

            return res;
        }
    }
}
