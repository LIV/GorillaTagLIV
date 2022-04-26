using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Mono.Cecil;

namespace GorilaTagLIVInstaller
{
    public static class Preload
    {
        public static IEnumerable<string> TargetDLLs { get; } = new[] {"Assembly-CSharp.dll"};

        public static void Patch(AssemblyDefinition assembly)
        {
            var executablePath = Assembly.GetExecutingAssembly().Location;
            var patchersPath = Path.GetDirectoryName(executablePath);
            var pluginPath = Path.Combine(patchersPath, "../../../Gorilla Tag_Data/Plugins/x86_64/LIV_Bridge.dll");

            if (File.Exists(pluginPath))
            {
                return;
            }
            
            File.Copy(Path.Combine(patchersPath, "LIV_Bridge.dll"), pluginPath);
        }
    }
}