using Microsoft.EntityFrameworkCore;

namespace ProjectManagement.Api.Data;

public static class DatabaseInitializer
{
    public static void EnsureDatabaseObjects(AppDbContext db)
    {
        db.Database.ExecuteSqlRaw(
            """
            IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = N'Master')
            BEGIN
                EXEC(N'CREATE SCHEMA [Master]');
            END;

            IF OBJECT_ID(N'[Master].[Company]', N'U') IS NULL
            BEGIN
                CREATE TABLE [Master].[Company]
                (
                    [Id] uniqueidentifier NOT NULL,
                    [Name] nvarchar(150) NOT NULL,
                    [Code] nvarchar(50) NOT NULL,
                    [ContactEmail] nvarchar(150) NOT NULL CONSTRAINT [DF_Master_Company_ContactEmail] DEFAULT (N''),
                    [ContactPhone] nvarchar(25) NULL,
                    [Website] nvarchar(150) NULL,
                    [AddressLine1] nvarchar(200) NULL,
                    [AddressLine2] nvarchar(200) NULL,
                    [City] nvarchar(100) NULL,
                    [State] nvarchar(100) NULL,
                    [Country] nvarchar(100) NULL,
                    [PostalCode] nvarchar(20) NULL,
                    [IsLoginAllowed] bit NOT NULL CONSTRAINT [DF_Master_Company_IsLoginAllowed] DEFAULT ((1)),
                    [CreatedAtUtc] datetime2 NOT NULL CONSTRAINT [DF_Master_Company_CreatedAtUtc] DEFAULT (SYSUTCDATETIME()),
                    [UpdatedAtUtc] datetime2 NOT NULL CONSTRAINT [DF_Master_Company_UpdatedAtUtc] DEFAULT (SYSUTCDATETIME()),
                    CONSTRAINT [PK_Master_Company] PRIMARY KEY ([Id])
                );
            END;

            IF COL_LENGTH('Master.Company', 'ContactEmail') IS NULL
            BEGIN
                ALTER TABLE [Master].[Company] ADD [ContactEmail] nvarchar(150) NOT NULL CONSTRAINT [DF_Master_Company_ContactEmail_Alter] DEFAULT (N'') WITH VALUES;
            END;

            IF COL_LENGTH('Master.Company', 'ContactPhone') IS NULL
            BEGIN
                ALTER TABLE [Master].[Company] ADD [ContactPhone] nvarchar(25) NULL;
            END;

            IF COL_LENGTH('Master.Company', 'Website') IS NULL
            BEGIN
                ALTER TABLE [Master].[Company] ADD [Website] nvarchar(150) NULL;
            END;

            IF COL_LENGTH('Master.Company', 'AddressLine1') IS NULL
            BEGIN
                ALTER TABLE [Master].[Company] ADD [AddressLine1] nvarchar(200) NULL;
            END;

            IF COL_LENGTH('Master.Company', 'AddressLine2') IS NULL
            BEGIN
                ALTER TABLE [Master].[Company] ADD [AddressLine2] nvarchar(200) NULL;
            END;

            IF COL_LENGTH('Master.Company', 'City') IS NULL
            BEGIN
                ALTER TABLE [Master].[Company] ADD [City] nvarchar(100) NULL;
            END;

            IF COL_LENGTH('Master.Company', 'State') IS NULL
            BEGIN
                ALTER TABLE [Master].[Company] ADD [State] nvarchar(100) NULL;
            END;

            IF COL_LENGTH('Master.Company', 'Country') IS NULL
            BEGIN
                ALTER TABLE [Master].[Company] ADD [Country] nvarchar(100) NULL;
            END;

            IF COL_LENGTH('Master.Company', 'PostalCode') IS NULL
            BEGIN
                ALTER TABLE [Master].[Company] ADD [PostalCode] nvarchar(20) NULL;
            END;

            IF COL_LENGTH('Master.Company', 'IsLoginAllowed') IS NULL
            BEGIN
                ALTER TABLE [Master].[Company] ADD [IsLoginAllowed] bit NOT NULL CONSTRAINT [DF_Master_Company_IsLoginAllowed_Alter] DEFAULT ((1)) WITH VALUES;
            END;

            IF COL_LENGTH('Master.Company', 'UpdatedAtUtc') IS NULL
            BEGIN
                ALTER TABLE [Master].[Company] ADD [UpdatedAtUtc] datetime2 NOT NULL CONSTRAINT [DF_Master_Company_UpdatedAtUtc_Alter] DEFAULT (SYSUTCDATETIME()) WITH VALUES;
            END;

            IF NOT EXISTS
            (
                SELECT 1
                FROM sys.indexes
                WHERE name = N'IX_Master_Company_Code'
                    AND object_id = OBJECT_ID(N'[Master].[Company]')
            )
            BEGIN
                CREATE UNIQUE INDEX [IX_Master_Company_Code] ON [Master].[Company] ([Code]);
            END;

            IF OBJECT_ID(N'[Master].[Role]', N'U') IS NULL
            BEGIN
                CREATE TABLE [Master].[Role]
                (
                    [Id] uniqueidentifier NOT NULL,
                    [Name] nvarchar(100) NOT NULL,
                    [Description] nvarchar(250) NULL,
                    [CanManagePortal] bit NOT NULL CONSTRAINT [DF_Master_Role_CanManagePortal] DEFAULT ((0)),
                    [CanManageEmployees] bit NOT NULL CONSTRAINT [DF_Master_Role_CanManageEmployees] DEFAULT ((0)),
                    [CanManageCustomers] bit NOT NULL CONSTRAINT [DF_Master_Role_CanManageCustomers] DEFAULT ((0)),
                    [LimitPortalManagementToOwnCustomer] bit NOT NULL CONSTRAINT [DF_Master_Role_LimitPortalManagementToOwnCustomer] DEFAULT ((0)),
                    [IsActive] bit NOT NULL CONSTRAINT [DF_Master_Role_IsActive] DEFAULT ((1)),
                    [DisplayOrder] int NOT NULL CONSTRAINT [DF_Master_Role_DisplayOrder] DEFAULT ((0)),
                    [CreatedAtUtc] datetime2 NOT NULL CONSTRAINT [DF_Master_Role_CreatedAtUtc] DEFAULT (SYSUTCDATETIME()),
                    CONSTRAINT [PK_Master_Role] PRIMARY KEY ([Id])
                );
            END;

            IF COL_LENGTH('Master.Role', 'Description') IS NULL
            BEGIN
                ALTER TABLE [Master].[Role] ADD [Description] nvarchar(250) NULL;
            END;

            IF COL_LENGTH('Master.Role', 'CanManagePortal') IS NULL
            BEGIN
                ALTER TABLE [Master].[Role] ADD [CanManagePortal] bit NOT NULL CONSTRAINT [DF_Master_Role_CanManagePortal_Alter] DEFAULT ((0)) WITH VALUES;
            END;

            IF COL_LENGTH('Master.Role', 'CanManageEmployees') IS NULL
            BEGIN
                ALTER TABLE [Master].[Role] ADD [CanManageEmployees] bit NOT NULL CONSTRAINT [DF_Master_Role_CanManageEmployees_Alter] DEFAULT ((0)) WITH VALUES;
            END;

            IF COL_LENGTH('Master.Role', 'CanManageCustomers') IS NULL
            BEGIN
                ALTER TABLE [Master].[Role] ADD [CanManageCustomers] bit NOT NULL CONSTRAINT [DF_Master_Role_CanManageCustomers_Alter] DEFAULT ((0)) WITH VALUES;
            END;

            IF COL_LENGTH('Master.Role', 'LimitPortalManagementToOwnCustomer') IS NULL
            BEGIN
                ALTER TABLE [Master].[Role] ADD [LimitPortalManagementToOwnCustomer] bit NOT NULL CONSTRAINT [DF_Master_Role_LimitPortalManagementToOwnCustomer_Alter] DEFAULT ((0)) WITH VALUES;
            END;

            IF COL_LENGTH('Master.Role', 'IsActive') IS NULL
            BEGIN
                ALTER TABLE [Master].[Role] ADD [IsActive] bit NOT NULL CONSTRAINT [DF_Master_Role_IsActive_Alter] DEFAULT ((1)) WITH VALUES;
            END;

            IF COL_LENGTH('Master.Role', 'DisplayOrder') IS NULL
            BEGIN
                ALTER TABLE [Master].[Role] ADD [DisplayOrder] int NOT NULL CONSTRAINT [DF_Master_Role_DisplayOrder_Alter] DEFAULT ((0)) WITH VALUES;
            END;

            IF COL_LENGTH('Master.Role', 'CreatedAtUtc') IS NULL
            BEGIN
                ALTER TABLE [Master].[Role] ADD [CreatedAtUtc] datetime2 NOT NULL CONSTRAINT [DF_Master_Role_CreatedAtUtc_Alter] DEFAULT (SYSUTCDATETIME()) WITH VALUES;
            END;

            IF NOT EXISTS
            (
                SELECT 1
                FROM sys.indexes
                WHERE name = N'IX_Master_Role_Name'
                    AND object_id = OBJECT_ID(N'[Master].[Role]')
            )
            BEGIN
                CREATE UNIQUE INDEX [IX_Master_Role_Name] ON [Master].[Role] ([Name]);
            END;
            """);

        db.Database.ExecuteSqlRaw(
            """
            IF COL_LENGTH('Users', 'CompanyCode') IS NULL
            BEGIN
                ALTER TABLE [Users] ADD [CompanyCode] nvarchar(50) NULL;
            END;

            IF COL_LENGTH('Users', 'IsLoginAllowed') IS NULL
            BEGIN
                ALTER TABLE [Users] ADD [IsLoginAllowed] bit NOT NULL CONSTRAINT [DF_Users_IsLoginAllowed] DEFAULT ((1)) WITH VALUES;
            END;

            IF COL_LENGTH('Users', 'UpdatedAtUtc') IS NULL
            BEGIN
                ALTER TABLE [Users] ADD [UpdatedAtUtc] datetime2 NOT NULL CONSTRAINT [DF_Users_UpdatedAtUtc] DEFAULT (SYSUTCDATETIME()) WITH VALUES;
            END;
            """);

        db.Database.ExecuteSqlRaw(
            """
            UPDATE [Users] SET [CompanyCode] = 'GLOBAL' WHERE [CompanyCode] IS NULL;
            """);

        db.Database.ExecuteSqlRaw(
            """
            IF EXISTS
            (
                SELECT 1
                FROM sys.columns
                WHERE object_id = OBJECT_ID(N'[Users]')
                    AND name = N'CompanyCode'
                    AND is_nullable = 1
            )
            BEGIN
                ALTER TABLE [Users] ALTER COLUMN [CompanyCode] nvarchar(50) NOT NULL;
            END;
            """);

        db.Database.ExecuteSqlRaw(
            """
            IF COL_LENGTH('PortalDesigns', 'CompanyCode') IS NULL
            BEGIN
                ALTER TABLE [PortalDesigns] ADD [CompanyCode] nvarchar(50) NULL;
            END;

            IF COL_LENGTH('PortalDesigns', 'SiteName') IS NULL
            BEGIN
                ALTER TABLE [PortalDesigns] ADD [SiteName] nvarchar(150) NOT NULL CONSTRAINT [DF_PortalDesigns_SiteName] DEFAULT (N'Unified Project Management Portal') WITH VALUES;
            END;

            IF COL_LENGTH('PortalDesigns', 'SiteSlug') IS NULL
            BEGIN
                ALTER TABLE [PortalDesigns] ADD [SiteSlug] nvarchar(180) NOT NULL CONSTRAINT [DF_PortalDesigns_SiteSlug] DEFAULT (N'unified-project-management-portal') WITH VALUES;
            END;

            IF COL_LENGTH('PortalDesigns', 'PageConfigurationsJson') IS NULL
            BEGIN
                ALTER TABLE [PortalDesigns] ADD [PageConfigurationsJson] nvarchar(max) NULL;
            END;
            """);

        db.Database.ExecuteSqlRaw(
            """
            UPDATE [PortalDesigns] SET [CompanyCode] = 'GLOBAL' WHERE [CompanyCode] IS NULL;
            """);

        db.Database.ExecuteSqlRaw(
            """
            IF EXISTS
            (
                SELECT 1
                FROM sys.columns
                WHERE object_id = OBJECT_ID(N'[PortalDesigns]')
                    AND name = N'CompanyCode'
                    AND is_nullable = 1
            )
            BEGIN
                ALTER TABLE [PortalDesigns] ALTER COLUMN [CompanyCode] nvarchar(50) NOT NULL;
            END;
            """);

        db.Database.ExecuteSqlRaw(
            """
            IF EXISTS
            (
                SELECT 1
                FROM sys.indexes
                WHERE name = N'IX_PortalDesigns_CustomerCode'
                    AND object_id = OBJECT_ID(N'[PortalDesigns]')
            )
            BEGIN
                DROP INDEX [IX_PortalDesigns_CustomerCode] ON [PortalDesigns];
            END;

            IF NOT EXISTS
            (
                SELECT 1
                FROM sys.indexes
                WHERE name = N'IX_PortalDesigns_SiteSlug'
                    AND object_id = OBJECT_ID(N'[PortalDesigns]')
            )
            BEGIN
                CREATE INDEX [IX_PortalDesigns_SiteSlug] ON [PortalDesigns] ([SiteSlug]);
            END;

            IF NOT EXISTS
            (
                SELECT 1
                FROM sys.indexes
                WHERE name = N'IX_PortalDesigns_CompanyCode_CustomerCode'
                    AND object_id = OBJECT_ID(N'[PortalDesigns]')
            )
            BEGIN
                CREATE UNIQUE INDEX [IX_PortalDesigns_CompanyCode_CustomerCode] ON [PortalDesigns] ([CompanyCode], [CustomerCode]);
            END;
            """);
    }
}
