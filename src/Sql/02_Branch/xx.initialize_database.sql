﻿USE VOneG4
:r $(cdir)\Data\Initialize_[dbo].[BankAccountType].sql
:r $(cdir)\Data\Initialize_[dbo].[CategoryBase].sql
:r $(cdir)\Data\Initialize_[dbo].[ExportFieldSettingBase].sql
:r $(cdir)\Data\Initialize_[dbo].[GeneralSettingBase].sql
:r $(cdir)\Data\Initialize_[dbo].[GridSettingBase].sql
:r $(cdir)\Data\Initialize_[dbo].[ImporterFormat]and[ImporterSettingBase].sql
:r $(cdir)\Data\Initialize_[dbo].[JuridicalPersonalityBase].sql
:r $(cdir)\Data\Initialize_[dbo].[MasterImportSettingBase].sql
:r $(cdir)\Data\Initialize_[dbo].[Menu].sql
:r $(cdir)\Data\Initialize_[dbo].[PaymentFileFormat].sql
:r $(cdir)\Data\Initialize_[dbo].[ReportSettingBase].sql
:r $(cdir)\Data\Initialize_[dbo].[SettingDefinition]and[Setting].sql
:r $(cdir)\Data\Initialize_[dbo].[TaxClass].sql

:r $(cdir)\Debug\01.initialize_[dbo].[Company].sql
:r $(cdir)\Debug\02.initialize_[dbo].[ApplicationControl].sql
:r $(cdir)\Debug\03.initialize_[dbo].[Category].sql
:r $(cdir)\Debug\04.initialize_[dbo].[Department].sql
:r $(cdir)\Debug\05.initialize_[dbo].[Staff].sql
:r $(cdir)\Debug\06.initialize_[dbo].[LoginUser].sql
:r $(cdir)\Debug\07.initialize_[dbo].[MenuAuthority].sql
:r $(cdir)\Debug\08.initialize_[dbo].[FunctionAuthority].sql
:r $(cdir)\Debug\09.initialize_[dbo].[Customer].sql
:r $(cdir)\Debug\10.Initialize_[dbo].[Currency].sql
:r $(cdir)\Debug\11.initialize_[dbo].[Billing].sql
:r $(cdir)\Debug\12.initialize_[dbo].[Receipt].sql
:r $(cdir)\Debug\13.Initialize_[dbo].[MasterImportSetting].sql
:r $(cdir)\Debug\14.Initialize_[dbo].[ColumnNameSetting].sql
:r $(cdir)\Debug\15.Initialize_[dbo].[PaymentAgency].sql
:r $(cdir)\Debug\17.Initialize_[dbo].[CollationOrder].sql
:r $(cdir)\Debug\18.Initialize_[dbo].[GeneralSetting].sql
:r $(cdir)\Debug\19.Initialize_[dbo].[CollationSetting].sql
:r $(cdir)\Debug\20.Initialize_[dbo].[MatchingOrder].sql
:r $(cdir)\Debug\21.Initialize_[dbo].[JuridicalPersonality].sql
:r $(cdir)\Debug\22.Initialize_[dbo].[PasswordPolicy].sql
:r $(cdir)\Debug\23.Initialize_[dbo].[LoginUserPassword].sql

:r $(cdir)\SP\[dbo].[uspCollationCustomerId].sql
:r $(cdir)\SP\[dbo].[uspCollationHistory].sql
:r $(cdir)\SP\[dbo].[uspCollationInitialize].sql
:r $(cdir)\SP\[dbo].[uspCollationMissing].sql
:r $(cdir)\SP\[dbo].[uspCollationPayerCode].sql
:r $(cdir)\SP\[dbo].[uspCollationPayerName].sql
:r $(cdir)\SP\[dbo].[uspCollation].sql

:r $(cdir)\FN\[dbo].[GetClientKey].sql

:r $(cdir)\TYPE\create_user_type_table_valued_parameters.sql
