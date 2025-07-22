-- Fixed DDL for LaptopRentalDB on SQL Server (replace ON DELETE RESTRICT with NO ACTION)
USE [master];
GO
IF DB_ID(N'LaptopRentalDB') IS NULL
CREATE DATABASE [LaptopRentalDB];
GO
USE [LaptopRentalDB];
GO

/* 1. Table: Account (User) */
CREATE TABLE [dbo].[Account] (
    [AccountId]    INT           IDENTITY(1,1)    NOT NULL PRIMARY KEY,
    [Email]        NVARCHAR(255) NOT NULL UNIQUE,
    [PasswordHash] NVARCHAR(255) NOT NULL,
    [Role]         NVARCHAR(20)  NOT NULL
    CONSTRAINT chk_Account_Role CHECK ([Role] IN ('Admin','Staff','Customer')),
    [Name]         NVARCHAR(200) NULL,
    [CreatedAt]    DATETIME2     NOT NULL DEFAULT SYSUTCDATETIME()
    );
GO

/* 2. Table: Brand */
CREATE TABLE [dbo].[Brand] (
    [BrandId]     INT           IDENTITY(1,1)    NOT NULL PRIMARY KEY,
    [Name]        NVARCHAR(100) NOT NULL UNIQUE,
    [Description] NVARCHAR(500) NULL,
    [LogoUrl]     NVARCHAR(255) NULL,
    [Country]     NVARCHAR(100) NULL,
    [CreatedAt]   DATETIME2     NOT NULL DEFAULT SYSUTCDATETIME(),
    [UpdatedAt]   DATETIME2     NOT NULL DEFAULT SYSUTCDATETIME()
    );
GO

/* 3. Table: Category */
CREATE TABLE [dbo].[Category] (
    [CategoryId]  INT           IDENTITY(1,1)    NOT NULL PRIMARY KEY,
    [Name]        NVARCHAR(100) NOT NULL UNIQUE,
    [Description] NVARCHAR(500) NULL,
    [CreatedAt]   DATETIME2     NOT NULL DEFAULT SYSUTCDATETIME()
    );
GO

/* 4. Table: Laptop */
CREATE TABLE [dbo].[Laptop] (
    [LaptopId]    INT           IDENTITY(1,1)    NOT NULL PRIMARY KEY,
    [Name]        NVARCHAR(200) NOT NULL,
    [Description] NVARCHAR(1000) NULL,
    [PricePerDay] DECIMAL(18,2) NOT NULL,
    [BrandId]     INT           NOT NULL,
    [CPU]         NVARCHAR(100) NOT NULL,
    [RAM]         INT           NOT NULL,
    [Storage]     INT           NOT NULL,
    [ImageUrl]    NVARCHAR(500) NULL,
    [Status]      NVARCHAR(50)  NOT NULL DEFAULT 'PendingApproval'
    CONSTRAINT chk_Laptop_Status CHECK ([Status] IN ('PendingApproval','Available','InUse','Rejected')),
    [CreatedAt]   DATETIME2     NOT NULL DEFAULT SYSUTCDATETIME(),
    [UpdatedAt]   DATETIME2     NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT fk_Laptop_Brand FOREIGN KEY ([BrandId])
    REFERENCES [dbo].[Brand]([BrandId]) ON DELETE NO ACTION
    );
GO
CREATE INDEX ix_Laptop_BrandId ON [dbo].[Laptop]([BrandId]);
GO

/* 5. Junction Table: LaptopCategory */
CREATE TABLE [dbo].[LaptopCategory] (
    [LaptopId]   INT NOT NULL,
    [CategoryId] INT NOT NULL,
     CONSTRAINT pk_LaptopCategory PRIMARY KEY ([LaptopId],[CategoryId]),
    CONSTRAINT fk_LaptopCategory_Laptop FOREIGN KEY ([LaptopId])
    REFERENCES [dbo].[Laptop]([LaptopId]) ON DELETE CASCADE,
    CONSTRAINT fk_LaptopCategory_Category FOREIGN KEY ([CategoryId])
    REFERENCES [dbo].[Category]([CategoryId]) ON DELETE CASCADE
    );
GO

