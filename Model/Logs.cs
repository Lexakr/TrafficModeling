using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficModeling.Model
{
    internal class Logs
    {
        private static Logs logs;
        public List<string> _Logs { get; set; }
        private Logs() { }

        public static Logs GetLogs()
        {
            if (logs == null)
            {
                logs = new Logs();
            }
            return logs;
        }

        public void Debug(string data)
        {
            _Logs.Add(data); // в свойство List<string> с названием Logs
        }
    }
}
