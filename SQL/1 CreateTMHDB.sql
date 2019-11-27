USE [master]
GO

/****** Object:  Database [TakeMeHome]    Script Date: 1/7/2019 9:16:12 AM ******/
CREATE DATABASE [TakeMeHome]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TakeMeHome', FILENAME = N'E:\SQLDB\TakeMeHome.mdf' , SIZE = 307200KB , MAXSIZE = UNLIMITED, FILEGROWTH = 102400KB )
 LOG ON 
( NAME = N'TakeMeHome_log', FILENAME = N'D:\SQL_Logs\TakeMeHome_log.ldf' , SIZE = 20480KB , MAXSIZE = 2048000KB , FILEGROWTH = 20480KB )
GO

ALTER DATABASE [TakeMeHome] SET COMPATIBILITY_LEVEL = 90
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TakeMeHome].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO

ALTER DATABASE [TakeMeHome] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [TakeMeHome] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [TakeMeHome] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [TakeMeHome] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [TakeMeHome] SET ARITHABORT OFF 
GO

ALTER DATABASE [TakeMeHome] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [TakeMeHome] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [TakeMeHome] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [TakeMeHome] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [TakeMeHome] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [TakeMeHome] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [TakeMeHome] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [TakeMeHome] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [TakeMeHome] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [TakeMeHome] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [TakeMeHome] SET  DISABLE_BROKER 
GO

ALTER DATABASE [TakeMeHome] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [TakeMeHome] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [TakeMeHome] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [TakeMeHome] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [TakeMeHome] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [TakeMeHome] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [TakeMeHome] SET PARTNER TIMEOUT 10 
GO

ALTER DATABASE [TakeMeHome] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [TakeMeHome] SET RECOVERY FULL 
GO

ALTER DATABASE [TakeMeHome] SET  MULTI_USER 
GO

ALTER DATABASE [TakeMeHome] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [TakeMeHome] SET DB_CHAINING OFF 
GO

ALTER DATABASE [TakeMeHome] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [TakeMeHome] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [TakeMeHome] SET  READ_WRITE 
GO


