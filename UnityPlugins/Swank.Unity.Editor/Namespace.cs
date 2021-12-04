using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Swank.Unity.Editor
{
    public static class Namespace
    {
        public static string Trim(string ns)
        {
            return ns?.Trim() ?? throw new ArgumentNullException("ns");
        }

        public static string Combine(string ns1, string ns2)
        {
            ns1 = Trim(ns1);
            ns2 = Trim(ns2);
            if (!string.IsNullOrEmpty(ns1) && !string.IsNullOrEmpty(ns2))
                return $"{ns1}.{ns2}";
            else
                return $"{ns1}{ns2}";
        }

        public static string Combine(params string[] nss)
        {
            string fullNs = string.Empty;

            int i = 0;
            while ((fullNs += Trim(nss[i++])) != string.Empty && i < nss.Length);

            while (i < nss.Length)
            {
                var ns = Trim(nss[i++]);
                if (ns != string.Empty)
                        fullNs += $".{ns}";
            }

            return fullNs;
        }

        public static IList<string> GetRootNamespaceList(string root, IList<string> ns)
        {
            if (!string.IsNullOrEmpty(root?.Trim()))
                ns.Add(root);
            return ns;
        }

        public static IList<string> GetNamespaceList(string path, string sourcePath, string rootPath, IList<string> ns)
        {
            // Plugin assembly source path
            var asmdefs = Directory.GetFiles(path, "*.asmdef");
            if (asmdefs.Length > 0)
            {
                var o = JObject.Parse(File.ReadAllText(asmdefs[0]));
                return GetRootNamespaceList((string)o.SelectToken("rootNamespace"), ns);
            }

            // Project source path
            if (path == sourcePath)
                return GetRootNamespaceList(EditorSettings.projectGenerationRootNamespace, ns);

            // Project root path
            if (path == rootPath)
                throw new InvalidOperationException("The script file is not in a valid source directory.");

            GetNamespaceList(Path.GetDirectoryName(path), sourcePath, rootPath, ns).Add(Path.GetFileName(path));
            return ns;
        }

        public static string GetNamespace(string path, string sourcePath)
        {
            if (!Directory.Exists(path))
                path = Path.GetDirectoryName(path);
            return string.Join(".",
                GetNamespaceList(
                    Path.GetFullPath(path),
                    Path.GetFullPath(sourcePath),
                    Path.GetFullPath(Application.dataPath),
                    new List<string>()));
        }
    }
}
