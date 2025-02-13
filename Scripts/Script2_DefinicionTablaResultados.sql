USE [PruebaApiBCCR]
GO

CREATE TABLE [Resultados] (
	[Id] int IDENTITY(1,1),
	[FechaConsulta] DATETIME,
	[TipoCambioCompra] VARCHAR(20),
	[TipoCambioVenta] VARCHAR(20),

	CONSTRAINT [PK_Resultados] PRIMARY KEY (Id)
);
GO

