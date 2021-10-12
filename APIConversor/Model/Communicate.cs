using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICommunicator.Model
{
    public class COMMUNICATE
    {
        public string sqlType { get; set; }
        public string sqlHost { get; set; }
        public int sqlPort { get; set; }
        public string sqlUser { get; set; }
        public string sqlPass { get; set; }
        public string sqlDatabase { get; set; }
        public string sqlFile { get; set; }
        public int sqlKeepAlive { get; set; }
        public int mysqlReadTimeout { get; set; }
        public int mysqlWriteTimeout { get; set; }
        public string encryptionType { get; set; }
    }
}