IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Customers] (
    [Custid] int NOT NULL,
    [Name] Varchar(100) NULL,
    [Balance] Money NULL,
    [City] Varchar(100) NULL,
    [Status] bit NOT NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY ([Custid])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250308165433_InitialMigration', N'9.0.2');

COMMIT;
GO