/* 6. Table: [Order] */
CREATE TABLE [dbo].[Order] (
    [OrderId]     INT           IDENTITY(1,1)    NOT NULL PRIMARY KEY,
    [Status]      NVARCHAR(20)  NOT NULL DEFAULT 'Pending'
    CONSTRAINT chk_Order_Status CHECK ([Status] IN ('Pending','Confirmed','Completed')),
    [TotalCharge] DECIMAL(18,2) NOT NULL,
    [StartDate]   DATE          NOT NULL,
    [EndDate]     DATE          NOT NULL,
    [OwnerId]     INT           NOT NULL,
    [RenterId]    INT           NOT NULL,
    [LaptopId]    INT           NOT NULL,
    [CreatedAt]   DATETIME2     NOT NULL DEFAULT SYSUTCDATETIME(),
    [UpdatedAt]   DATETIME2     NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT chk_Order_Dates CHECK ([EndDate] >= [StartDate]),
    CONSTRAINT fk_Order_Owner FOREIGN KEY ([OwnerId])
    REFERENCES [dbo].[Account]([AccountId]) ON DELETE NO ACTION,
    CONSTRAINT fk_Order_Renter FOREIGN KEY ([RenterId])
    REFERENCES [dbo].[Account]([AccountId]) ON DELETE NO ACTION,
    CONSTRAINT fk_Order_Laptop FOREIGN KEY ([LaptopId])
    REFERENCES [dbo].[Laptop]([LaptopId]) ON DELETE NO ACTION
    );
GO
CREATE INDEX ix_Order_RenterId  ON [dbo].[Order]([RenterId]);
CREATE INDEX ix_Order_OwnerId   ON [dbo].[Order]([OwnerId]);
CREATE INDEX ix_Order_LaptopId  ON [dbo].[Order]([LaptopId]);
CREATE INDEX ix_Order_StartDate ON [dbo].[Order]([StartDate]);
GO

/* 7. Table: Slot (1 ngày thuê = 1 slot) */
CREATE TABLE [dbo].[Slot] (
    [SlotId]    INT         IDENTITY(1,1)    NOT NULL PRIMARY KEY,
    [LaptopId]  INT         NOT NULL,
    [SlotDate]  DATE        NOT NULL,
    [Status]    NVARCHAR(20) NOT NULL DEFAULT 'Available'
    CONSTRAINT chk_Slot_Status CHECK ([Status] IN ('Available','Blocked','Booked')),
    [OrderId]   INT         NULL,
    [CreatedAt] DATETIME2   NOT NULL DEFAULT SYSUTCDATETIME(),
    [UpdatedAt] DATETIME2   NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT uc_Slot_LaptopDate UNIQUE ([LaptopId],[SlotDate]),
    CONSTRAINT fk_Slot_Laptop FOREIGN KEY ([LaptopId])
    REFERENCES [dbo].[Laptop]([LaptopId]) ON DELETE NO ACTION,
    CONSTRAINT fk_Slot_Order FOREIGN KEY ([OrderId])
    REFERENCES [dbo].[Order]([OrderId]) ON DELETE SET NULL
    );
GO
CREATE INDEX ix_Slot_LaptopDate_Status ON [dbo].[Slot]([LaptopId],[SlotDate],[Status]);
GO

/* 8. Table: Review */
CREATE TABLE [dbo].[Review] (
    [ReviewId]   INT           IDENTITY(1,1)    NOT NULL PRIMARY KEY,
    [OrderId]    INT           NOT NULL,
    [RaterId]    INT           NOT NULL,
    [Rating]     TINYINT       NOT NULL CHECK ([Rating] BETWEEN 1 AND 5),
    [Comment]    NVARCHAR(1000) NULL,
    [CreatedAt]  DATETIME2     NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT fk_Review_Order FOREIGN KEY ([OrderId])
    REFERENCES [dbo].[Order]([OrderId]) ON DELETE CASCADE,
    CONSTRAINT fk_Review_Rater FOREIGN KEY ([RaterId])
    REFERENCES [dbo].[Account]([AccountId]) ON DELETE NO ACTION
    );
GO

/* 9. Table: Notification */
CREATE TABLE [dbo].[Notification] (
    [NotificationId] INT           IDENTITY(1,1)    NOT NULL PRIMARY KEY,
    [AccountId]      INT           NOT NULL,
    [Type]           NVARCHAR(50)  NOT NULL,
    [Payload]        NVARCHAR(1000) NULL,
    [IsRead]         BIT           NOT NULL DEFAULT 0,
    [CreatedAt]      DATETIME2     NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT fk_Notification_Account FOREIGN KEY ([AccountId])
    REFERENCES [dbo].[Account]([AccountId]) ON DELETE CASCADE
    );
GO
