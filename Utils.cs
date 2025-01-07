using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSCodeFunction
{
    internal class Utils
    {
        public static string? GetEnvironmentVariable(string name)
        {
            string? env = System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);

            return env;
        }
    }
}
