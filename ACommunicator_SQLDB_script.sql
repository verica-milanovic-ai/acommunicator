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
 [Name] [nvarchar](255) NOT NULL,
 [Description] [nvarchar](255) NULL,
 [ParentOptionId] int NULL,
 [FolderID] [nvarchar](255) NULL,
 [Level] int NOT NULL,
 [IsDefault] bit NOT NULL DEFAULT 0,
 FOREIGN KEY (ParentOptionId) REFERENCES Options (Id)
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

CREATE TRIGGER OnCreateEndUser 
ON EndUsers
AFTER INSERT
AS 
BEGIN
	INSERT INTO EndUserOption (OptionId, EndUserId)
	SELECT o.Id, i.Id from inserted as i, Options o WHERE o.IsDefault = 1
END
GO

USE [ACommunicator]
GO
SET IDENTITY_INSERT [dbo].[Options] ON 

INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (1, N'Ja zelim', N'Ja zelim', N'0B8UwC5dtixfIUVpkV0l2ekNsMHc', 1, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (2, N'Ja se osecam', N'Ja se osecam', N'0B8UwC5dtixfIUEJvSm1wVmp1czQ', 1, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (3, N'Pitanja i odgovori', N'Pitanja i odgovori', N'0B8UwC5dtixfISzFJUkp2NFFWR3c', 1, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (4, N'Treba mi pomoc', N'Treba mi pomoc', N'0B8UwC5dtixfIREwzejY3QTgtdWc', 1, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (5, N'Kategorije', N'Kategorije', N'0B8UwC5dtixfIRHA1YzVwc2VCNG8', 1, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (6, N'Zelim da jedem', N'Zelim da jedem', N'0B8UwC5dtixfIZzNDMEdmYW8wRkE', 2, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (7, N'Zelim da jedem voce', N'Zelim da jedem voce', N'0B8UwC5dtixfIb0ZwYmRzMDZ1bXc', 3, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (9, N'Zelim da jedem pomorandzu', N'Zelim da jedem pomorandzu', N'0B8UwC5dtixfIb0ZwYmRzMDZ1bXc', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (10, N'Zelim da jedem krusku', N'Zelim da jedem krusku', N'0B8UwC5dtixfIb0ZwYmRzMDZ1bXc', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (11, N'Zelim da jedem jagodu', N'Zelim da jedem jagodu', N'0B8UwC5dtixfIb0ZwYmRzMDZ1bXc', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (12, N'Zelim da jedem jabuku', N'Zelim da jedem jabuku', N'0B8UwC5dtixfIb0ZwYmRzMDZ1bXc', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (13, N'Zelim da jedem povrce', N'Zelim da jedem povrce', N'0B8UwC5dtixfIUUlTZnhnLVJ4bEE', 2, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (14, N'Zelim da jedem zelenu salatu', N'Zelim da jedem zelenu salatu', N'0B8UwC5dtixfIUUlTZnhnLVJ4bEE', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (15, N'Zelim da jedem spanac', N'Zelim da jedem spanac', N'0B8UwC5dtixfIUUlTZnhnLVJ4bEE', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (16, N'Zelim da jedem sargarepu', N'Zelim da jedem sargarepu', N'0B8UwC5dtixfIUUlTZnhnLVJ4bEE', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (17, N'Zelim da jedem paradajz', N'Zelim da jedem paradajz', N'0B8UwC5dtixfIUUlTZnhnLVJ4bEE', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (18, N'Zelim da jedem neko jelo', N'Zelim da jedem neko jelo', N'0B8UwC5dtixfIODhKVmJfVWwxdW8', 3, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (19, N'Zelim da jedem supu', N'Zelim da jedem supu', N'0B8UwC5dtixfIODhKVmJfVWwxdW8', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (22, N'Zelim da jedem spagete', N'Zelim da jedem spagete', N'0B8UwC5dtixfIODhKVmJfVWwxdW8', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (23, N'Zelim da jedem riblje stapice', N'Zelim da jedem riblje stapice', N'0B8UwC5dtixfIODhKVmJfVWwxdW8', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (24, N'Zelim da jedem pomfrit', N'Zelim da jedem pomfrit', N'0B8UwC5dtixfIODhKVmJfVWwxdW8', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (25, N'Zelim da jedem picu', N'Zelim da jedem picu', N'0B8UwC5dtixfIODhKVmJfVWwxdW8', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (26, N'Zelim da jedem jaja', N'Zelim da jedem jaja', N'0B8UwC5dtixfIODhKVmJfVWwxdW8', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (27, N'Zelim da jedem hot dog', N'Zelim da jedem hot dog', N'0B8UwC5dtixfIODhKVmJfVWwxdW8', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (28, N'Zelim da jedem hleb', N'Zelim da jedem hleb', N'0B8UwC5dtixfIODhKVmJfVWwxdW8', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (29, N'Zelim da jedem hamburger', N'Zelim da jedem hamburger', N'0B8UwC5dtixfIODhKVmJfVWwxdW8', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (30, N'Zelim da jedem slatkise', N'Zelim da jedem slatkise', N'0B8UwC5dtixfIM1NCeWJaMUpjWjQ', 3, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (31, N'Zelim da jedem tortu', N'Zelim da jedem tortu', N'0B8UwC5dtixfIM1NCeWJaMUpjWjQ', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (32, N'Zelim da jedem sladoled', N'Zelim da jedem sladoled', N'0B8UwC5dtixfIM1NCeWJaMUpjWjQ', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (33, N'Zelim da jedem puding', N'Zelim da jedem puding', N'0B8UwC5dtixfIM1NCeWJaMUpjWjQ', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (34, N'Zelim da jedem pitu', N'Zelim da jedem pitu', N'0B8UwC5dtixfIM1NCeWJaMUpjWjQ', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (35, N'Zelim da jedem palacinke', N'Zelim da jedem palacinke', N'0B8UwC5dtixfIM1NCeWJaMUpjWjQ', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (36, N'Zelim da jedem kolac', N'Zelim da jedem kolac', N'0B8UwC5dtixfIM1NCeWJaMUpjWjQ', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (38, N'Zelim da jedem keks', N'Zelim da jedem keks', N'0B8UwC5dtixfIM1NCeWJaMUpjWjQ', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (39, N'Zelim da jedem cokoladu', N'Zelim da jedem cokoladu', N'0B8UwC5dtixfIM1NCeWJaMUpjWjQ', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (40, N'Zelim da jedem bombone', N'Zelim da jedem bombone', N'0B8UwC5dtixfIM1NCeWJaMUpjWjQ', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (41, N'Zelim nesto da grickam', N'Zelim nesto da grickam', N'0B8UwC5dtixfIek5WWlk1dWdIT0U', 3, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (42, N'Zelim da jedem smoki', N'Zelim da jedem smoki', N'0B8UwC5dtixfIek5WWlk1dWdIT0U', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (43, N'Zelim da jedem perece', N'Zelim da jedem perece', N'0B8UwC5dtixfIek5WWlk1dWdIT0U', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (44, N'Zelim da jedem kokice', N'Zelim da jedem kokice', N'0B8UwC5dtixfIek5WWlk1dWdIT0U', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (45, N'Zelim da jedem kikiriki', N'Zelim da jedem kikiriki', N'0B8UwC5dtixfIek5WWlk1dWdIT0U', 4, 1)
INSERT [dbo].[Options] ([Id], [Name], [Description],  [FolderID], [Level], [IsDefault]) VALUES (46, N'Ono sto zelim nije tu', N'Ono sto zelim nije tu', N'0B8UwC5dtixfIek5WWlk1dWdIT0U', 4, 1)
SET IDENTITY_INSERT [dbo].[Options] OFF
