using System;
using System.Collections.Generic;
using System.IO;

namespace Swank.Unity.Editor
{
    public class CSharpChangeHandler : ChangeHandler
    {
        public const string SourcePath = @"Assets/Source";

        public override void OnWillCreate(string path)
        {
            var templateVariables = new Dictionary<string, string>()
            {
                { @"NAMESPACEBEGIN", string.Empty },
                { @"NAMESPACEEND", string.Empty }
            };

            var ns = Namespace.GetNamespace(path, SourcePath);
            if (!string.IsNullOrEmpty(ns))
            {
                templateVariables[@"NAMESPACEBEGIN"] = $"namespace {ns}{Environment.NewLine}{{";
                templateVariables[@"NAMESPACEEND"] = "}";
            }

            var text = File.ReadAllText(path);
            foreach (var tv in templateVariables)
                text = text.Replace($"#{tv.Key}#", tv.Value);
            File.WriteAllText(path, text);
        }
    }
}
