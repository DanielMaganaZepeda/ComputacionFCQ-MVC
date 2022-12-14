USE [master]
GO
/****** Object:  Database [ComputacionFCQ]    Script Date: 10/22/2022 8:21:05 PM ******/
CREATE DATABASE [ComputacionFCQ]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ComputacionFCQ', FILENAME = N'D:\SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\ComputacionFCQ.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ComputacionFCQ_log', FILENAME = N'D:\SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\ComputacionFCQ_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [ComputacionFCQ] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ComputacionFCQ].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ComputacionFCQ] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ComputacionFCQ] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ComputacionFCQ] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ComputacionFCQ] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ComputacionFCQ] SET ARITHABORT OFF 
GO
ALTER DATABASE [ComputacionFCQ] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [ComputacionFCQ] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ComputacionFCQ] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ComputacionFCQ] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ComputacionFCQ] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ComputacionFCQ] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ComputacionFCQ] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ComputacionFCQ] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ComputacionFCQ] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ComputacionFCQ] SET  ENABLE_BROKER 
GO
ALTER DATABASE [ComputacionFCQ] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ComputacionFCQ] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ComputacionFCQ] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ComputacionFCQ] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ComputacionFCQ] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ComputacionFCQ] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ComputacionFCQ] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ComputacionFCQ] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ComputacionFCQ] SET  MULTI_USER 
GO
ALTER DATABASE [ComputacionFCQ] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ComputacionFCQ] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ComputacionFCQ] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ComputacionFCQ] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ComputacionFCQ] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ComputacionFCQ] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [ComputacionFCQ] SET QUERY_STORE = OFF
GO
USE [ComputacionFCQ]
GO
/****** Object:  Table [dbo].[Administrador]    Script Date: 10/22/2022 8:21:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Administrador](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[usuario] [varchar](100) NOT NULL,
	[nombre] [varchar](100) NULL,
	[apellidos] [varchar](100) NULL,
	[contrasena] [varchar](100) NOT NULL,
	[creado_por] [int] NOT NULL,
	[fecha_creacion] [datetime] NULL,
	[activo] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Carrera]    Script Date: 10/22/2022 8:21:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Carrera](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Computadora]    Script Date: 10/22/2022 8:21:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Computadora](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[sala_id] [int] NOT NULL,
	[numero] [int] NOT NULL,
	[funcional] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Fecha]    Script Date: 10/22/2022 8:21:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fecha](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](100) NULL,
	[fecha] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Frecuencia]    Script Date: 10/22/2022 8:21:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Frecuencia](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[curso] [varchar](100) NULL,
	[periodo_inicio] [datetime] NULL,
	[periodo_fin] [datetime] NULL,
 CONSTRAINT [PK_Reservacion] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Programa]    Script Date: 10/22/2022 8:21:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Programa](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](100) NULL,
	[activo] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UC_Nombre] UNIQUE NONCLUSTERED 
(
	[nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProgramaSala]    Script Date: 10/22/2022 8:21:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProgramaSala](
	[sala_id] [int] NOT NULL,
	[programa_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[sala_id] ASC,
	[programa_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reporte]    Script Date: 10/22/2022 8:21:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reporte](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[computadora_id] [int] NOT NULL,
	[detalle] [varchar](100) NOT NULL,
	[fecha_creacion] [datetime] NOT NULL,
	[fecha_solucion] [datetime] NULL,
 CONSTRAINT [PK_Reporte] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reservacion]    Script Date: 10/22/2022 8:21:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reservacion](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[usuario_id] [int] NOT NULL,
	[fecha_inicio] [datetime] NULL,
	[fecha_fin] [datetime] NULL,
	[sala_id] [int] NOT NULL,
	[programa_id] [int] NOT NULL,
	[cantidad_alumnos] [int] NULL,
	[activa] [bit] NULL,
	[curso] [varchar](100) NULL,
	[frecuencia_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sala]    Script Date: 10/22/2022 8:21:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sala](
	[id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sesion]    Script Date: 10/22/2022 8:21:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sesion](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[usuario_id] [int] NOT NULL,
	[fecha_inicio] [datetime] NULL,
	[fecha_fin] [datetime] NULL,
	[computadora_id] [int] NOT NULL,
	[programa_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 10/22/2022 8:21:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[matricula] [varchar](100) NOT NULL,
	[nombre] [varchar](100) NOT NULL,
	[apellidos] [varchar](100) NOT NULL,
	[carrera_id] [int] NOT NULL,
	[correo] [varchar](100) NOT NULL,
	[es_alumno] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[matricula] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Administrador] ADD  CONSTRAINT [datetime]  DEFAULT (getdate()) FOR [fecha_creacion]
GO
ALTER TABLE [dbo].[Administrador] ADD  DEFAULT ((1)) FOR [activo]
GO
ALTER TABLE [dbo].[Computadora] ADD  DEFAULT ((1)) FOR [funcional]
GO
ALTER TABLE [dbo].[Programa] ADD  DEFAULT ((1)) FOR [activo]
GO
ALTER TABLE [dbo].[Reporte] ADD  CONSTRAINT [DF_FechaSolucion]  DEFAULT (NULL) FOR [fecha_solucion]
GO
ALTER TABLE [dbo].[Reservacion] ADD  DEFAULT ((1)) FOR [activa]
GO
ALTER TABLE [dbo].[Sesion] ADD  CONSTRAINT [DF_Sesion]  DEFAULT (getdate()) FOR [fecha_inicio]
GO
ALTER TABLE [dbo].[Usuario] ADD  DEFAULT ((1)) FOR [es_alumno]
GO
ALTER TABLE [dbo].[Administrador]  WITH CHECK ADD FOREIGN KEY([creado_por])
REFERENCES [dbo].[Administrador] ([id])
GO
ALTER TABLE [dbo].[Computadora]  WITH CHECK ADD FOREIGN KEY([sala_id])
REFERENCES [dbo].[Sala] ([id])
GO
ALTER TABLE [dbo].[ProgramaSala]  WITH CHECK ADD FOREIGN KEY([programa_id])
REFERENCES [dbo].[Programa] ([id])
GO
ALTER TABLE [dbo].[ProgramaSala]  WITH CHECK ADD FOREIGN KEY([sala_id])
REFERENCES [dbo].[Sala] ([id])
GO
ALTER TABLE [dbo].[Reporte]  WITH CHECK ADD  CONSTRAINT [FK_Reporte_Computadora] FOREIGN KEY([computadora_id])
REFERENCES [dbo].[Computadora] ([id])
GO
ALTER TABLE [dbo].[Reporte] CHECK CONSTRAINT [FK_Reporte_Computadora]
GO
ALTER TABLE [dbo].[Reservacion]  WITH CHECK ADD FOREIGN KEY([programa_id])
REFERENCES [dbo].[Programa] ([id])
GO
ALTER TABLE [dbo].[Reservacion]  WITH CHECK ADD FOREIGN KEY([sala_id])
REFERENCES [dbo].[Sala] ([id])
GO
ALTER TABLE [dbo].[Reservacion]  WITH CHECK ADD FOREIGN KEY([usuario_id])
REFERENCES [dbo].[Usuario] ([id])
GO
ALTER TABLE [dbo].[Reservacion]  WITH CHECK ADD  CONSTRAINT [FK_Reservacion_Frecuencia] FOREIGN KEY([frecuencia_id])
REFERENCES [dbo].[Frecuencia] ([id])
GO
ALTER TABLE [dbo].[Reservacion] CHECK CONSTRAINT [FK_Reservacion_Frecuencia]
GO
ALTER TABLE [dbo].[Sesion]  WITH CHECK ADD FOREIGN KEY([computadora_id])
REFERENCES [dbo].[Computadora] ([id])
GO
ALTER TABLE [dbo].[Sesion]  WITH CHECK ADD FOREIGN KEY([programa_id])
REFERENCES [dbo].[Programa] ([id])
GO
ALTER TABLE [dbo].[Sesion]  WITH CHECK ADD FOREIGN KEY([usuario_id])
REFERENCES [dbo].[Usuario] ([id])
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD FOREIGN KEY([carrera_id])
REFERENCES [dbo].[Carrera] ([id])
GO
/****** Object:  StoredProcedure [dbo].[SP_ActualizarFechas]    Script Date: 10/22/2022 8:21:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SP_ActualizarFechas]
	@inicio datetime,
	@final datetime
	as
	begin
		update Fecha set fecha = @inicio where id=1
		update Fecha set fecha = @final where id=2
	end
GO
/****** Object:  StoredProcedure [dbo].[SP_ActualizarProgramaSala]    Script Date: 10/22/2022 8:21:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SP_ActualizarProgramaSala]
	@sala_id int,
	@programa_id int
	as
	begin
		if((select count(programa_id) from ProgramaSala where sala_id=@sala_id and programa_id=@programa_id)=1)
		begin
			delete from ProgramaSala where sala_id = @sala_id and programa_id = @programa_id
		end
		else
		begin
			insert into ProgramaSala(sala_id,programa_id) values(@sala_id,@programa_id)
		end
	end
GO
/****** Object:  StoredProcedure [dbo].[SP_AgregarAdministrador]    Script Date: 10/22/2022 8:21:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SP_AgregarAdministrador]
	@usuario varchar(100),
	@nombre varchar(100),
	@apellidos varchar(100),
	@contrasena varchar(100),
	@creado_por int
	as
	begin
		insert into Administrador(usuario,nombre,apellidos,contrasena,creado_por)
		values (@usuario,@nombre,@apellidos,@contrasena,@creado_por)
	end
GO
/****** Object:  StoredProcedure [dbo].[SP_AgregarFrecuencia]    Script Date: 10/22/2022 8:21:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SP_AgregarFrecuencia]
	@curso varchar(100),
	@periodo_inicio datetime,
	@periodo_fin datetime
	as
	begin
		insert into Frecuencia(curso, periodo_inicio, periodo_fin) values(@curso,@periodo_inicio,@periodo_fin)
	end
GO
/****** Object:  StoredProcedure [dbo].[SP_AgregarPrograma]    Script Date: 10/22/2022 8:21:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SP_AgregarPrograma]
	@nombre varchar(100)
	as
	begin
		if((select count(id) from programa where nombre = @nombre)=1)
		begin
			update Programa
			set activo=1 where nombre=@nombre
		end
		else
		begin
			insert into Programa(nombre) values(@nombre)
		end
	end
GO
/****** Object:  StoredProcedure [dbo].[SP_AgregarReporte]    Script Date: 10/22/2022 8:21:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SP_AgregarReporte]
	@sala_id int,
	@numero int,
	@detalle varchar(100)
	as
	begin
		insert into Reporte(computadora_id,detalle,fecha_creacion)
		values ((select top 1 id from computadora where sala_id = @sala_id and numero=@numero),@detalle,getdate())

		update Computadora set funcional = 0 where id = (select top 1 id from computadora where sala_id = @sala_id and numero=@numero)
	end
GO
/****** Object:  StoredProcedure [dbo].[SP_AgregarReservacion]    Script Date: 10/22/2022 8:21:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SP_AgregarReservacion]
	@usuario_id int,
	@fecha_inicio datetime,
	@fecha_fin datetime,
	@sala_id int,
	@programa_id int,
	@cantidad_alumnos int,
	@curso varchar(100),
	@frecuencia_id int
	as
	begin
		insert into Reservacion(usuario_id, fecha_inicio, fecha_fin, sala_id, programa_id, cantidad_alumnos, curso, frecuencia_id)
		values (@usuario_id, @fecha_inicio, @fecha_fin, @sala_id, @programa_id, @cantidad_alumnos, @curso, @frecuencia_id)
	end
GO
/****** Object:  StoredProcedure [dbo].[SP_CerrarSesion]    Script Date: 10/22/2022 8:21:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SP_CerrarSesion]
	@usuario_id varchar(100)
	as
	begin
		update Sesion
		set fecha_fin=GETDATE() where usuario_id=@usuario_id
	end
GO
/****** Object:  StoredProcedure [dbo].[SP_EliminarEvento]    Script Date: 10/22/2022 8:21:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SP_EliminarEvento]
	@id int
	as
	begin
		update Reservacion set activa=0 where id=@id
	end
GO
/****** Object:  StoredProcedure [dbo].[SP_EliminarFrecuencia]    Script Date: 10/22/2022 8:21:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SP_EliminarFrecuencia]
	@frecuencia_id int
	as
	begin
		update Reservacion set activa=0 where frecuencia_id=@frecuencia_id
	end
GO
/****** Object:  StoredProcedure [dbo].[SP_EliminarPrograma]    Script Date: 10/22/2022 8:21:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SP_EliminarPrograma]
	@id int
	as
	begin
		update Programa set activo=0 where id=@id
		delete from ProgramaSala where programa_id=@id
	end
GO
/****** Object:  StoredProcedure [dbo].[SP_IniciarSesion]    Script Date: 10/22/2022 8:21:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SP_IniciarSesion]
	@usuario_id int,
	@computadora_id int,
	@programa_id int
	as
	begin
		insert into Sesion(usuario_id,computadora_id,programa_id)
		values(@usuario_id,@computadora_id,@programa_id)
	end
GO
/****** Object:  StoredProcedure [dbo].[SP_SolucionarReporte]    Script Date: 10/22/2022 8:21:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create Procedure [dbo].[SP_SolucionarReporte]
	@reporte_id int
	as
	begin
		update Reporte set fecha_solucion=getdate() where id=@reporte_id;

		if((select count(id) from reporte where computadora_id = (select computadora_id from reporte where id = @reporte_id) and fecha_solucion is null) = 0)
			update Computadora set funcional=1 where id = (select computadora_id from reporte where id = @reporte_id)
	end
GO
/****** Object:  StoredProcedure [dbo].[SP_Usuario]    Script Date: 10/22/2022 8:21:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SP_Usuario]
	@matricula varchar(100),@nombre varchar(100),@apellidos varchar(100),@carrera_id int,@correo varchar(100),@es_alumno int
	as
	begin
		if((select count(id) from Usuario where matricula=@matricula)=0)
			begin
				insert into Usuario(matricula,nombre,apellidos,carrera_id,correo,es_alumno)
				values (@matricula,@nombre,@apellidos,@carrera_id,@correo,@es_alumno)
			end
		else
			begin
				if((select nombre from Usuario where matricula=@matricula) <> @nombre)
					begin update usuario set nombre = @nombre where matricula=@matricula; end

				if((select apellidos from Usuario where matricula=@matricula) <> @apellidos)
					begin update usuario set apellidos = @apellidos where matricula=@matricula; end

				if((select correo from Usuario where matricula=@matricula) <> @correo)
					begin update usuario set correo = @correo where matricula=@matricula; end

				if((select carrera_id from Usuario where matricula=@matricula) <> @carrera_id)
					begin update usuario set carrera_id = @carrera_id where matricula=@matricula; end

				if((select es_alumno from Usuario where matricula=@matricula) <> @es_alumno)
					begin update usuario set es_alumno = @es_alumno where matricula=@matricula; end
			end
	end
GO
/****** Object:  StoredProcedure [dbo].[SP_ValidarAdministrador]    Script Date: 10/22/2022 8:21:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SP_ValidarAdministrador]
	@usuario varchar(100),
	@contrasena varchar(100)
	as
	begin
		select * from Administrador where usuario=@Usuario and contrasena=@contrasena
	end
GO
USE [master]
GO
ALTER DATABASE [ComputacionFCQ] SET  READ_WRITE 
GO
