using System;
using System.Data;

namespace _modules._multi_language_support._scripts._excel
{
    public abstract class AbstractParser
    {
        protected DataTable DataTable;

        protected int ParseInteger(int row, int column)
        {
            return Convert.ToInt32(DataTable.Rows[row][column].ToString());
        }
    }
}