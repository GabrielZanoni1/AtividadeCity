create database dbProjetoCity;
use dbProjetoCity;

create table tbUsuario(
Id int not null,
Nome varchar (50) not null,
Email varchar (50) not null,
Senha varchar (20) not null
);

create table tbProdutos(
Id int not null,
Nome varchar (50) not null,
Descricao varchar (255) not null,
Preco decimal not null,
Quantidade int not null
);

SELECT * FROM tbUsuario;
SELECT * FROM tbProdutos;