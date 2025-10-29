using System.Collections.Generic;

namespace Utilities
{
    public static class Constants
    {
        #region UI Tags

        public static string LevelUIPanelTag = "LevelUIPanel";
        public static string ItemSelectionPanelTag = "ItemSelectionPanel";
        
        #endregion

        #region Tower Tags

        public static string ArcherTowerTag = "Archer Tower";
        public static string WizardTowerTag = "Wizard Tower";
        public static string DivineTowerTag = "Divine Tower";

        public static List<string> GetTowerNameList()
        {
            return new List<string>
            {
                ArcherTowerTag,
                WizardTowerTag,
                DivineTowerTag
            };
        }
        
        #endregion

        #region Entity Tags

        public static string EnemyTag = "Enemy";

        #endregion
        
        #region Animation Tags
        public static readonly string TowerPlacementAnimationTag = "Placement";
        #endregion
    }
}