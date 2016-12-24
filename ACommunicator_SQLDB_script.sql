USE master ;  
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
 [Id] int NOT NULL PRIMARY KEY,
 [Name] [nchar](50) NULL,
 [Username] [nchar](50) NULL,
 [Password] [nchar](50) NULL,
 [Email] [nchar](50) NULL,
 [Telephone] [nchar](50) NULL,
 [PicturePath] [nchar](50) NULL
) ON [PRIMARY]
GO

USE [ACommunicator]
GO

CREATE TABLE [dbo].[EndUsers]
(   
 [Id] int NOT NULL PRIMARY KEY,
 [Name] [nchar](50) NULL,
 [Password] [nchar](50) NULL,
 [PicturePath] [nchar](50) NULL
) ON [PRIMARY]
GO

USE [ACommunicator]
GO

CREATE TABLE [dbo].[Options]
(   
 [Id] int NOT NULL PRIMARY KEY,
 [Title] [nchar](20) NULL,
 [Description] [nchar](255) NULL,
 [PicturePath] [nchar](50) NULL,
 [SoundPath] [nchar](50) NULL
) ON [PRIMARY]
GO

USE [ACommunicator]
GO

CREATE TABLE [dbo].[AUserEndUser]
(   
 [AdminId] int NOT NULL,
 [EndUserId] int NOT NULL,
 CONSTRAINT PK_AUserEndUser PRIMARY KEY
    (
        AdminId,
        EndUserId
    ),
 FOREIGN KEY (AdminId) REFERENCES AUsers (Id),
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