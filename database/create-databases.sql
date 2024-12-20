USE master;
GO

-- Create the litres database if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'litres')
BEGIN
    CREATE DATABASE litres;
    PRINT 'Database "litres" has been created.';
END
ELSE
BEGIN
    PRINT 'Database "litres" already exists.';
END
GO

USE litres;
GO

-- Create the HangFire login without specifying a password
IF NOT EXISTS (SELECT name FROM sys.server_principals WHERE name = N'HangFire')
BEGIN
    CREATE LOGIN HangFire WITH PASSWORD = '$(HANGFIRE_PASSWORD)';
    PRINT 'Login for HangFire user has been created.';
END
ELSE
BEGIN
    PRINT 'Login for HangFire user already exists.';
END
GO

-- Create the HangFire user for the HangFire login if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = N'HangFire')
BEGIN
    CREATE USER [HangFire] FOR LOGIN HangFire;
    PRINT 'HangFire user has been created.';
END
ELSE
BEGIN
    PRINT 'HangFire user already exists.';
END
GO

-- Create the HangFire schema if it doesn't exist
IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE [name] = 'HangFire')
BEGIN
    EXEC ('CREATE SCHEMA [HangFire]');
    PRINT 'Schema "HangFire" has been created.';
END
ELSE
BEGIN
    PRINT 'Schema "HangFire" already exists.';
END
GO

-- Set the owner of the HangFire schema to the HangFire user
ALTER AUTHORIZATION ON SCHEMA::[HangFire] TO [HangFire];
PRINT 'Ownership of schema "HangFire" has been assigned to user "HangFire".';
GO

-- Grant table creation privileges to the HangFire user
GRANT CREATE TABLE TO [HangFire];
PRINT 'Table creation privileges have been granted to user "HangFire".';
GO