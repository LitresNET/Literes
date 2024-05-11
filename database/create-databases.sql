-- Используем контекст основной базы данных
USE master;
GO

-- Создаем базу данных litres, если не существует
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'litres')
CREATE DATABASE litres;
GO

-- Используем контекст базы данных litres
USE litres;
GO

-- Создаем логин HangFire и назначаем ему пароль
CREATE LOGIN HangFire WITH PASSWORD = '_BEZ_Par0lya_007_';

-- Создаем пользователя HangFire на основе логина HangFire
CREATE USER [HangFire] FOR LOGIN HangFire;
GO

-- Если схема HangFire не существует, создаем ее
IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE [name] = 'HangFire') EXEC ('CREATE SCHEMA [HangFire]')
GO

-- Назначаем владельца схемы HangFire пользователю HangFire
ALTER AUTHORIZATION ON SCHEMA::[HangFire] TO [HangFire]
GO

-- Предоставляем права на создание таблиц пользователю HangFire
GRANT CREATE TABLE TO [HangFire]
GO
