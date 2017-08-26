USE master ;  
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'ACommunicator')
DROP DATABASE [ACommunicator]
GO
CREATE DATABASE ACommunicator  
ON   
( NAME = ACommunicator_dat,  
    FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\ACommunicatordat.mdf',  
    SIZE = 10,  
    MAXSIZE = 50,  
    FILEGROWTH = 5 )  
LOG ON  
( NAME = ACommunicator_log,  
    FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\ACommunicatorlog.ldf',  
    SIZE = 5MB,  
    MAXSIZE = 25MB,  
    FILEGROWTH = 5MB ) ;  
GO  

USE [ACommunicator]
GO

CREATE TABLE [dbo].[AUsers]
(   
 [Id] int NOT NULL PRIMARY KEY IDENTITY(1,1),
 [Name] [nvarchar](50) NULL,
 [Username] [nvarchar](50) NOT NULL,
 [Password] [nvarchar](50) NOT NULL,
 [Email] [nvarchar](50) NOT NULL,
 [Telephone] [nvarchar](50) NULL,
 [PicturePath] [nvarchar](255) NULL
) ON [PRIMARY]
GO

USE [ACommunicator]
GO

CREATE TABLE [dbo].[EndUsers]
(   
 [Id] int NOT NULL PRIMARY KEY IDENTITY(1,1),
 [Name] [nvarchar](50) NULL,
 [Username] [nvarchar](50) NOT NULL,
 [PicturePath] [nvarchar](255) NULL
) ON [PRIMARY]
GO

USE [ACommunicator]
GO

CREATE TABLE [dbo].[Options]
(   
 [Id] int NOT NULL PRIMARY KEY IDENTITY(1,1),
 [Name] [nvarchar](20) NOT NULL,
 [Description] [nvarchar](255) NULL,
 [ParentFolderID] [nvarchar](255) NOT NULL,
 [FolderID] [nvarchar](255) NULL,
 [Level] int NOT NULL
) ON [PRIMARY]
GO

USE [ACommunicator]
GO

CREATE TABLE [dbo].[AUserEndUser]
(   
 [AUserId] int NOT NULL,
 [EndUserId] int NOT NULL,
 CONSTRAINT PK_AUserEndUser PRIMARY KEY
    (
        AUserId,
        EndUserId
    ),
 FOREIGN KEY (AUserId) REFERENCES AUsers (Id),
 FOREIGN KEY (EndUserId) REFERENCES EndUsers (Id)
) ON [PRIMARY]
GO

USE [ACommunicator]
GO

CREATE TABLE [dbo].[EndUserOption]
(   
 [EndUserId] int NOT NULL,
 [OptionId] int NOT NULL,
 CONSTRAINT PK_EndUserOption PRIMARY KEY
    (
        EndUserId,
        OptionId
    ),
 FOREIGN KEY (EndUserId) REFERENCES EndUsers (Id),
 FOREIGN KEY (OptionId) REFERENCES Options (Id)
) ON [PRIMARY]
GO