use [master];
go

if not exists (select 1 from sys.sysdatabases where [name] = 'clases') create database control_notas;
go
use control_notas;
go

--Creacion de tablas

create table curso (
	id int identity(1,1) not null,
	id_area int null,
	nombre varchar(50) not null,
	descripcion varchar(100) not null,
	activo bit default 1,
	fecha_registro datetime not null,
	fecha_actualizacion datetime not null,
);

create table area_curso(
	id int identity(1,1) primary key,
	nombre varchar(50) not null,
	activo bit default 1,
	fecha_registro datetime default getdate()
);

create table tarifario_curso(
	id_curso int not null,
	precio_hora money default 0.00,
	porcentaje_descuento decimal(3,2) default 0.00,
	fecha_inicio_precio date not null,
	fecha_fin_precio date null,
	fecha_registro datetime default getdate()
);

create table estudiante (
	id int identity(1,1) not null,
	nombre varchar(50) not null,
	activo bit default 1,
	fecha_registro datetime default getdate()
);


--Llaves primarias
alter table curso
	add constraint pk_curso 
	primary key(id);

alter table area_curso 
	add constraint pk_area_curso 
	primary key(id);

alter table estudiante
	add constraint pk_estudiante
	primary key(id);

--Llaves for�neas
alter table curso 
	add constraint fk_curso_area_curso
	foreign key (id_area) references area_curso(id);

