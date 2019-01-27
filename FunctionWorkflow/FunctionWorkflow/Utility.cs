using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionWorkflow
{
    public static class Utility
    {
        public static string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
    }
}
