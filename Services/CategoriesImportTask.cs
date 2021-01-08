using Nop.Services.Tasks;

namespace Nop.Plugin.Misc.ErplyIntegration.Services
{
    /// <summary>
    /// Represents a schedule task to import Erply categories
    /// </summary>
    public class CategoriesImportTask : IScheduleTask
    {
        #region Fields

        private readonly ErplyImportManager _erplyImportManager;

        #endregion

        #region Ctor

        public CategoriesImportTask(ErplyImportManager erplyImportManager)
        {
            _erplyImportManager = erplyImportManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Execute task
        /// </summary>
        public async void Execute()
        {
            await _erplyImportManager.ImportCategories();
        }

        #endregion
    }
}
