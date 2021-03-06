USE [2018_2C_TP]
GO
INSERT [dbo].[EstadoPedido] ([IdEstadoPedido], [Nombre]) VALUES (1, N'Abierto')
INSERT [dbo].[EstadoPedido] ([IdEstadoPedido], [Nombre]) VALUES (2, N'Cerrado')
SET IDENTITY_INSERT [dbo].[Usuario] ON 

select * from EstadoPedido;

INSERT [dbo].[Usuario] ([IdUsuario], [Email], [Password]) VALUES (1, N'pnsanchez@unlam.edu.ar', N'test1234')
INSERT [dbo].[Usuario] ([IdUsuario], [Email], [Password]) VALUES (2, N'pablo_kuko@yahoo.com.ar', N'test1234')
INSERT [dbo].[Usuario] ([IdUsuario], [Email], [Password]) VALUES (3, N'pablokuko@gmail.com', N'test1234')
SET IDENTITY_INSERT [dbo].[Usuario] OFF

select * from Usuario;

SET IDENTITY_INSERT [dbo].[Pedido] ON 
INSERT [dbo].[Pedido] ([IdPedido], [IdUsuarioResponsable], [NombreNegocio], [Descripcion], [IdEstadoPedido], [PrecioUnidad], [PrecioDocena], [FechaCreacion], [FechaModificacion]) VALUES (1, 1, N'La Continental', N'EJEMPLO PEDIDO ABIERTO. Gente, llamo para pedir empanadas, tienen tiempo hasta las 13', 1, 28, 300, CAST(N'2018-09-03 02:42:35.957' AS DateTime), NULL)
INSERT [dbo].[Pedido] ([IdPedido], [IdUsuarioResponsable], [NombreNegocio], [Descripcion], [IdEstadoPedido], [PrecioUnidad], [PrecioDocena], [FechaCreacion], [FechaModificacion]) VALUES (2, 2, N'Solo Empanadas', N'EJEMPLO PEDIDO CERRADO. Gente, llamo para pedir empanadas, tienen tiempo hasta las 13', 2, 20, 220, CAST(N'2018-09-03 02:42:35.957' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Pedido] OFF

select * from Pedido;

INSERT [dbo].[GustoEmpanada] ([IdGustoEmpanada], [Nombre]) VALUES (1, N'Jamon y Queso')
INSERT [dbo].[GustoEmpanada] ([IdGustoEmpanada], [Nombre]) VALUES (2, N'Carne')
INSERT [dbo].[GustoEmpanada] ([IdGustoEmpanada], [Nombre]) VALUES (3, N'Pollo')
INSERT [dbo].[GustoEmpanada] ([IdGustoEmpanada], [Nombre]) VALUES (4, N'Humita')
INSERT [dbo].[GustoEmpanada] ([IdGustoEmpanada], [Nombre]) VALUES (5, N'Verdura')
INSERT [dbo].[GustoEmpanada] ([IdGustoEmpanada], [Nombre]) VALUES (6, N'Queso y Cebolla')

select * from GustoEmpanada;


SET IDENTITY_INSERT [dbo].[InvitacionPedidoGustoEmpanadaUsuario] ON 

INSERT [dbo].[InvitacionPedidoGustoEmpanadaUsuario] ([IdInvitacionPedidoGustoEmpanadaUsuario], [IdPedido], [IdGustoEmpanada], [IdUsuario], [Cantidad]) VALUES (1, 1, 1, 1, 2)
INSERT [dbo].[InvitacionPedidoGustoEmpanadaUsuario] ([IdInvitacionPedidoGustoEmpanadaUsuario], [IdPedido], [IdGustoEmpanada], [IdUsuario], [Cantidad]) VALUES (2, 1, 2, 1, 2)
INSERT [dbo].[InvitacionPedidoGustoEmpanadaUsuario] ([IdInvitacionPedidoGustoEmpanadaUsuario], [IdPedido], [IdGustoEmpanada], [IdUsuario], [Cantidad]) VALUES (3, 1, 4, 2, 2)
INSERT [dbo].[InvitacionPedidoGustoEmpanadaUsuario] ([IdInvitacionPedidoGustoEmpanadaUsuario], [IdPedido], [IdGustoEmpanada], [IdUsuario], [Cantidad]) VALUES (4, 1, 5, 2, 1)
INSERT [dbo].[InvitacionPedidoGustoEmpanadaUsuario] ([IdInvitacionPedidoGustoEmpanadaUsuario], [IdPedido], [IdGustoEmpanada], [IdUsuario], [Cantidad]) VALUES (5, 1, 6, 2, 1)
INSERT [dbo].[InvitacionPedidoGustoEmpanadaUsuario] ([IdInvitacionPedidoGustoEmpanadaUsuario], [IdPedido], [IdGustoEmpanada], [IdUsuario], [Cantidad]) VALUES (6, 2, 1, 1, 2)
INSERT [dbo].[InvitacionPedidoGustoEmpanadaUsuario] ([IdInvitacionPedidoGustoEmpanadaUsuario], [IdPedido], [IdGustoEmpanada], [IdUsuario], [Cantidad]) VALUES (7, 2, 1, 2, 4)
INSERT [dbo].[InvitacionPedidoGustoEmpanadaUsuario] ([IdInvitacionPedidoGustoEmpanadaUsuario], [IdPedido], [IdGustoEmpanada], [IdUsuario], [Cantidad]) VALUES (8, 2, 2, 1, 2)
INSERT [dbo].[InvitacionPedidoGustoEmpanadaUsuario] ([IdInvitacionPedidoGustoEmpanadaUsuario], [IdPedido], [IdGustoEmpanada], [IdUsuario], [Cantidad]) VALUES (9, 2, 4, 3, 4)
SET IDENTITY_INSERT [dbo].[InvitacionPedidoGustoEmpanadaUsuario] OFF

select * from InvitacionPedidoGustoEmpanadaUsuario;


SET IDENTITY_INSERT [dbo].[InvitacionPedido] ON 

INSERT [dbo].[InvitacionPedido] ([IdInvitacionPedido], [IdPedido], [IdUsuario], [Token], [Completado]) VALUES (1, 1, 1, N'767ee897-d6dc-47e3-abb9-4e0309b54d89', 1)
INSERT [dbo].[InvitacionPedido] ([IdInvitacionPedido], [IdPedido], [IdUsuario], [Token], [Completado]) VALUES (2, 1, 2, N'892aa6c4-d677-4c03-871d-cbd8c9f3ff9a', 1)
INSERT [dbo].[InvitacionPedido] ([IdInvitacionPedido], [IdPedido], [IdUsuario], [Token], [Completado]) VALUES (3, 1, 3, N'e0d50af0-4ff4-4cbc-a369-91b035386eb2', 0)
INSERT [dbo].[InvitacionPedido] ([IdInvitacionPedido], [IdPedido], [IdUsuario], [Token], [Completado]) VALUES (4, 2, 1, N'6ab48656-b784-45bc-bf9b-30cb5143a564', 1)
INSERT [dbo].[InvitacionPedido] ([IdInvitacionPedido], [IdPedido], [IdUsuario], [Token], [Completado]) VALUES (5, 2, 2, N'cc53e037-2571-451d-a016-53f6ae06c74f', 1)
INSERT [dbo].[InvitacionPedido] ([IdInvitacionPedido], [IdPedido], [IdUsuario], [Token], [Completado]) VALUES (6, 2, 3, N'1525396e-045f-4212-a448-5d8c867c378d', 1)
SET IDENTITY_INSERT [dbo].[InvitacionPedido] OFF

select * from InvitacionPedido;

INSERT [dbo].[GustoEmpanadaDisponiblePedido] ([IdPedido], [IdGustoEmpanada]) VALUES (1, 1)
INSERT [dbo].[GustoEmpanadaDisponiblePedido] ([IdPedido], [IdGustoEmpanada]) VALUES (1, 2)
INSERT [dbo].[GustoEmpanadaDisponiblePedido] ([IdPedido], [IdGustoEmpanada]) VALUES (1, 3)

select * from GustoEmpanadaDisponiblePedido;