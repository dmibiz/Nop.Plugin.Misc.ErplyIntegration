using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Nop.Plugin.Misc.ErplyIntegration.Services
{
    public interface IErplyImportManager
    {
        public Task ImportCategories();
        public Task ImportProducts();
    }
}
