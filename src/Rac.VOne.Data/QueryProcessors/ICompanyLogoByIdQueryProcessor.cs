using Rac.VOne.Web.Models;
using System.Collections.Generic;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface ICompanyLogoByIdQueryProcessor
    {
        CompanyLogo GetCompanyLogo(int CompanyId);
    }
}
