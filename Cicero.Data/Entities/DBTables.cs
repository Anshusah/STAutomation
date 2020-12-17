using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Data.Entities
{
   public class DBTables
    {
        public string Table_Name { get; set; }
    }

  public class DBTableColumn
    {
        public string Column_Name { get; set; }
    }
    public class DBTablesProperties
    {
        public string DATA_TYPE { get; set; }
        public string IS_NULLABLE { get; set; }
        public string column_name { get; set; }
    }
}
