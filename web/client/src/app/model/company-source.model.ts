import { Company } from 'src/app/model/company.model';
import { CompanyLogo } from 'src/app/model/company-logo.model';
import { ApplicationControl } from 'src/app/model/application-control.model';
import { MenuAuthority } from 'src/app/model/menu-authority.model';
import { FunctionAuthority } from 'src/app/model/function-authority.model';
import { PasswordPolicy } from 'src/app/model/password-policy.model';
import { LoginUserLicense } from 'src/app/model/login-user-license.model';

export class CompanySource {
    public company:Company;
    public saveCompanyLogos:CompanyLogo[];
    public deleteCompanyLogos:CompanyLogo[];
    public applicationControl:ApplicationControl;
    public menuAuthorities:MenuAuthority[];
    public functionAuthorities:FunctionAuthority[];
    public passwordPolicy:PasswordPolicy;
    public loginUserLicense:LoginUserLicense;
}

