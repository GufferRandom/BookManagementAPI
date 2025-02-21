IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'BookManagementApi')
BEGIN
    CREATE DATABASE [BookManagementApi];
END