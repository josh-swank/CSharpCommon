using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Swank.Unity.Editor
{
    public abstract class ChangeHandler
    {
        public static readonly ChangeHandler
            AssetHandler = new Asset(),
            DirHandler = new DirChangeHandler(),
            CSharpHandler = new CSharpChangeHandler();

        public static ImmutableDictionary<string, ChangeHandler> ExtHandler
            = new Dictionary<string, ChangeHandler>()
            {
                { ".meta", AssetHandler },
                { ".cs", CSharpHandler }
            }.ToImmutableDictionary();

        public static ChangeHandler Get(ref string path)
        {
            if (Directory.Exists(path))
                return DirHandler;

            var ext = Path.GetExtension(path);
            if (ext == ".meta")
                path = Path.ChangeExtension(path, null);
            if (ExtHandler.ContainsKey(ext))
                return ExtHandler[ext];

            Debug.LogWarning($"No change handler is registered for asset extension {ext}");
            return null;
        }

        private class Asset : ChangeHandler
        {
            public override void OnWillCreate(string path) => Get(ref path).OnWillCreate(path);
            public override void OnWillDelete(string path, RemoveAssetOptions options) => Get(ref path)?.OnWillDelete(path, options);
        }

        public virtual void OnWillCreate(string path) { }
        public virtual void OnWillDelete(string path, RemoveAssetOptions options) { }
    }
}
