use gigante;

CREATE TABLE IF NOT EXISTS `roles` (
    `cod` INT NOT NULL AUTO_INCREMENT,
    `descripcion` VARCHAR(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    PRIMARY KEY (`cod`),
    UNIQUE KEY `UQ_roles_descripcion` (`descripcion`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla: usuarios
CREATE TABLE IF NOT EXISTS `usuarios` (
    `cod` INT NOT NULL AUTO_INCREMENT,
    `usuario` VARCHAR(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    `clave` VARCHAR(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    `razon_social` VARCHAR(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    `fecha_creacion` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `activo` TINYINT(1) NOT NULL DEFAULT 1,
    PRIMARY KEY (`cod`),
    UNIQUE KEY `UQ_usuarios_usuario` (`usuario`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla: usuario_rol (relación muchos a muchos)
CREATE TABLE IF NOT EXISTS `usuario_rol` (
    `cod_usuario` INT NOT NULL,
    `cod_rol` INT NOT NULL,
    `fecha_asignacion` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (`cod_usuario`, `cod_rol`),
    KEY `IX_usuario_rol_cod_rol` (`cod_rol`),
    CONSTRAINT `FK_usuario_rol_usuario` FOREIGN KEY (`cod_usuario`) REFERENCES `usuarios` (`cod`) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT `FK_usuario_rol_rol` FOREIGN KEY (`cod_rol`) REFERENCES `roles` (`cod`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla: solicitantes
CREATE TABLE IF NOT EXISTS `solicitantes` (
    `cod_usuario` INT NOT NULL,
    PRIMARY KEY (`cod_usuario`),
    CONSTRAINT `FK_solicitantes_usuario` FOREIGN KEY (`cod_usuario`) REFERENCES `usuarios` (`cod`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla: prestadores
CREATE TABLE IF NOT EXISTS `prestadores` (
    `cod_usuario` INT NOT NULL,
    `especialidad` VARCHAR(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci,
    PRIMARY KEY (`cod_usuario`),
    CONSTRAINT `FK_prestadores_usuario` FOREIGN KEY (`cod_usuario`) REFERENCES `usuarios` (`cod`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla: solicitante_prestador (relación muchos a muchos)
CREATE TABLE IF NOT EXISTS `solicitante_prestador` (
    `cod_usuario_solicitante` INT NOT NULL,
    `cod_usuario_prestador` INT NOT NULL,
    `fecha_suscripcion` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (`cod_usuario_solicitante`, `cod_usuario_prestador`),
    KEY `IX_solicitante_prestador_prestador` (`cod_usuario_prestador`),
    CONSTRAINT `FK_solicitante_prestador_solicitante` FOREIGN KEY (`cod_usuario_solicitante`) REFERENCES `solicitantes` (`cod_usuario`) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT `FK_solicitante_prestador_prestador` FOREIGN KEY (`cod_usuario_prestador`) REFERENCES `prestadores` (`cod_usuario`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla: citas
CREATE TABLE IF NOT EXISTS `citas` (
    `cod` INT NOT NULL AUTO_INCREMENT,
    `descripcion` VARCHAR(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    `fecha` DATETIME NOT NULL,
    `cupos_totales` INT NOT NULL DEFAULT 1,
    `cupos_disponibles` INT NOT NULL DEFAULT 1,
    `cod_usuario_prestador` INT NOT NULL,
    `fecha_creacion` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `activa` TINYINT(1) NOT NULL DEFAULT 1,
    PRIMARY KEY (`cod`),
    KEY `IX_citas_prestador` (`cod_usuario_prestador`),
    KEY `IX_citas_fecha` (`fecha`),
    CONSTRAINT `FK_citas_prestador` FOREIGN KEY (`cod_usuario_prestador`) REFERENCES `prestadores` (`cod_usuario`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla: cupos (reservas de citas)
CREATE TABLE IF NOT EXISTS `cupos` (
    `cod_cita` INT NOT NULL,
    `cod_usuario_solicitante` INT NOT NULL,
    `fecha_reserva` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (`cod_cita`, `cod_usuario_solicitante`),
    KEY `IX_cupos_solicitante` (`cod_usuario_solicitante`),
    CONSTRAINT `FK_cupos_cita` FOREIGN KEY (`cod_cita`) REFERENCES `citas` (`cod`) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT `FK_cupos_solicitante` FOREIGN KEY (`cod_usuario_solicitante`) REFERENCES `solicitantes` (`cod_usuario`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Insertar roles por defecto
INSERT INTO `roles` (`descripcion`) VALUES 
('Administrador'),
('Solicitante'),
('Prestador')
ON DUPLICATE KEY UPDATE `cod` = `cod`;

-- Índices adicionales para optimización
CREATE INDEX `IX_usuarios_activo` ON `usuarios` (`activo`);
CREATE INDEX `IX_citas_activa` ON `citas` (`activa`);
CREATE INDEX `IX_citas_cupos_disponibles` ON `citas` (`cupos_disponibles`);

