# Booking System Backend

Un sistema completo de gestión de citas (appointments) desarrollado en **.NET 8** con arquitectura en capas, autenticación JWT y base de datos MySQL.

---

## Índice

1. [Visión General](#visión-general)
2. [Arquitectura](#arquitectura)
3. [Tecnologías](#tecnologías)
4. [Requisitos](#requisitos)
5. [Instalación y Configuración](#instalación-y-configuración)
6. [Cómo Ejecutar](#cómo-ejecutar)
7. [Autenticación y Autorización](#autenticación-y-autorización)
8. [Flujos Principales](#flujos-principales)
9. [Endpoints API](#endpoints-api)
10. [Estructura de Datos](#estructura-de-datos)
11. [Manejo de Errores](#manejo-de-errores)
12. [Resolución de Problemas](#resolución-de-problemas)

---

## Visión General

Este backend es un sistema de reserva de citas que permite:

- **Usuarios** se registren e inicien sesión
- **Proveedores de servicios** (prestadores) crear citas
- **Solicitantes** ver citas disponibles y reservar cupos
- **Gestión de roles** y permisos basados en JWT
- **Validación completa** de datos con FluentValidation
- **Logging detallado** de operaciones con Serilog

### Casos de Uso Principales:

```
1. Registro -> Login -> Obtener Token JWT
2. Prestador crea cita -> Solicitante ve citas -> Solicitante reserva cupo
3. Validación de datos en entrada -> Respuestas estructuradas
```

---

## Arquitectura

El proyecto está dividido en **4 capas principales**:

```
BookingSystem.Api/              <- Presentación (Controllers, Middlewares)
    |
BookingSystem.Application/      <- Lógica de Negocio (Services, DTOs, Validators)
    |
BookingSystem.Domain/           <- Entidades y Contratos (Interfaces, Entities)
    |
BookingSystem.Infrastructure/   <- Acceso a Datos (DbContext, Repositories)

BookingSystem.Shared/           <- Componentes Compartidos (Wrappers de respuesta)
```

### Patrón de Capas:

**API Layer** -> Recibe solicitudes HTTP -> Valida automáticamente con FluentValidation
   |
Application Layer -> Ejecuta lógica de negocio -> Mapea DTOs a entidades
   |
Domain Layer -> Define reglas de negocio -> Interfaces de repositorios
   |
Infrastructure Layer -> Persiste datos en BD -> Ejecuta queries

---

## Tecnologías

| Componente | Versión | Propósito |
|---|---|---|
| **.NET SDK** | 8.0 | Framework principal |
| **Entity Framework Core** | 8.0.5 | ORM para acceso a datos |
| **MySQL** | 5.7+ | Base de datos |
| **JWT (System.IdentityModel.Tokens.Jwt)** | 7.6.0 | Autenticación |
| **FluentValidation** | 11.9.0 | Validación de DTOs |
| **AutoMapper** | 13.0.1 | Mapeo de objetos |
| **BCrypt.Net** | 4.0.3 | Hash de contraseñas |
| **Serilog** | 8.0.1 | Logging estructurado |
| **Swagger/Swagger UI** | 6.6.1 | Documentación de API |

---

## Requisitos

Antes de ejecutar el proyecto, asegúrate de tener:

- **.NET 8 SDK** instalado
- **MySQL Server 5.7 o superior** instalado y ejecutándose
- **Visual Studio Code** o **Visual Studio 2022**
- **dotnet CLI** disponible en la terminal

### Verificar instalación:

```powershell
dotnet --version          # Debe mostrar 8.x.x
mysql --version          # Debe mostrar la versión de MySQL
```

---

## Instalación y Configuración

### 1. Clonar/Descargar el Proyecto

```powershell
cd d:\PruebaTecnica\Backend
```

### 2. Configurar la Conexión a Base de Datos

Edita `appsettings.json` en `BookingSystem.Api/`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=gigante;User=root;Password=tu_contraseña;"
  },
  "JwtSettings": {
    "Secret": "M1S3cr3t0MuyS3gur0Yl4rg0P4raJWT1234567890!"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

**Notas:**
- Reemplaza `tu_contraseña` con tu contraseña de MySQL
- El nombre de la base de datos es `gigante`
- El usuario por defecto es `root`

### 3. Restaurar Dependencias

```powershell
dotnet restore
```

### 4. Crear/Migrar la Base de Datos

El proyecto crea automáticamente la BD en el primer inicio. Si deseas crearla manualmente:

```powershell
# Desde el directorio BookingSystem.Api
dotnet ef database update

# O ejecutar el script SQL
mysql -u root -p < ../../database_script.sql
```

---

## Cómo Ejecutar

### Opción 1: Desde PowerShell

```powershell
# Navega a la carpeta del API
cd .\src\BookingSystem.Api\

# Ejecuta el proyecto
dotnet run
```

### Opción 2: Desde Visual Studio

1. Abre `BookingSystem.slnx`
2. Click derecho en **BookingSystem.Api** → **Set as Startup Project**
3. Presiona `F5` o **Run**

### Opción 3: Usar el setup script

```powershell
# En la raíz del Backend
.\setup.ps1
```

### Resultado Esperado:

```
[14:02:50 INF] Now listening on: http://localhost:5268
[14:02:50 INF] Application started. Press Ctrl+C to shut down.
```

### Acceder a Swagger UI

Una vez ejecutada la aplicación, abre en tu navegador:

```
http://localhost:5268/swagger/index.html
```

---

## Autenticación y Autorización

### Flujo JWT:

```
Usuario envía credenciales (username + password)
      |
AuthService valida credenciales
      |
Se genera Token JWT con Claims (UserId, Username, Role)
      |
Cliente recibe token y lo guarda
      |
Cliente incluye token en header: Authorization: Bearer {token}
      |
Middleware valida token y usuario accede a recursos protegidos
```

### Roles del Sistema:

```
- "solicitante"  -> Puede ver citas y reservar cupos
- "prestador"    -> Puede crear citas y ver sus propias citas
- "admin"        -> Acceso completo (futuro)
```

### Configuración JWT:

**Ubicación:** `Program.cs` (línea ~69)

```csharp
var jwtSecret = builder.Configuration["JwtSettings:Secret"] 
    

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
```

---

## Flujos Principales

### Flujo de Registro (Register)

```
POST /api/auth/register
{
  "username": "usuario@ejemplo.com",
  "clave": "Password123!",
  "razonSocial": "Mi Empresa",
  "rol": "solicitante"
}
    ↓
┌─ Validación ──────────────────────────┐
│ ✓ Username no vacío                   │
│ ✓ Contraseña ≥ 8 caracteres           │
│ ✓ RazonSocial no vacía                │
│ ✓ Usuario no existe previamente       │
└───────────────────────────────────────┘
    ↓
┌─ Creación ────────────────────────────┐
│ 1. Hash de contraseña con BCrypt      │
│ 2. Crear Usuario en BD                │
│ 3. Asignar Rol (solicitante/prestador)│
└───────────────────────────────────────┘
    ↓
Respuesta exitosa con datos del usuario
```

### Flujo de Login

```
POST /api/auth/login
{
  "username": "usuario@ejemplo.com",
  "clave": "Password123!"
}
    ↓
┌─ Validación ──────────────────────────┐
│ ✓ Username y clave no vacíos          │
└───────────────────────────────────────┘
    ↓
┌─ Autenticación ───────────────────────┐
│ 1. Buscar usuario por username        │
│ 2. Verificar contraseña con BCrypt    │
│ 3. Obtener roles del usuario          │
└───────────────────────────────────────┘
    ↓
┌─ Generación de JWT ───────────────────┐
│ 1. Crear claims (UserId, Username, Role)
│ 2. Firmar token con JwtSecret         │
│ 3. Establecer expiración (24 horas)   │
└───────────────────────────────────────┘
    ↓
Respuesta: { token, usuario, rol }
```

### Flujo de Crear Cita (Prestador)

```
POST /api/appointments (Requiere: [Authorize] + Rol=prestador)
{
  "descripcion": "Consultoría de Software",
  "fecha": "2026-05-25T10:00:00",
  "cuposTotales": 5
}
    ↓
┌─ Validación ──────────────────────────┐
│ ✓ Descripción no vacía                │
│ ✓ Fecha en el futuro                  │
│ ✓ Cupos > 0                           │
└───────────────────────────────────────┘
    ↓
┌─ Creación ────────────────────────────┐
│ 1. Crear Cita (appointment)           │
│ 2. Crear Cupos (slots) automáticamente│
│ 3. Guardar en BD                      │
└───────────────────────────────────────┘
    ↓
Respuesta: Cita creada exitosamente
```

### Flujo de Reservar Cupo (Solicitante)

```
POST /api/slots/reserve
{
  "codCita": 1,
  "codSolicitante": 5
}
    ↓
┌─ Validación ──────────────────────────┐
│ ✓ Cita existe y está activa           │
│ ✓ Hay cupos disponibles               │
│ ✓ Solicitante no tiene cita duplicada │
└───────────────────────────────────────┘
    ↓
┌─ Reservación ─────────────────────────┐
│ 1. Actualizar estado cupo a "Ocupado" │
│ 2. Reducir cupos disponibles          │
│ 3. Guardar en BD                      │
└───────────────────────────────────────┘
    ↓
Respuesta: Cupo reservado exitosamente
```

---

## Endpoints API

### **Authentication**

#### Registrarse
```http
POST /api/auth/register
Content-Type: application/json

{
  "username": "nuevo_usuario@email.com",
  "clave": "Password123!",
  "razonSocial": "Mi Organización",
  "rol": "solicitante"
}
```

**Respuesta exitosa (201):**
```json
{
  "success": true,
  "message": "Usuario registrado exitosamente",
  "data": {
    "cod": 1,
    "username": "nuevo_usuario@email.com",
    "razonSocial": "Mi Organización",
    "rol": "solicitante"
  }
}
```

#### Iniciar Sesión
```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "nuevo_usuario@email.com",
  "clave": "Password123!"
}
```

**Respuesta exitosa (200):**
```json
{
  "success": true,
  "message": "Login exitoso",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "usuario": {
      "cod": 1,
      "username": "nuevo_usuario@email.com",
      "razonSocial": "Mi Organización"
    },
    "rol": "solicitante"
  }
}
```

---

### **Appointments (Citas)**

#### Crear Cita (Solo Prestadores)
```http
POST /api/appointments
Authorization: Bearer {token}
Content-Type: application/json

{
  "descripcion": "Asesoría de IT",
  "fecha": "2026-05-28T14:00:00",
  "cuposTotales": 10
}
```

**Headers requeridos:**
- `Authorization: Bearer {token}` - Token JWT del prestador
- Solo usuarios con rol "prestador" pueden crear citas

**Respuesta exitosa (200):**
```json
{
  "success": true,
  "message": "Cita creada exitosamente",
  "data": {
    "cod": 1,
    "descripcion": "Asesoría de IT",
    "fecha": "2026-05-28T14:00:00",
    "cuposTotales": 10
  }
}
```

#### Obtener Todas las Citas Activas
```http
GET /api/appointments
```

**Respuesta (200):**
```json
{
  "success": true,
  "data": [
    {
      "cod": 1,
      "descripcion": "Asesoría de IT",
      "fecha": "2026-05-28T14:00:00",
      "cuposTotales": 10,
      "cuposDisponibles": 8,
      "codUsuarioPrestador": 2
    }
  ]
}
```

#### Obtener Citas de un Prestador
```http
GET /api/appointments/provider/{id}
```

**Parámetros:**
- `id` (path) - ID del prestador

---

### **Slots (Cupos)**

#### Reservar Cupo
```http
POST /api/slots/reserve
Authorization: Bearer {token}
Content-Type: application/json

{
  "codCita": 1,
  "codSolicitante": 5
}
```

**Respuesta exitosa (200):**
```json
{
  "success": true,
  "message": "Cupo reservado exitosamente",
  "data": { "cod": 1, "estado": "Ocupado" }
}
```

---

### **Users (Usuarios)**

#### Obtener Todos los Usuarios
```http
GET /api/users
```

#### Obtener Usuario por ID
```http
GET /api/users/{id}
```

---

### **Providers (Prestadores)**

#### Obtener Todos los Prestadores
```http
GET /api/providers
```

#### Crear Prestador
```http
POST /api/providers
```

---

## Estructura de Datos

### **Entidades Principales**

#### Usuario
```csharp
public class Usuario
{
    public int Cod { get; set; }
    public string Username { get; set; }        // Email o username único
    public string Clave { get; set; }           // Hash de contraseña (BCrypt)
    public string RazonSocial { get; set; }     // Nombre empresa/persona
    public DateTime FechaCreacion { get; set; }
    public bool Activo { get; set; }
    
    // Relaciones
    public ICollection<UsuarioRol> UsuariosRoles { get; set; }
    public Prestador? Prestador { get; set; }
    public Solicitante? Solicitante { get; set; }
}
```

#### Rol
```csharp
public class Rol
{
    public int Cod { get; set; }
    public string Nombre { get; set; }  // "solicitante", "prestador", "admin"
    public ICollection<UsuarioRol> UsuariosRoles { get; set; }
}
```

#### Cita (Appointment)
```csharp
public class Cita
{
    public int Cod { get; set; }
    public string Descripcion { get; set; }
    public DateTime Fecha { get; set; }
    public int CuposTotales { get; set; }
    public int CuposDisponibles { get; set; }
    public bool Activo { get; set; }
    public int CodUsuarioPrestador { get; set; }
    
    // Relación
    public Usuario? UsuarioPrestador { get; set; }
    public ICollection<Cupo>? Cupos { get; set; }
}
```

#### Cupo (Slot)
```csharp
public class Cupo
{
    public int Cod { get; set; }
    public int CodCita { get; set; }
    public int? CodSolicitante { get; set; }
    public string Estado { get; set; }  // "Disponible", "Ocupado", "Cancelado"
    public DateTime FechaReserva { get; set; }
    
    // Relaciones
    public Cita? Cita { get; set; }
    public Solicitante? Solicitante { get; set; }
}
```

#### Prestador (Provider)
```csharp
public class Prestador
{
    public int Cod { get; set; }
    public int CodUsuario { get; set; }
    public string Especialidad { get; set; }
    public DateTime FechaRegistro { get; set; }
    
    // Relación
    public Usuario? Usuario { get; set; }
}
```

#### Solicitante (Requester)
```csharp
public class Solicitante
{
    public int Cod { get; set; }
    public int CodUsuario { get; set; }
    public string Documento { get; set; }
    public DateTime FechaRegistro { get; set; }
    
    // Relación
    public Usuario? Usuario { get; set; }
}
```

### **Diagrama de Relaciones**

```
Usuario (1) --> (M) UsuarioRol <-- (M) Rol
  |
  +-> (1) Prestador
  |     +-> (M) Cita
  |           +-> (M) Cupo
  +-> (1) Solicitante
        +-> (M) Cupo
```

---

## Manejo de Errores

### Estructura de Respuesta de Error

```json
{
  "success": false,
  "message": "Descripción del error",
  "errors": ["Error específico 1", "Error específico 2"]
}
```

### Códigos de Error HTTP

| Código | Significado | Ejemplo |
|---|---|---|
| 400 | Bad Request - Validación fallida | Contraseña muy corta |
| 401 | Unauthorized - No autenticado | Token expirado o ausente |
| 403 | Forbidden - No autorizado | Solicitante intenta crear cita |
| 404 | Not Found - Recurso no existe | Cita no encontrada |
| 500 | Internal Server Error - Error del servidor | Error en BD |

### Ejemplos de Errores

#### Error de Validación (400)
```json
{
  "success": false,
  "message": "Errores de validación",
  "errors": [
    "La contraseña debe tener al menos 8 caracteres",
    "El username es requerido"
  ]
}
```

#### Error de Autorización (403)
```json
{
  "success": false,
  "message": "No tienes permiso para realizar esta acción",
  "errors": ["Solo prestadores pueden crear citas"]
}
```

#### Error de Base de Datos (500)
```json
{
  "success": false,
  "message": "Error interno del servidor",
  "errors": ["No se pudo conectar a la base de datos"]
}
```

---

## Resolución de Problemas

### Error: "Could not load file or assembly 'Microsoft.EntityFrameworkCore'"

**Causa:** Versiones conflictivas de Entity Framework Core

**Solución:**
```powershell
dotnet clean
dotnet restore
dotnet run
```

### Error: "MySQL Connection refused"

**Causa:** MySQL Server no está ejecutándose

**Solución:**
```powershell
# Windows
net start MySQL80  # o la versión que tengas

# Verificar conexión
mysql -u root -p -e "SELECT 1;"
```

### Error: "400 Bad Request" en login/register

**Causas posibles:**
1. Contraseña muy corta (< 8 caracteres)
2. Username vacío
3. Usuario ya existe
4. Validaciones no cumplidas

**Solución:** Verifica los `errors` en la respuesta JSON para detalles específicos

### Error: "401 Unauthorized"

**Causas:**
1. Token JWT expirado
2. Token no incluido en headers
3. Token inválido

**Solución:**
```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Error: "403 Forbidden - Solo prestadores pueden crear citas"

**Causa:** Usuario con rol "solicitante" intenta crear cita

**Solución:** Registrarse con `"rol": "prestador"`

### Error: "Database 'gigante' already exists"

**Solución:**
```powershell
dotnet ef database drop --force
dotnet ef database update
```

---

## Logging

Los logs se guardan en: `BookingSystem.Api/logs/log-YYYYMMDD.txt`

### Niveles de Log:
- **Information** - Operaciones normales
- **Warning** - Algo anómalo (validación fallida, etc)
- **Error** - Errores que no impiden ejecución
- **Fatal** - Errores críticos

### Ejemplo de Log:
```
[14:02:49 INF] Executed DbCommand (26ms)
SELECT CASE WHEN COUNT(*) = 0 THEN FALSE ELSE TRUE END
FROM information_schema.tables
WHERE table_type = 'BASE TABLE' AND table_schema = 'gigante'

[14:02:50 INF] Now listening on: http://localhost:5268
```

---

## Configuración Avanzada

### Cambiar Puerto Predeterminado

Edita `Properties/launchSettings.json`:

```json
{
  "profiles": {
    "BookingSystem.Api": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": false,
      "applicationUrl": "http://localhost:5268;https://localhost:7123",
      ...
    }
  }
}
```

Cambia `5268` al puerto deseado.

### Cambiar JwtSettings

Edita `appsettings.json`:

```json
{
  "JwtSettings": {
    "Secret": "TuNuevaClaveSecretaMasSegura1234567890!",
    "Issuer": "https://tudominio.com",
    "Audience": "TuAppCliente",
    "ExpirationInHours": 24
  }
}
```

---

## Recursos Adicionales

- [Documentación .NET 8](https://learn.microsoft.com/en-us/dotnet/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [JWT en ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/jwt)
- [FluentValidation](https://docs.fluentvalidation.net/)
- [Swagger/OpenAPI](https://swagger.io/)

---

## Soporte

Si encuentras problemas:

1. Revisa los logs en `logs/log-*.txt`
2. Verifica la conexión a MySQL
3. Asegúrate que todas las validaciones se cumplan
4. Consulta la sección de [Resolución de Problemas](#resolución-de-problemas)

---

**Última actualización:** Mayo 21, 2026  
**Versión:** 1.0.0
