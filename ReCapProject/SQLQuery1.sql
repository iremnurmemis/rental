﻿CREATE TABLE [dbo].[Users]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FirstName] VARCHAR(50) NOT NULL, 
    [LastName] VARCHAR(50) NOT NULL, 
    [Email] VARCHAR(50) NOT NULL, 
    [PasswordHash] BINARY(500) NOT NULL, 
    [PasswordSalt] BINARY(500) NOT NULL, 
    [Status] BIT NOT NULL
)CREATE TABLE [dbo].[OperationClaims]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(250) NOT NULL
)
CREATE TABLE [dbo].[UserOperationClaims]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL, 
    [OperationClaimId] INT NOT NULL
)


