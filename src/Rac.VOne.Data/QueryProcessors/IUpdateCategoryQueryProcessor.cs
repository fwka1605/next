using Rac.VOne.Web.Models;
using System.Collections.Generic;


namespace Rac.VOne.Data.QueryProcessors
{
    public interface IUpdateCategoryQueryProcessor
    {
        Category UpdateCategory(Category Category);
    }
}
