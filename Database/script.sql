CREATE DATABASE IF NOT EXISTS TiendaDb;
USE TiendaDb;

CREATE TABLE IF NOT EXISTS Productos (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(250) NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Stock INT NOT NULL
);

INSERT INTO Productos (Nombre, Descripcion, Precio, Stock) VALUES
('Laptop', 'Laptop 15 pulgadas', 12999.90, 10),
('Mouse', 'Mouse inalámbrico', 249.50, 50),
('Teclado', 'Teclado mecánico', 899.00, 25);
