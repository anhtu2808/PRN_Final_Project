USE [master]
GO
/****** Object:  Database [LaptopRentalDB]    Script Date: 7/31/2025 11:30:33 AM ******/
CREATE DATABASE [LaptopRentalDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'LaptopRentalDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\LaptopRentalDB.mdf' , SIZE = 3136KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'LaptopRentalDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\LaptopRentalDB_log.ldf' , SIZE = 1088KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [LaptopRentalDB] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [LaptopRentalDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [LaptopRentalDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [LaptopRentalDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [LaptopRentalDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [LaptopRentalDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [LaptopRentalDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [LaptopRentalDB] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [LaptopRentalDB] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [LaptopRentalDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [LaptopRentalDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [LaptopRentalDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [LaptopRentalDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [LaptopRentalDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [LaptopRentalDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [LaptopRentalDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [LaptopRentalDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [LaptopRentalDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [LaptopRentalDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [LaptopRentalDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [LaptopRentalDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [LaptopRentalDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [LaptopRentalDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [LaptopRentalDB] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [LaptopRentalDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [LaptopRentalDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [LaptopRentalDB] SET  MULTI_USER 
GO
ALTER DATABASE [LaptopRentalDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [LaptopRentalDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [LaptopRentalDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [LaptopRentalDB] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [LaptopRentalDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 7/31/2025 11:30:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Account]    Script Date: 7/31/2025 11:30:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[AccountId] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[PasswordHash] [nvarchar](255) NOT NULL,
	[Role] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](200) NULL,
	[CreatedAt] [datetime2](7) NOT NULL DEFAULT (sysutcdatetime()),
 CONSTRAINT [PK__Account__349DA5A6B038BC0D] PRIMARY KEY CLUSTERED 
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Brand]    Script Date: 7/31/2025 11:30:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Brand](
	[BrandId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[LogoUrl] [nvarchar](255) NULL,
	[Country] [nvarchar](100) NULL,
	[CreatedAt] [datetime2](7) NOT NULL DEFAULT (sysutcdatetime()),
	[UpdatedAt] [datetime2](7) NOT NULL DEFAULT (sysutcdatetime()),
 CONSTRAINT [PK__Brand__DAD4F05EA61E0654] PRIMARY KEY CLUSTERED 
(
	[BrandId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Category]    Script Date: 7/31/2025 11:30:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[IconClass] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[CreatedAt] [datetime2](7) NOT NULL DEFAULT (sysutcdatetime()),
 CONSTRAINT [PK__Category__19093A0B11FB5713] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ChatMessage]    Script Date: 7/31/2025 11:30:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChatMessage](
	[ChatMessageId] [int] IDENTITY(1,1) NOT NULL,
	[ChatRoomId] [int] NOT NULL,
	[SenderId] [int] NOT NULL,
	[Content] [nvarchar](2000) NOT NULL,
	[MessageType] [nvarchar](20) NOT NULL,
	[SentAt] [datetime2](7) NOT NULL,
	[IsRead] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_ChatMessage] PRIMARY KEY CLUSTERED 
(
	[ChatMessageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ChatRoom]    Script Date: 7/31/2025 11:30:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChatRoom](
	[ChatRoomId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[StaffId] [int] NULL,
	[Subject] [nvarchar](200) NOT NULL,
	[Status] [nvarchar](20) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[LastMessageAt] [datetime2](7) NULL,
	[IsCustomerActive] [bit] NOT NULL,
	[IsStaffActive] [bit] NOT NULL,
 CONSTRAINT [PK_ChatRoom] PRIMARY KEY CLUSTERED 
(
	[ChatRoomId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Laptop]    Script Date: 7/31/2025 11:30:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Laptop](
	[LaptopId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[PricePerDay] [decimal](18, 2) NOT NULL,
	[BrandId] [int] NOT NULL,
	[CPU] [nvarchar](100) NOT NULL,
	[RAM] [int] NOT NULL,
	[Storage] [int] NOT NULL,
	[ImageUrl] [nvarchar](500) NULL,
	[Status] [nvarchar](50) NOT NULL DEFAULT (N'PendingApproval'),
	[CreatedAt] [datetime2](7) NOT NULL DEFAULT (sysutcdatetime()),
	[UpdatedAt] [datetime2](7) NOT NULL DEFAULT (sysutcdatetime()),
	[AccountId] [int] NOT NULL DEFAULT ((0)),
	[Deposit] [decimal](18, 2) NOT NULL DEFAULT ((0.0)),
 CONSTRAINT [PK__Laptop__19F02684D2D04B06] PRIMARY KEY CLUSTERED 
(
	[LaptopId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LaptopCategory]    Script Date: 7/31/2025 11:30:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LaptopCategory](
	[LaptopId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
 CONSTRAINT [pk_LaptopCategory] PRIMARY KEY CLUSTERED 
(
	[LaptopId] ASC,
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Notification]    Script Date: 7/31/2025 11:30:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[NotificationId] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[Payload] [nvarchar](1000) NULL,
	[IsRead] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK__Notifica__20CF2E127411AEBF] PRIMARY KEY CLUSTERED 
(
	[NotificationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Order]    Script Date: 7/31/2025 11:30:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
	[Status] [nvarchar](20) NOT NULL DEFAULT (N'Pending'),
	[TotalCharge] [decimal](18, 2) NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[OwnerId] [int] NOT NULL,
	[RenterId] [int] NULL,
	[LaptopId] [int] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL DEFAULT (sysutcdatetime()),
	[UpdatedAt] [datetime2](7) NOT NULL DEFAULT (sysutcdatetime()),
	[ZaloPayTransactionId] [nvarchar](100) NULL,
	[DepositAmount] [decimal](18, 2) NOT NULL DEFAULT ((0.0)),
	[RentalFee] [decimal](18, 2) NOT NULL DEFAULT ((0.0)),
 CONSTRAINT [PK__Order__C3905BCFAA91515F] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderLogImgs]    Script Date: 7/31/2025 11:30:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderLogImgs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderLogId] [int] NOT NULL,
	[ImgUrl] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_OrderLogImg] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderLogs]    Script Date: 7/31/2025 11:30:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderLogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Content] [nvarchar](1000) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL DEFAULT (sysutcdatetime()),
	[OrderId] [int] NOT NULL,
	[NewStatus] [nvarchar](50) NOT NULL DEFAULT (N''),
	[OldStatus] [nvarchar](50) NOT NULL DEFAULT (N''),
 CONSTRAINT [PK_OrderLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Review]    Script Date: 7/31/2025 11:30:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Review](
	[ReviewId] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[RaterId] [int] NOT NULL,
	[Rating] [tinyint] NOT NULL,
	[Comment] [nvarchar](1000) NULL,
	[CreatedAt] [datetime2](7) NOT NULL DEFAULT (sysutcdatetime()),
 CONSTRAINT [PK__Review__74BC79CEB4CFF5C5] PRIMARY KEY CLUSTERED 
(
	[ReviewId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Slot]    Script Date: 7/31/2025 11:30:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Slot](
	[SlotId] [int] IDENTITY(1,1) NOT NULL,
	[LaptopId] [int] NOT NULL,
	[SlotDate] [date] NOT NULL,
	[Status] [nvarchar](20) NOT NULL DEFAULT (N'Available'),
	[OrderId] [int] NULL,
	[CreatedAt] [datetime2](7) NOT NULL DEFAULT (sysutcdatetime()),
	[UpdatedAt] [datetime2](7) NOT NULL DEFAULT (sysutcdatetime()),
 CONSTRAINT [PK__Slot__0A124AAFFF5C6A81] PRIMARY KEY CLUSTERED 
(
	[SlotId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Ticket]    Script Date: 7/31/2025 11:30:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ticket](
	[TicketId] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[RenterId] [int] NOT NULL,
	[OwnerId] [int] NULL,
	[Content] [nvarchar](2000) NOT NULL,
	[Status] [nvarchar](20) NOT NULL DEFAULT (N'Open'),
	[CreatedAt] [datetime2](7) NOT NULL DEFAULT (sysutcdatetime()),
	[RespondedAt] [datetime2](7) NULL,
	[Response] [nvarchar](2000) NULL,
 CONSTRAINT [PK_Ticket] PRIMARY KEY CLUSTERED 
(
	[TicketId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TicketChatMessage]    Script Date: 7/31/2025 11:30:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TicketChatMessage](
	[TicketChatMessageId] [int] IDENTITY(1,1) NOT NULL,
	[TicketId] [int] NOT NULL,
	[SenderId] [int] NOT NULL,
	[Content] [nvarchar](2000) NOT NULL,
	[SentAt] [datetime2](7) NOT NULL DEFAULT (sysutcdatetime()),
 CONSTRAINT [PK_TicketChatMessage] PRIMARY KEY CLUSTERED 
(
	[TicketChatMessageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250722043300_InitialCreate', N'9.0.7')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250722110150_AddAccountIdToLaptop', N'9.0.7')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250722182219_MakeRenterNullable', N'9.0.7')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250725125433_AddOrderLogsAndImages', N'9.0.7')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250725133815_TenMigrationMoi', N'9.0.7')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250725134021_TenMigrationMoi1', N'9.0.7')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250728145113_addTicketEntity', N'9.0.7')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250728161525_AddChatEntities', N'9.0.7')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250728163338_FixTicketFks', N'9.0.7')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250728164130_AddZaloPayTransactionId', N'9.0.7')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250728185150_AddDepositFields', N'9.0.7')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250729035854_AddChat', N'9.0.7')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250729091154_AddRentalFee', N'9.0.7')
SET IDENTITY_INSERT [dbo].[Account] ON 

INSERT [dbo].[Account] ([AccountId], [Email], [PasswordHash], [Role], [Name], [CreatedAt]) VALUES (1500, N'admin@email.com', N'12345678', N'Admin', NULL, CAST(N'2025-07-29 12:58:00.0520488' AS DateTime2))
INSERT [dbo].[Account] ([AccountId], [Email], [PasswordHash], [Role], [Name], [CreatedAt]) VALUES (1501, N'staff@email.com', N'12345678', N'Staff', NULL, CAST(N'2025-07-29 12:58:00.0520488' AS DateTime2))
INSERT [dbo].[Account] ([AccountId], [Email], [PasswordHash], [Role], [Name], [CreatedAt]) VALUES (1502, N'customer1@email.com', N'12345678', N'Customer', NULL, CAST(N'2025-07-29 12:58:00.0520488' AS DateTime2))
INSERT [dbo].[Account] ([AccountId], [Email], [PasswordHash], [Role], [Name], [CreatedAt]) VALUES (1503, N'customer2@email.com', N'12345678', N'Customer', NULL, CAST(N'2025-07-29 12:58:00.0520488' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Account] OFF
SET IDENTITY_INSERT [dbo].[Brand] ON 

INSERT [dbo].[Brand] ([BrandId], [Name], [Description], [LogoUrl], [Country], [CreatedAt], [UpdatedAt]) VALUES (1500, N'Apple', NULL, N'https://simpleicons.org/icons/apple.svg', NULL, CAST(N'2025-07-29 12:58:00.0520488' AS DateTime2), CAST(N'2025-07-29 12:58:00.0520488' AS DateTime2))
INSERT [dbo].[Brand] ([BrandId], [Name], [Description], [LogoUrl], [Country], [CreatedAt], [UpdatedAt]) VALUES (1501, N'Dell', NULL, N'https://simpleicons.org/icons/dell.svg', NULL, CAST(N'2025-07-29 12:58:00.0520488' AS DateTime2), CAST(N'2025-07-29 12:58:00.0520488' AS DateTime2))
INSERT [dbo].[Brand] ([BrandId], [Name], [Description], [LogoUrl], [Country], [CreatedAt], [UpdatedAt]) VALUES (1502, N'HP', NULL, N'https://simpleicons.org/icons/hp.svg', NULL, CAST(N'2025-07-29 12:58:00.0520488' AS DateTime2), CAST(N'2025-07-29 12:58:00.0520488' AS DateTime2))
INSERT [dbo].[Brand] ([BrandId], [Name], [Description], [LogoUrl], [Country], [CreatedAt], [UpdatedAt]) VALUES (1503, N'Lenovo', NULL, N'https://simpleicons.org/icons/lenovo.svg', NULL, CAST(N'2025-07-29 12:58:00.0520488' AS DateTime2), CAST(N'2025-07-29 12:58:00.0520488' AS DateTime2))
INSERT [dbo].[Brand] ([BrandId], [Name], [Description], [LogoUrl], [Country], [CreatedAt], [UpdatedAt]) VALUES (1504, N'Asus', NULL, N'https://simpleicons.org/icons/asus.svg', NULL, CAST(N'2025-07-29 12:58:00.0520488' AS DateTime2), CAST(N'2025-07-29 12:58:00.0520488' AS DateTime2))
INSERT [dbo].[Brand] ([BrandId], [Name], [Description], [LogoUrl], [Country], [CreatedAt], [UpdatedAt]) VALUES (1505, N'Acer', NULL, N'https://simpleicons.org/icons/acer.svg', NULL, CAST(N'2025-07-29 12:58:00.0520488' AS DateTime2), CAST(N'2025-07-29 12:58:00.0520488' AS DateTime2))
INSERT [dbo].[Brand] ([BrandId], [Name], [Description], [LogoUrl], [Country], [CreatedAt], [UpdatedAt]) VALUES (1506, N'Microsoft', NULL, N'https://upload.wikimedia.org/wikipedia/commons/4/44/Microsoft_logo.svg', NULL, CAST(N'2025-07-29 12:58:00.0520488' AS DateTime2), CAST(N'2025-07-29 12:58:00.0520488' AS DateTime2))
INSERT [dbo].[Brand] ([BrandId], [Name], [Description], [LogoUrl], [Country], [CreatedAt], [UpdatedAt]) VALUES (1507, N'Samsung', NULL, N'https://simpleicons.org/icons/samsung.svg', NULL, CAST(N'2025-07-29 12:58:00.0520488' AS DateTime2), CAST(N'2025-07-29 12:58:00.0520488' AS DateTime2))
INSERT [dbo].[Brand] ([BrandId], [Name], [Description], [LogoUrl], [Country], [CreatedAt], [UpdatedAt]) VALUES (1508, N'Google', NULL, N'https://simpleicons.org/icons/google.svg', NULL, CAST(N'2025-07-29 12:58:00.0520488' AS DateTime2), CAST(N'2025-07-29 12:58:00.0520488' AS DateTime2))
INSERT [dbo].[Brand] ([BrandId], [Name], [Description], [LogoUrl], [Country], [CreatedAt], [UpdatedAt]) VALUES (1509, N'Huawei', NULL, N'https://simpleicons.org/icons/huawei.svg', NULL, CAST(N'2025-07-29 12:58:00.0520488' AS DateTime2), CAST(N'2025-07-29 12:58:00.0520488' AS DateTime2))
INSERT [dbo].[Brand] ([BrandId], [Name], [Description], [LogoUrl], [Country], [CreatedAt], [UpdatedAt]) VALUES (1510, N'Alienware', NULL, N'https://simpleicons.org/icons/alienware.svg', NULL, CAST(N'2025-07-29 12:58:00.0520488' AS DateTime2), CAST(N'2025-07-29 12:58:00.0520488' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Brand] OFF
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([CategoryId], [Name], [IconClass], [Description], [CreatedAt]) VALUES (1, N'Gaming', N'fas fa-gamepad', N'High-performance laptops optimized for gaming', CAST(N'2025-07-22 04:39:36.0271252' AS DateTime2))
INSERT [dbo].[Category] ([CategoryId], [Name], [IconClass], [Description], [CreatedAt]) VALUES (2, N'Ultrabook', N'fas fa-laptop', N'Thin, lightweight laptops with long battery life', CAST(N'2025-07-22 04:39:36.0271252' AS DateTime2))
INSERT [dbo].[Category] ([CategoryId], [Name], [IconClass], [Description], [CreatedAt]) VALUES (3, N'Business', N'fas fa-briefcase', N'Secure and reliable laptops for enterprise use', CAST(N'2025-07-22 04:39:36.0271252' AS DateTime2))
INSERT [dbo].[Category] ([CategoryId], [Name], [IconClass], [Description], [CreatedAt]) VALUES (4, N'Student', N'fas fa-user-graduate', N'Affordable laptops tailored for students', CAST(N'2025-07-22 04:39:36.0271252' AS DateTime2))
INSERT [dbo].[Category] ([CategoryId], [Name], [IconClass], [Description], [CreatedAt]) VALUES (5, N'Convertible', N'fas fa-tablet-alt', N'2-in-1 laptops with a foldable touchscreen', CAST(N'2025-07-22 04:39:36.0271252' AS DateTime2))
INSERT [dbo].[Category] ([CategoryId], [Name], [IconClass], [Description], [CreatedAt]) VALUES (6, N'Budget 2', N'fas fa-wallet', N'Entry-level laptops with basic configurations', CAST(N'2025-07-22 04:39:36.0271252' AS DateTime2))
INSERT [dbo].[Category] ([CategoryId], [Name], [IconClass], [Description], [CreatedAt]) VALUES (7, N'High-End', N'fas fa-gem', N'Premium laptops with top-tier hardware', CAST(N'2025-07-22 04:39:36.0271252' AS DateTime2))
INSERT [dbo].[Category] ([CategoryId], [Name], [IconClass], [Description], [CreatedAt]) VALUES (8, N'Workstation', N'fas fa-cogs', N'Mobile workstations for CAD and heavy graphics', CAST(N'2025-07-22 04:39:36.0271252' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Category] OFF
SET IDENTITY_INSERT [dbo].[Laptop] ON 

INSERT [dbo].[Laptop] ([LaptopId], [Name], [Description], [PricePerDay], [BrandId], [CPU], [RAM], [Storage], [ImageUrl], [Status], [CreatedAt], [UpdatedAt], [AccountId], [Deposit]) VALUES (1000, N'Alienware M15 R7', N'Powerful gaming laptop with RTX 3070 Ti', CAST(500000.00 AS Decimal(18, 2)), 1510, N'Intel Core i9-12900H', 32, 1000, N'https://hungphatlaptop.com/wp-content/uploads/2022/06/Alienware-M15-R7-2022-H1.jpeg', N'Available', CAST(N'2025-07-29 12:58:00.0570000' AS DateTime2), CAST(N'2025-07-29 12:58:00.0570000' AS DateTime2), 1502, CAST(600000.00 AS Decimal(18, 2)))
INSERT [dbo].[Laptop] ([LaptopId], [Name], [Description], [PricePerDay], [BrandId], [CPU], [RAM], [Storage], [ImageUrl], [Status], [CreatedAt], [UpdatedAt], [AccountId], [Deposit]) VALUES (1001, N'HP EliteBook 840 G9', N'Slim and secure business laptop', CAST(350000.00 AS Decimal(18, 2)), 1502, N'Intel Core i7-1265U', 16, 512, N'https://mayxaugiacao.com/wp-content/uploads/2021/07/hp-elitebook-850-g7-mayxaugiacao.jpg', N'Available', CAST(N'2025-07-29 12:58:00.0570000' AS DateTime2), CAST(N'2025-07-29 12:58:00.0570000' AS DateTime2), 1502, CAST(750000.00 AS Decimal(18, 2)))
INSERT [dbo].[Laptop] ([LaptopId], [Name], [Description], [PricePerDay], [BrandId], [CPU], [RAM], [Storage], [ImageUrl], [Status], [CreatedAt], [UpdatedAt], [AccountId], [Deposit]) VALUES (1002, N'MacBook Air M2', N'Lightweight laptop with Apple Silicon chip', CAST(400000.00 AS Decimal(18, 2)), 1500, N'Apple M2', 8, 512, N'https://minhtuanmobile.com/uploads/products/241107043209-macbook-air-13-m2-8gpu-8cpu-16g-256gb-starlight.jpeg', N'Available', CAST(N'2025-07-29 12:58:00.0570000' AS DateTime2), CAST(N'2025-07-29 12:58:00.0570000' AS DateTime2), 1503, CAST(800000.00 AS Decimal(18, 2)))
INSERT [dbo].[Laptop] ([LaptopId], [Name], [Description], [PricePerDay], [BrandId], [CPU], [RAM], [Storage], [ImageUrl], [Status], [CreatedAt], [UpdatedAt], [AccountId], [Deposit]) VALUES (1003, N'Acer Aspire 5', N'Affordable laptop for students', CAST(200000.00 AS Decimal(18, 2)), 1505, N'Intel Core i5-1235U', 8, 256, N'https://anphat.com.vn/media/product/45049_1.jpg', N'Available', CAST(N'2025-07-29 12:58:00.0570000' AS DateTime2), CAST(N'2025-07-29 12:58:00.0570000' AS DateTime2), 1503, CAST(500000.00 AS Decimal(18, 2)))
INSERT [dbo].[Laptop] ([LaptopId], [Name], [Description], [PricePerDay], [BrandId], [CPU], [RAM], [Storage], [ImageUrl], [Status], [CreatedAt], [UpdatedAt], [AccountId], [Deposit]) VALUES (1004, N'Lenovo ThinkPad P1 Gen 5', N'Mobile workstation with NVIDIA RTX A2000', CAST(100000.00 AS Decimal(18, 2)), 1503, N'Intel Core i7-12700H', 32, 1000, N'https://macstores.vn/wp-content/uploads/2022/09/lenovo-thinkpad-p1-gen-5-16-1.jpg', N'Available', CAST(N'2025-07-29 12:58:00.0570000' AS DateTime2), CAST(N'2025-07-29 12:58:00.0570000' AS DateTime2), 1502, CAST(300000.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[Laptop] OFF
SET IDENTITY_INSERT [dbo].[Order] ON 

INSERT [dbo].[Order] ([OrderId], [Status], [TotalCharge], [StartDate], [EndDate], [OwnerId], [RenterId], [LaptopId], [CreatedAt], [UpdatedAt], [ZaloPayTransactionId], [DepositAmount], [RentalFee]) VALUES (1, N'Cancelled', CAST(120.00 AS Decimal(18, 2)), CAST(N'2025-07-29' AS Date), CAST(N'2025-07-29' AS Date), 1503, 1502, 1002, CAST(N'2025-07-29 13:01:26.4262662' AS DateTime2), CAST(N'2025-07-29 13:03:26.4941059' AS DateTime2), N'250729_1_638894160865345635', CAST(80.00 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)))
INSERT [dbo].[Order] ([OrderId], [Status], [TotalCharge], [StartDate], [EndDate], [OwnerId], [RenterId], [LaptopId], [CreatedAt], [UpdatedAt], [ZaloPayTransactionId], [DepositAmount], [RentalFee]) VALUES (2, N'Renting', CAST(1200000.00 AS Decimal(18, 2)), CAST(N'2025-07-29' AS Date), CAST(N'2025-07-29' AS Date), 1503, 1502, 1002, CAST(N'2025-07-29 13:02:49.7214158' AS DateTime2), CAST(N'2025-07-29 13:03:13.1460387' AS DateTime2), N'250729_2_638894161697299546', CAST(800000.00 AS Decimal(18, 2)), CAST(400000.00 AS Decimal(18, 2)))
INSERT [dbo].[Order] ([OrderId], [Status], [TotalCharge], [StartDate], [EndDate], [OwnerId], [RenterId], [LaptopId], [CreatedAt], [UpdatedAt], [ZaloPayTransactionId], [DepositAmount], [RentalFee]) VALUES (3, N'Pending', CAST(1100000.00 AS Decimal(18, 2)), CAST(N'2025-07-30' AS Date), CAST(N'2025-07-30' AS Date), 1502, 1502, 1000, CAST(N'2025-07-29 13:21:29.3617844' AS DateTime2), CAST(N'2025-07-29 13:21:54.9031737' AS DateTime2), N'250729_3_638894172894067555', CAST(600000.00 AS Decimal(18, 2)), CAST(500000.00 AS Decimal(18, 2)))
INSERT [dbo].[Order] ([OrderId], [Status], [TotalCharge], [StartDate], [EndDate], [OwnerId], [RenterId], [LaptopId], [CreatedAt], [UpdatedAt], [ZaloPayTransactionId], [DepositAmount], [RentalFee]) VALUES (4, N'Completed', CAST(1200000.00 AS Decimal(18, 2)), CAST(N'2025-07-30' AS Date), CAST(N'2025-07-30' AS Date), 1503, 1502, 1002, CAST(N'2025-07-29 13:23:00.1582365' AS DateTime2), CAST(N'2025-07-29 13:23:16.1330180' AS DateTime2), N'250729_4_638894173801646927', CAST(800000.00 AS Decimal(18, 2)), CAST(400000.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[Order] OFF
SET IDENTITY_INSERT [dbo].[OrderLogImgs] ON 

INSERT [dbo].[OrderLogImgs] ([Id], [OrderLogId], [ImgUrl]) VALUES (1, 6, N'https://metro-content-image-bucket.s3.amazonaws.com/laptops/74da7604-e7df-4067-80fc-3d5087035bc3.png')
SET IDENTITY_INSERT [dbo].[OrderLogImgs] OFF
SET IDENTITY_INSERT [dbo].[OrderLogs] ON 

INSERT [dbo].[OrderLogs] ([Id], [Content], [CreatedAt], [OrderId], [NewStatus], [OldStatus]) VALUES (1, N'Rental request has been accepted', CAST(N'2025-07-29 13:03:32.5414045' AS DateTime2), 2, N'Approved', N'Pending')
INSERT [dbo].[OrderLogs] ([Id], [Content], [CreatedAt], [OrderId], [NewStatus], [OldStatus]) VALUES (2, N'Laptop is being delivered to you', CAST(N'2025-07-29 13:03:53.7097330' AS DateTime2), 2, N'Delivering', N'Approved')
INSERT [dbo].[OrderLogs] ([Id], [Content], [CreatedAt], [OrderId], [NewStatus], [OldStatus]) VALUES (3, N'Laptop has been delivered successfully', CAST(N'2025-07-29 13:03:59.3876650' AS DateTime2), 2, N'Renting', N'Delivering')
INSERT [dbo].[OrderLogs] ([Id], [Content], [CreatedAt], [OrderId], [NewStatus], [OldStatus]) VALUES (4, N'Rental request has been accepted', CAST(N'2025-07-29 13:23:36.8905131' AS DateTime2), 4, N'Approved', N'Pending')
INSERT [dbo].[OrderLogs] ([Id], [Content], [CreatedAt], [OrderId], [NewStatus], [OldStatus]) VALUES (5, N'Laptop is being delivered to you', CAST(N'2025-07-29 13:26:04.5580756' AS DateTime2), 4, N'Delivering', N'Approved')
INSERT [dbo].[OrderLogs] ([Id], [Content], [CreatedAt], [OrderId], [NewStatus], [OldStatus]) VALUES (6, N'Laptop has been delivered failed.
Reason: tới nơi gọi khong nghe máy', CAST(N'2025-07-29 13:26:41.3417398' AS DateTime2), 4, N'Delivering', N'Delivering')
INSERT [dbo].[OrderLogs] ([Id], [Content], [CreatedAt], [OrderId], [NewStatus], [OldStatus]) VALUES (7, N'Laptop has been delivered successfully', CAST(N'2025-07-29 13:26:58.2564776' AS DateTime2), 4, N'Renting', N'Delivering')
SET IDENTITY_INSERT [dbo].[OrderLogs] OFF
SET IDENTITY_INSERT [dbo].[Review] ON 

INSERT [dbo].[Review] ([ReviewId], [OrderId], [RaterId], [Rating], [Comment], [CreatedAt]) VALUES (1, 4, 1502, 1, N'laptop oke giao hang nhanh', CAST(N'2025-07-29 13:37:18.4730961' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Review] OFF
SET IDENTITY_INSERT [dbo].[Slot] ON 

INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (1, 1000, CAST(N'2025-07-30' AS Date), N'Unavailable', 3, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (2, 1000, CAST(N'2025-07-31' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (3, 1000, CAST(N'2025-08-01' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (4, 1000, CAST(N'2025-08-02' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (5, 1000, CAST(N'2025-08-03' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (6, 1000, CAST(N'2025-08-04' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (7, 1000, CAST(N'2025-08-05' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (8, 1000, CAST(N'2025-08-06' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (9, 1000, CAST(N'2025-08-07' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (10, 1000, CAST(N'2025-08-08' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (11, 1000, CAST(N'2025-08-09' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (12, 1000, CAST(N'2025-08-10' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (13, 1000, CAST(N'2025-08-11' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (14, 1000, CAST(N'2025-08-12' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (15, 1000, CAST(N'2025-08-13' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (16, 1000, CAST(N'2025-08-14' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (17, 1000, CAST(N'2025-08-15' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (18, 1001, CAST(N'2025-07-30' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (19, 1001, CAST(N'2025-07-31' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (20, 1001, CAST(N'2025-08-01' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (21, 1001, CAST(N'2025-08-02' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (22, 1001, CAST(N'2025-08-03' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (23, 1001, CAST(N'2025-08-04' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (24, 1001, CAST(N'2025-08-05' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (25, 1001, CAST(N'2025-08-06' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (26, 1001, CAST(N'2025-08-07' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (27, 1001, CAST(N'2025-08-08' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (28, 1001, CAST(N'2025-08-09' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (29, 1001, CAST(N'2025-08-10' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (30, 1001, CAST(N'2025-08-11' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (31, 1001, CAST(N'2025-08-12' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (32, 1001, CAST(N'2025-08-13' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (33, 1001, CAST(N'2025-08-14' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (34, 1001, CAST(N'2025-08-15' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (35, 1002, CAST(N'2025-07-28' AS Date), N'Booked', 4, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (36, 1002, CAST(N'2025-07-31' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (37, 1002, CAST(N'2025-08-01' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (38, 1002, CAST(N'2025-08-02' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (39, 1002, CAST(N'2025-08-03' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (40, 1002, CAST(N'2025-08-04' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (41, 1002, CAST(N'2025-08-05' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (42, 1002, CAST(N'2025-08-06' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (43, 1002, CAST(N'2025-08-07' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (44, 1002, CAST(N'2025-08-08' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (45, 1002, CAST(N'2025-08-09' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (46, 1002, CAST(N'2025-08-10' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (47, 1002, CAST(N'2025-08-11' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (48, 1002, CAST(N'2025-08-12' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (49, 1002, CAST(N'2025-08-13' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (50, 1002, CAST(N'2025-08-14' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (51, 1002, CAST(N'2025-08-15' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (52, 1003, CAST(N'2025-07-30' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (53, 1003, CAST(N'2025-07-31' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (54, 1003, CAST(N'2025-08-01' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (55, 1003, CAST(N'2025-08-02' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (56, 1003, CAST(N'2025-08-03' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (57, 1003, CAST(N'2025-08-04' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (58, 1003, CAST(N'2025-08-05' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (59, 1003, CAST(N'2025-08-06' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (60, 1003, CAST(N'2025-08-07' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (61, 1003, CAST(N'2025-08-08' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (62, 1003, CAST(N'2025-08-09' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (63, 1003, CAST(N'2025-08-10' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (64, 1003, CAST(N'2025-08-11' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (65, 1003, CAST(N'2025-08-12' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (66, 1003, CAST(N'2025-08-13' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (67, 1003, CAST(N'2025-08-14' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (68, 1003, CAST(N'2025-08-15' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (69, 1004, CAST(N'2025-07-30' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (70, 1004, CAST(N'2025-07-31' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (71, 1004, CAST(N'2025-08-01' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (72, 1004, CAST(N'2025-08-02' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (73, 1004, CAST(N'2025-08-03' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (74, 1004, CAST(N'2025-08-04' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (75, 1004, CAST(N'2025-08-05' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (76, 1004, CAST(N'2025-08-06' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (77, 1004, CAST(N'2025-08-07' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (78, 1004, CAST(N'2025-08-08' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (79, 1004, CAST(N'2025-08-09' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (80, 1004, CAST(N'2025-08-10' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (81, 1004, CAST(N'2025-08-11' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (82, 1004, CAST(N'2025-08-12' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (83, 1004, CAST(N'2025-08-13' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (84, 1004, CAST(N'2025-08-14' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (85, 1004, CAST(N'2025-08-15' AS Date), N'Available', NULL, CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2), CAST(N'2025-07-29 12:59:28.5570000' AS DateTime2))
INSERT [dbo].[Slot] ([SlotId], [LaptopId], [SlotDate], [Status], [OrderId], [CreatedAt], [UpdatedAt]) VALUES (86, 1002, CAST(N'2025-07-24' AS Date), N'Booked', 2, CAST(N'2025-07-29 13:01:10.3172475' AS DateTime2), CAST(N'2025-07-29 13:01:10.3172832' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Slot] OFF
SET IDENTITY_INSERT [dbo].[Ticket] ON 

INSERT [dbo].[Ticket] ([TicketId], [OrderId], [RenterId], [OwnerId], [Content], [Status], [CreatedAt], [RespondedAt], [Response]) VALUES (1, 2, 1502, 1503, N'Giao trước 8 giờ luôn được không anh ơi', N'Open', CAST(N'2025-07-29 13:03:48.4123762' AS DateTime2), NULL, NULL)
INSERT [dbo].[Ticket] ([TicketId], [OrderId], [RenterId], [OwnerId], [Content], [Status], [CreatedAt], [RespondedAt], [Response]) VALUES (2, 2, 1502, 1503, N'giao hang tre', N'InProgress', CAST(N'2025-07-29 13:18:49.2706432' AS DateTime2), CAST(N'2025-07-29 13:34:43.4718374' AS DateTime2), NULL)
SET IDENTITY_INSERT [dbo].[Ticket] OFF
SET IDENTITY_INSERT [dbo].[TicketChatMessage] ON 

INSERT [dbo].[TicketChatMessage] ([TicketChatMessageId], [TicketId], [SenderId], [Content], [SentAt]) VALUES (1, 2, 1502, N'nguoi cho thu giao may bi loi', CAST(N'2025-07-29 13:33:38.9482431' AS DateTime2))
INSERT [dbo].[TicketChatMessage] ([TicketChatMessageId], [TicketId], [SenderId], [Content], [SentAt]) VALUES (2, 2, 1503, N'toi giao dung roi', CAST(N'2025-07-29 13:33:55.9146437' AS DateTime2))
SET IDENTITY_INSERT [dbo].[TicketChatMessage] OFF
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ__Account__A9D10534465320BF]    Script Date: 7/31/2025 11:30:33 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ__Account__A9D10534465320BF] ON [dbo].[Account]
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ__Brand__737584F6D23B21A2]    Script Date: 7/31/2025 11:30:33 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ__Brand__737584F6D23B21A2] ON [dbo].[Brand]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ__Category__737584F6123D8C29]    Script Date: 7/31/2025 11:30:33 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ__Category__737584F6123D8C29] ON [dbo].[Category]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ChatMessage_ChatRoomId]    Script Date: 7/31/2025 11:30:33 AM ******/
CREATE NONCLUSTERED INDEX [IX_ChatMessage_ChatRoomId] ON [dbo].[ChatMessage]
(
	[ChatRoomId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ChatMessage_SenderId]    Script Date: 7/31/2025 11:30:33 AM ******/
CREATE NONCLUSTERED INDEX [IX_ChatMessage_SenderId] ON [dbo].[ChatMessage]
(
	[SenderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ChatRoom_CustomerId]    Script Date: 7/31/2025 11:30:33 AM ******/
CREATE NONCLUSTERED INDEX [IX_ChatRoom_CustomerId] ON [dbo].[ChatRoom]
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ChatRoom_StaffId]    Script Date: 7/31/2025 11:30:33 AM ******/
CREATE NONCLUSTERED INDEX [IX_ChatRoom_StaffId] ON [dbo].[ChatRoom]
(
	[StaffId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ix_Laptop_AccountId]    Script Date: 7/31/2025 11:30:33 AM ******/
CREATE NONCLUSTERED INDEX [ix_Laptop_AccountId] ON [dbo].[Laptop]
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ix_Laptop_BrandId]    Script Date: 7/31/2025 11:30:33 AM ******/
CREATE NONCLUSTERED INDEX [ix_Laptop_BrandId] ON [dbo].[Laptop]
(
	[BrandId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_LaptopCategory_CategoryId]    Script Date: 7/31/2025 11:30:33 AM ******/
CREATE NONCLUSTERED INDEX [IX_LaptopCategory_CategoryId] ON [dbo].[LaptopCategory]
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Notification_AccountId]    Script Date: 7/31/2025 11:30:33 AM ******/
CREATE NONCLUSTERED INDEX [IX_Notification_AccountId] ON [dbo].[Notification]
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ix_Order_LaptopId]    Script Date: 7/31/2025 11:30:33 AM ******/
CREATE NONCLUSTERED INDEX [ix_Order_LaptopId] ON [dbo].[Order]
(
	[LaptopId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ix_Order_OwnerId]    Script Date: 7/31/2025 11:30:33 AM ******/
CREATE NONCLUSTERED INDEX [ix_Order_OwnerId] ON [dbo].[Order]
(
	[OwnerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ix_Order_RenterId]    Script Date: 7/31/2025 11:30:33 AM ******/
CREATE NONCLUSTERED INDEX [ix_Order_RenterId] ON [dbo].[Order]
(
	[RenterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ix_Order_StartDate]    Script Date: 7/31/2025 11:30:33 AM ******/
CREATE NONCLUSTERED INDEX [ix_Order_StartDate] ON [dbo].[Order]
(
	[StartDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderLogImgs_OrderLogId]    Script Date: 7/31/2025 11:30:33 AM ******/
CREATE NONCLUSTERED INDEX [IX_OrderLogImgs_OrderLogId] ON [dbo].[OrderLogImgs]
(
	[OrderLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderLogs_OrderId]    Script Date: 7/31/2025 11:30:33 AM ******/
CREATE NONCLUSTERED INDEX [IX_OrderLogs_OrderId] ON [dbo].[OrderLogs]
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Review_OrderId]    Script Date: 7/31/2025 11:30:33 AM ******/
CREATE NONCLUSTERED INDEX [IX_Review_OrderId] ON [dbo].[Review]
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Review_RaterId]    Script Date: 7/31/2025 11:30:33 AM ******/
CREATE NONCLUSTERED INDEX [IX_Review_RaterId] ON [dbo].[Review]
(
	[RaterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [ix_Slot_LaptopDate_Status]    Script Date: 7/31/2025 11:30:33 AM ******/
CREATE NONCLUSTERED INDEX [ix_Slot_LaptopDate_Status] ON [dbo].[Slot]
(
	[LaptopId] ASC,
	[SlotDate] ASC,
	[Status] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Slot_OrderId]    Script Date: 7/31/2025 11:30:33 AM ******/
CREATE NONCLUSTERED INDEX [IX_Slot_OrderId] ON [dbo].[Slot]
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [uc_Slot_LaptopDate]    Script Date: 7/31/2025 11:30:33 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [uc_Slot_LaptopDate] ON [dbo].[Slot]
(
	[LaptopId] ASC,
	[SlotDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Ticket_OrderId]    Script Date: 7/31/2025 11:30:33 AM ******/
CREATE NONCLUSTERED INDEX [IX_Ticket_OrderId] ON [dbo].[Ticket]
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Ticket_OwnerId]    Script Date: 7/31/2025 11:30:33 AM ******/
CREATE NONCLUSTERED INDEX [IX_Ticket_OwnerId] ON [dbo].[Ticket]
(
	[OwnerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Ticket_RenterId]    Script Date: 7/31/2025 11:30:33 AM ******/
CREATE NONCLUSTERED INDEX [IX_Ticket_RenterId] ON [dbo].[Ticket]
(
	[RenterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TicketChatMessage_SenderId]    Script Date: 7/31/2025 11:30:33 AM ******/
CREATE NONCLUSTERED INDEX [IX_TicketChatMessage_SenderId] ON [dbo].[TicketChatMessage]
(
	[SenderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TicketChatMessage_TicketId]    Script Date: 7/31/2025 11:30:33 AM ******/
CREATE NONCLUSTERED INDEX [IX_TicketChatMessage_TicketId] ON [dbo].[TicketChatMessage]
(
	[TicketId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ChatMessage] ADD  DEFAULT (N'Text') FOR [MessageType]
GO
ALTER TABLE [dbo].[ChatMessage] ADD  DEFAULT (sysutcdatetime()) FOR [SentAt]
GO
ALTER TABLE [dbo].[ChatRoom] ADD  DEFAULT (N'Open') FOR [Status]
GO
ALTER TABLE [dbo].[ChatRoom] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Notification] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[ChatMessage]  WITH CHECK ADD  CONSTRAINT [fk_ChatMessage_ChatRoom] FOREIGN KEY([ChatRoomId])
REFERENCES [dbo].[ChatRoom] ([ChatRoomId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ChatMessage] CHECK CONSTRAINT [fk_ChatMessage_ChatRoom]
GO
ALTER TABLE [dbo].[ChatMessage]  WITH CHECK ADD  CONSTRAINT [fk_ChatMessage_Sender] FOREIGN KEY([SenderId])
REFERENCES [dbo].[Account] ([AccountId])
GO
ALTER TABLE [dbo].[ChatMessage] CHECK CONSTRAINT [fk_ChatMessage_Sender]
GO
ALTER TABLE [dbo].[ChatRoom]  WITH CHECK ADD  CONSTRAINT [fk_ChatRoom_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Account] ([AccountId])
GO
ALTER TABLE [dbo].[ChatRoom] CHECK CONSTRAINT [fk_ChatRoom_Customer]
GO
ALTER TABLE [dbo].[ChatRoom]  WITH CHECK ADD  CONSTRAINT [fk_ChatRoom_Staff] FOREIGN KEY([StaffId])
REFERENCES [dbo].[Account] ([AccountId])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[ChatRoom] CHECK CONSTRAINT [fk_ChatRoom_Staff]
GO
ALTER TABLE [dbo].[Laptop]  WITH CHECK ADD  CONSTRAINT [fk_Laptop_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([AccountId])
GO
ALTER TABLE [dbo].[Laptop] CHECK CONSTRAINT [fk_Laptop_Account]
GO
ALTER TABLE [dbo].[Laptop]  WITH CHECK ADD  CONSTRAINT [fk_Laptop_Brand] FOREIGN KEY([BrandId])
REFERENCES [dbo].[Brand] ([BrandId])
GO
ALTER TABLE [dbo].[Laptop] CHECK CONSTRAINT [fk_Laptop_Brand]
GO
ALTER TABLE [dbo].[LaptopCategory]  WITH CHECK ADD  CONSTRAINT [fk_LaptopCategory_Category] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([CategoryId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LaptopCategory] CHECK CONSTRAINT [fk_LaptopCategory_Category]
GO
ALTER TABLE [dbo].[LaptopCategory]  WITH CHECK ADD  CONSTRAINT [fk_LaptopCategory_Laptop] FOREIGN KEY([LaptopId])
REFERENCES [dbo].[Laptop] ([LaptopId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LaptopCategory] CHECK CONSTRAINT [fk_LaptopCategory_Laptop]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [fk_Notification_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([AccountId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [fk_Notification_Account]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [fk_Order_Laptop] FOREIGN KEY([LaptopId])
REFERENCES [dbo].[Laptop] ([LaptopId])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [fk_Order_Laptop]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [fk_Order_Owner] FOREIGN KEY([OwnerId])
REFERENCES [dbo].[Account] ([AccountId])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [fk_Order_Owner]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [fk_Order_Renter] FOREIGN KEY([RenterId])
REFERENCES [dbo].[Account] ([AccountId])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [fk_Order_Renter]
GO
ALTER TABLE [dbo].[OrderLogImgs]  WITH CHECK ADD  CONSTRAINT [FK_OrderLogImg_OrderLog] FOREIGN KEY([OrderLogId])
REFERENCES [dbo].[OrderLogs] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderLogImgs] CHECK CONSTRAINT [FK_OrderLogImg_OrderLog]
GO
ALTER TABLE [dbo].[OrderLogs]  WITH CHECK ADD  CONSTRAINT [FK_OrderLog_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([OrderId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderLogs] CHECK CONSTRAINT [FK_OrderLog_Order]
GO
ALTER TABLE [dbo].[Review]  WITH CHECK ADD  CONSTRAINT [fk_Review_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([OrderId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Review] CHECK CONSTRAINT [fk_Review_Order]
GO
ALTER TABLE [dbo].[Review]  WITH CHECK ADD  CONSTRAINT [fk_Review_Rater] FOREIGN KEY([RaterId])
REFERENCES [dbo].[Account] ([AccountId])
GO
ALTER TABLE [dbo].[Review] CHECK CONSTRAINT [fk_Review_Rater]
GO
ALTER TABLE [dbo].[Slot]  WITH CHECK ADD  CONSTRAINT [fk_Slot_Laptop] FOREIGN KEY([LaptopId])
REFERENCES [dbo].[Laptop] ([LaptopId])
GO
ALTER TABLE [dbo].[Slot] CHECK CONSTRAINT [fk_Slot_Laptop]
GO
ALTER TABLE [dbo].[Slot]  WITH CHECK ADD  CONSTRAINT [fk_Slot_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([OrderId])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Slot] CHECK CONSTRAINT [fk_Slot_Order]
GO
ALTER TABLE [dbo].[Ticket]  WITH CHECK ADD  CONSTRAINT [fk_Ticket_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([OrderId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Ticket] CHECK CONSTRAINT [fk_Ticket_Order]
GO
ALTER TABLE [dbo].[Ticket]  WITH CHECK ADD  CONSTRAINT [fk_Ticket_OwnerAccount] FOREIGN KEY([OwnerId])
REFERENCES [dbo].[Account] ([AccountId])
GO
ALTER TABLE [dbo].[Ticket] CHECK CONSTRAINT [fk_Ticket_OwnerAccount]
GO
ALTER TABLE [dbo].[Ticket]  WITH CHECK ADD  CONSTRAINT [fk_Ticket_RenterAccount] FOREIGN KEY([RenterId])
REFERENCES [dbo].[Account] ([AccountId])
GO
ALTER TABLE [dbo].[Ticket] CHECK CONSTRAINT [fk_Ticket_RenterAccount]
GO
ALTER TABLE [dbo].[TicketChatMessage]  WITH CHECK ADD  CONSTRAINT [fk_TicketChatMessage_Sender] FOREIGN KEY([SenderId])
REFERENCES [dbo].[Account] ([AccountId])
GO
ALTER TABLE [dbo].[TicketChatMessage] CHECK CONSTRAINT [fk_TicketChatMessage_Sender]
GO
ALTER TABLE [dbo].[TicketChatMessage]  WITH CHECK ADD  CONSTRAINT [fk_TicketChatMessage_Ticket] FOREIGN KEY([TicketId])
REFERENCES [dbo].[Ticket] ([TicketId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TicketChatMessage] CHECK CONSTRAINT [fk_TicketChatMessage_Ticket]
GO
USE [master]
GO
ALTER DATABASE [LaptopRentalDB] SET  READ_WRITE 
GO
