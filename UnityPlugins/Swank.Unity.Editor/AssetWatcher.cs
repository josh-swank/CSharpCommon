using UnityEditor;

namespace Swank.Unity.Editor
{
    sealed class AssetWatcher : UnityEditor.AssetModificationProcessor
    {
        /*************************************************************************************************************
          *************************************************************************************************************
         *
         * TODO: Look further into the other AssetModificationProcessor events.
         * 
         * OnWillMoveAsset and OnWillSaveAssets are both fairly straightforward, but like OnWillDeleteAsset, the
         * event handlers can allow, cancel, or take ownership of the associated operations.
         * 
         * The other four events are concerned exclusively with the file system. FileModeChanged is triggered when
         * one or more files switch between binary and text serialization modes. CanOpenForEdit	is mostly used by
         * version control integration to prevent conflicts. IsOpenForEdit is similar, but instead of preventing
         * edits, it can cause editors to block or shut down. Finally, MakeEditable	is called by Unity to request
         * that a resource be made available for a planned operation.
         * 
          *************************************************************************************************************
         *************************************************************************************************************
         */
        public static void OnWillCreateAsset(string path)
        {
            ChangeHandler.AssetHandler.OnWillCreate(path);
        }

        public static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions options)
        {
            ChangeHandler.AssetHandler.OnWillDelete(path, options);
            return AssetDeleteResult.DidNotDelete;
        }
    }
}
