# ОТЧЁТ  
## по учебной практике

---
## Предментая область
**Учёт платежей**

---
### Специальность  
**09.02.07** — *Информационные системы и программирование*  

---

### Профессиональный модуль  
**ПМ.01** — *Разработка модулей программного обеспечения для компьютерных систем*  

---

### Междисциплинарный курс  
**МДК.01.02** — *Поддержка и тестирование программных модулей*  

### Выполнил:
Студент **4 курса**, группа **4ИСИП-122**  
**К.В. Федченко**


### Проверил:
Руководитель практики от Колледжа информатики и программирования  
Преподаватель ВКК, к.п.н. **Т.Г. Аксёнова**

---

## SQL-скрипт базы данных `edchenko_DB_Payment`

```sql
USE [master]
GO
/****** Object:  Database [edchenko_DB_Payment]    Script Date: 27.10.2025 9:17:37 ******/
CREATE DATABASE [edchenko_DB_Payment]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'edchenko_DB_Payment', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER01\MSSQL\DATA\edchenko_DB_Payment.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'edchenko_DB_Payment_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER01\MSSQL\DATA\edchenko_DB_Payment_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [edchenko_DB_Payment] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [edchenko_DB_Payment].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [edchenko_DB_Payment] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [edchenko_DB_Payment] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [edchenko_DB_Payment] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [edchenko_DB_Payment] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [edchenko_DB_Payment] SET ARITHABORT OFF 
GO
ALTER DATABASE [edchenko_DB_Payment] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [edchenko_DB_Payment] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [edchenko_DB_Payment] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [edchenko_DB_Payment] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [edchenko_DB_Payment] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [edchenko_DB_Payment] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [edchenko_DB_Payment] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [edchenko_DB_Payment] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [edchenko_DB_Payment] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [edchenko_DB_Payment] SET  DISABLE_BROKER 
GO
ALTER DATABASE [edchenko_DB_Payment] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [edchenko_DB_Payment] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [edchenko_DB_Payment] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [edchenko_DB_Payment] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [edchenko_DB_Payment] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [edchenko_DB_Payment] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [edchenko_DB_Payment] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [edchenko_DB_Payment] SET RECOVERY FULL 
GO
ALTER DATABASE [edchenko_DB_Payment] SET  MULTI_USER 
GO
ALTER DATABASE [edchenko_DB_Payment] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [edchenko_DB_Payment] SET DB_CHAINING OFF 
GO
ALTER DATABASE [edchenko_DB_Payment] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [edchenko_DB_Payment] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [edchenko_DB_Payment] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [edchenko_DB_Payment] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'edchenko_DB_Payment', N'ON'
GO
ALTER DATABASE [edchenko_DB_Payment] SET QUERY_STORE = ON
GO
ALTER DATABASE [edchenko_DB_Payment] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [edchenko_DB_Payment]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 27.10.2025 9:17:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](max) NOT NULL,
 CONSTRAINT [PK_Category_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payment]    Script Date: 27.10.2025 9:17:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[CategoryID] [int] NOT NULL,
	  NOT NULL,
	[Name] [decimal](18, 0) NOT NULL,
	[Num] [decimal](18, 0) NOT NULL,
	[Price] [decimal](18, 0) NOT NULL,
 CONSTRAINT [PK_Payment] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 27.10.2025 9:17:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Login] [varchar](max) NOT NULL,
	[Password] [varchar](max) NOT NULL,
	  NOT NULL,
	[FIO] [varchar](max) NOT NULL,
	[Photo] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK_Payment_Category] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Category] ([ID])
GO
ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK_Payment_Category]
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK_Payment_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK_Payment_User]
GO
USE [master]
GO
ALTER DATABASE [edchenko_DB_Payment] SET  READ_WRITE 
GO
