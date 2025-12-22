create database	DBPruebaLogin

USE DBPruebaLogin

CREATE TABLE Usuario(
ID_Usuario int primary key identity,
NombreUsuario varchar(50),
Correo varchar(50),
Clave varchar(100),)

select * from Usuario