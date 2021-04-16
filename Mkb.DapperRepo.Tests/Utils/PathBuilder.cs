using System;
using System.IO;

namespace Mkb.DapperRepo.Tests.Utils
{
    public class PathBuilder
    {
        public static string BuildSqlScriptLocation(string scriptName) => Path.Join(Path.Join(Environment.CurrentDirectory, "SqlScripts"), scriptName);
    }
}