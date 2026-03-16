using Microsoft.EntityFrameworkCore;

namespace ProjectManagement.Api.Data;

public static class DatabaseInitializer
{
    public static void EnsureRoleMasterTable(AppDbContext db)
    {
        db.Database.ExecuteSqlRaw(
            """
            IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = N'Master')
            BEGIN
                EXEC(N'CREATE SCHEMA [Master]');
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
    }
}
