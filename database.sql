USE [planner]
GO
/****** Object:  Table [dbo].[cities]    Script Date: 2/27/2017 4:33:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cities](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[flights]    Script Date: 2/27/2017 4:33:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[flights](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[departure] [varchar](255) NULL,
	[departure_city] [varchar](255) NULL,
	[arrival_city] [varchar](255) NULL,
	[flight_status] [varchar](255) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[flights_cities]    Script Date: 2/27/2017 4:33:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[flights_cities](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[flight_id] [int] NULL,
	[city_id] [int] NULL
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[cities] ON 

INSERT [dbo].[cities] ([id], [name]) VALUES (1, N'Seattle')
INSERT [dbo].[cities] ([id], [name]) VALUES (2, N'Boston')
SET IDENTITY_INSERT [dbo].[cities] OFF
SET IDENTITY_INSERT [dbo].[flights] ON 

INSERT [dbo].[flights] ([id], [departure], [departure_city], [arrival_city], [flight_status]) VALUES (1, N'9 February, 2017', N'Seattle', N'Boston', N'"On time"')
SET IDENTITY_INSERT [dbo].[flights] OFF
SET IDENTITY_INSERT [dbo].[flights_cities] ON 

INSERT [dbo].[flights_cities] ([id], [flight_id], [city_id]) VALUES (1, 1, 1)
INSERT [dbo].[flights_cities] ([id], [flight_id], [city_id]) VALUES (2, 1, 2)
SET IDENTITY_INSERT [dbo].[flights_cities] OFF
