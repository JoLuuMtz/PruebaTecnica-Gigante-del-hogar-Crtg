# Booking System — Frontend

Sistema de gestión de citas médicas enterprise con cupos limitados. Aplicación React moderna que consume una API .NET para proporcionar una experiencia completa de reserva de citas.

---

## ¿Qué es esta aplicación?

Un sistema web para gestionar citas médicas donde:
- **Pacientes** pueden reservar citas con prestadores de salud en horarios disponibles
- **Prestadores** pueden registrarse, configurar sus cupos de atención y ver sus citas
- **Administradores** pueden asignar roles y gestionar usuarios
- El sistema controla **cupos limitados** por horario para evitar sobrevendidos

---

## Stack Tecnológico

| Tecnología | Versión | Propósito |
|-----------|---------|----------|
| **React** | 19 | Framework UI |
| **Vite** | 8 | Build tool y dev server ultrarrápido |
| **TypeScript** | Latest | Tipado estricto para seguridad |
| **Tailwind CSS** | 4 | Estilos y diseño responsive |
| **React Router** | 7 | Navegación y ruteo |
| **Axios** | Latest | Cliente HTTP para API calls |
| **Zustand** | Latest | State management global (ligero y simple) |
| **React Hook Form + Zod** | Latest | Validación de formularios |
| **React Hot Toast** | Latest | Notificaciones emergentes |

---

## Arquitectura General

```
┌─────────────────────────────────────────────────────────────┐
│                    VITE DEV SERVER (5173)                   │
├─────────────────────────────────────────────────────────────┤
│                      React Application                       │
│  ┌──────────────────────────────────────────────────────┐   │
│  │  React Router (AppRoutes)                            │   │
│  │  - ProtectedRoute: Rutas privadas (autenticadas)     │   │
│  │  - GuestRoute: Rutas públicas (login/register)       │   │
│  └──────────────────────────────────────────────────────┘   │
│                           │                                  │
│  ┌────────────┬──────────┴──────────┬────────────────────┐  │
│  │ AuthLayout │   AppLayout (privado) │  Special Pages  │  │
│  │  (login)   │ (Sidebar + Navbar)    │   (errores)     │  │
│  └────────────┴──────────────────────┴────────────────────┘  │
│                           │                                  │
│  ┌──────────────────────────────────────────────────────┐   │
│  │            Zustand State Management                  │   │
│  │  ┌─────────────────┬──────────────────────────────┐  │   │
│  │  │   authStore     │       uiStore               │  │   │
│  │  │ - user          │ - theme, notifications     │  │   │
│  │  │ - token         │ - loading states           │  │   │
│  │  │ - roles         │ - UI preferences           │  │   │
│  │  └─────────────────┴──────────────────────────────┘  │   │
│  └──────────────────────────────────────────────────────┘   │
│                           │                                  │
│  ┌──────────────────────────────────────────────────────┐   │
│  │         API Client (Axios + Interceptores)          │   │
│  │  Automáticamente agrega headers de autenticación     │   │
│  └──────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
                           │ (CORS Proxy)
                  .NET API (5174)
```

---

## Estructura del Proyecto Explicada

```
src/
│   ├── api/                    # Cliente HTTP y endpoints
│   ├── client.ts          # Instancia de Axios con interceptores
│   ├── auth.api.ts        # Login, register, refresh token
│   ├── users.api.ts       # Crear/listar usuarios, asignar roles
│   ├── providers.api.ts   # Gestión de prestadores
│   ├── appointments.api.ts # CRUD de citas
│   ├── slots.api.ts       # Reservas de horarios
│   └── index.ts           # Exporta todas las APIs
│
├── components/            # Componentes reutilizables
│   ├── common/
│   │   ├── Navbar.tsx     # Barra superior con perfil del usuario
│   │   ├── Sidebar.tsx    # Menú lateral con navegación
│   │   ├── PageHeader.tsx # Encabezado de páginas con título
│   │   ├── EmptyState.tsx # Estado vacío (sin datos)
│   │   └── LoadingOverlay.tsx # Pantalla de carga fullscreen
│   └── ui/                # Componentes de diseño
│       ├── Button.tsx     # Botón reutilizable (variants)
│       ├── Card.tsx       # Tarjeta contenedora
│       ├── Input.tsx      # Input de texto
│       ├── Select.tsx     # Selector dropdown
│       ├── Modal.tsx      # Diálogo modal
│       ├── Table.tsx      # Tabla con sorting
│       ├── Textarea.tsx   # Área de texto
│       └── Badge.tsx      # Etiquetas de estado
│
├── contexts/              # Providers globales
│   └── ToastProvider.tsx  # Proveedor de notificaciones toast
│
├── hooks/                 # Custom Hooks reutilizables
│   ├── useAuth.ts         # Acceso al estado y funciones de autenticación
│   ├── useToast.ts        # Disparar notificaciones
│   └── useAsync.ts        # Manejar llamadas async (loading, error, data)
│
├── layouts/               # Layouts principales
│   ├── AuthLayout.tsx     # Para login/register (sin sidebar)
│   └── AppLayout.tsx      # Para app privada (con sidebar + navbar)
│
├── pages/                 # Páginas (8 obligatorias)
│   ├── auth/
│   │   ├── LoginPage.tsx           # Formulario de login
│   │   └── RegisterPage.tsx        # Formulario de registro
│   ├── DashboardPage.tsx           # Página principal (resumen)
│   ├── AppointmentsPage.tsx        # Listar citas del usuario
│   ├── CreateAppointmentPage.tsx   # Crear nueva cita
│   ├── ReservationsPage.tsx        # Ver reservaciones de cupos
│   ├── ProvidersPage.tsx           # Listar prestadores disponibles
│   └── RolesPage.tsx               # Asignar roles (admin)
│
├── routes/                # Ruteo y protección
│   ├── AppRoutes.tsx      # Definición de todas las rutas
│   ├── ProtectedRoute.tsx # Guard para rutas privadas
│   └── GuestRoute.tsx     # Guard para rutas públicas
│
├── services/              # Lógica de negocio
│   └── auth.service.ts    # Funciones de autenticación
│
├── store/                 # Estado global (Zustand)
│   ├── authStore.ts       # State: usuario, token, roles
│   └── uiStore.ts         # State: tema, loading, notificaciones
│
├── styles/                # Estilos globales
│   └── index.css          # Tailwind + estilos personalizados
│
├── types/                 # Tipos TypeScript
│   ├── api.types.ts       # Respuestas de API
│   ├── auth.types.ts      # Tipos de autenticación
│   ├── appointment.types.ts # Tipos de citas
│   └── ... otros tipos
│
├── utils/                 # Utilidades
│   ├── cn.ts              # Helper para clases CSS (classnames)
│   ├── constants.ts       # Constantes globales (rutas, roles, etc)
│   ├── errors.ts          # Manejo de errores
│   ├── format.ts          # Formateo de datos (fechas, moneda, etc)
│   ├── storage.ts         # LocalStorage helpers
│   ├── validations.ts     # Esquemas de validación (Zod)
│   └── ... otros helpers
│
├── App.tsx                # Componente raíz
├── main.tsx               # Punto de entrada
└── vite-env.d.ts          # Tipos de Vite
```

---

## Flujo de Autenticación

```
┌─────────────────────────────────────────────────────────────┐
│                    USUARIO NO AUTENTICADO                   │
└────────────┬────────────────────────────────────────────────┘
             │
      [LoginPage.tsx]
             │
    Ingresa email/contraseña
             │
    POST /api/auth/login
             │
    Respuesta: { token, user: { id, email, roles } }
             │
    ┌────────▼────────────────────────────────────────────┐
    │ authStore.login(token, user)                        │
    │  - Guarda token en localStorage                     │
    │  - Guarda user en Zustand store                     │
    │  - Configura header Authorization en Axios         │
    └────────┬────────────────────────────────────────────┘
             │
    [Redirect a /dashboard]
             │
    USUARIO AUTENTICADO
             │
    ┌────────────────────────────────────────────────────────┐
    │ Cada petición HTTP incluye automáticamente:           │
    │ Header: Authorization: Bearer <token>                 │
    │ (Configurado en api/client.ts interceptor)           │
    └────────────────────────────────────────────────────────┘
             │
    Si token expira:
    POST /api/auth/refresh-token
    Obtiene nuevo token
             │
    Si token inválido:
    [Logout automático]
    Redirect a /login
```

---

## Flujo de Reserva de Cita (Caso de Uso Principal)

```
1. PACIENTE EN HOMEPAGE
   └─> Ve lista de prestadores disponibles (ProvidersPage)

2. PACIENTE SELECCIONA PRESTADOR
   └─> Ve calendario de horarios disponibles (ReservationsPage)
   └─> Cada horario muestra: fecha, hora, cupos disponibles

3. PACIENTE SELECCIONA UN CUPO
   POST /api/slots/reserve
   Body: { slotId, providerId }
   └─> Backend valida que hay cupo disponible
   └─> Backend crea la reserva
   └─> Respuesta: { reservationId, date, time, provider }

4. RESERVA EXITOSA
   Toast notificación: "¡Cita reservada!"
   └─> Cita aparece en AppointmentsPage
   └─> Cupo se decrementa en el calendario

5. PACIENTE VE SUS CITAS
   AppointmentsPage
   └─> GET /api/appointments (citas del usuario autenticado)
   └─> Tabla con: Fecha, Prestador, Estado, Acciones

6. PRESTADOR VE SUS CITAS
   AppointmentsPage (solo prestador)
   └─> GET /api/appointments/provider/{id}
   └─> Ve todos los pacientes reservados en su agenda
```

---

## Roles y Permisos

```
┌──────────────────┬──────────────────┬──────────────────┬──────────────────┐
│   PACIENTE       │   PRESTADOR      │   ADMIN          │  Anónimo         │
├──────────────────┼──────────────────┼──────────────────┼──────────────────┤
│ Sí Ver perfil    │ Sí Ver perfil    │ Sí Todo          │ No Restringido   │
│ Sí Mis citas     │ Sí Mis citas     │ Sí Asignar roles │ Ir Ver login     │
│ Sí Reservar      │ Sí Suscribirse   │ Sí Crear usuarios│ Ir Ver registro  │
│ Sí Ver prestador │ Sí Cupos propios │ Sí Listar todos  │                  │
│ No Crear cupos   │ Sí Crear cupos   │ Sí Eliminar      │                  │
│ No Admin panel   │ No Admin panel   │ Sí Admin panel   │                  │
└──────────────────┴──────────────────┴──────────────────┴──────────────────┘

Asignación de roles:
- Usuario se registra → Recibe rol "PACIENTE" por defecto
- Admin ejecuta assign-role → Usuario puede ser "PRESTADOR" o "ADMIN"
```

---

## Estado Global (Zustand)

### authStore (src/store/authStore.ts)
```typescript
{
  user: null,           // { id, email, roles: ["PACIENTE"] }
  token: null,          // JWT token
  isAuthenticated: false,
  
  // Acciones
  login(token, user),   // Guarda datos en state
  logout(),             // Limpia todo
  setUser(user),        // Actualiza usuario
  refreshToken(token)   // Actualiza JWT
}
```

### uiStore (src/store/uiStore.ts)
```typescript
{
  isDarkMode: false,
  isLoading: false,
  notifications: [],
  
  // Acciones
  setDarkMode(bool),
  setIsLoading(bool),
  addNotification(message),
  removeNotification(id)
}
```

---

## Endpoints Consumidos

| Método | Endpoint | Parámetros | Respuesta | Usado en |
|--------|----------|-----------|----------|----------|
| POST | `/api/auth/register` | `{ email, password }` | `{ user, token }` | RegisterPage |
| POST | `/api/auth/login` | `{ email, password }` | `{ user, token }` | LoginPage |
| POST | `/api/users/assign-role` | `{ userId, role }` | `{ success }` | RolesPage |
| POST | `/api/providers/subscribe` | `{ availableSlots }` | `{ providerId }` | ProvidersPage |
| GET | `/api/providers` | - | `[{ id, name, speciality }]` | ProvidersPage |
| GET | `/api/appointments` | - | `[{ id, date, provider }]` | AppointmentsPage |
| POST | `/api/appointments` | `{ providerId, date, time }` | `{ appointmentId }` | CreateAppointmentPage |
| GET | `/api/appointments/provider/{id}` | `id` | `[{ appointments }]` | AppointmentsPage |
| POST | `/api/slots/reserve` | `{ slotId, providerId }` | `{ reservationId }` | ReservationsPage |
| GET | `/api/slots/my-reservations` | - | `[{ reservations }]` | ReservationsPage |

---

## Cómo Funciona el Dev Server

```
npm install     → Instala dependencias
npm run dev     → Inicia Vite en puerto 5173
                → Hot Module Replacement (cambios en tiempo real)
                → Proxy automático de /api → http://localhost:5174
                → Abre http://localhost:5173 en el navegador
```

### Archivo de configuración (vite.config.ts)
```typescript
// Configura el proxy de API
server: {
  proxy: {
    '/api': {
      target: 'http://localhost:5174',
      changeOrigin: true,
      rewrite: (path) => path // /api/... → http://localhost:5174/api/...
    }
  }
}
```

---

## Componentes Clave Explicados

### ProtectedRoute
Valida que el usuario esté autenticado antes de permitir acceso:
```typescript
<ProtectedRoute>
  <DashboardPage />  // Solo visible si autenticado
</ProtectedRoute>
```

### GuestRoute
Redirige usuarios autenticados al dashboard (evita ver login dos veces):
```typescript
<GuestRoute>
  <LoginPage />  // Solo visible si NO autenticado
</GuestRoute>
```

### LoadingOverlay
Spinner fullscreen mientras se carga algo:
```typescript
const { loading } = useAsync(fetchData);
return <LoadingOverlay isVisible={loading} />;
```

### Modal
Diálogo para confirmaciones:
```typescript
<Modal isOpen={showConfirm} title="¿Confirmar?">
  ¿Deseas reservar esta cita?
  <Button onClick={handleConfirm}>Sí</Button>
</Modal>
```

---

## Flujo Completo: Usuario Nuevo Hasta Reserva

```
1. Usuario anónimo accede http://localhost:5173
   ↓
2. Router lo redirige a /login (LoginPage)
   ↓
3. Usuario hace clic en "Registrarse"
   ↓
4. RegisterPage → POST /api/auth/register
   ↓
5. Backend crea usuario con rol PACIENTE
   ↓
6. Se obtiene token JWT
   ↓
7. authStore.login(token, user)
   ↓
8. Redirect automático a /dashboard (AppLayout + Navbar + Sidebar)
   ↓
9. Usuario hace clic en "Ver Prestadores"
   ↓
10. ProvidersPage → GET /api/providers
    ↓
11. Muestra tabla de prestadores
    ↓
12. Usuario selecciona prestador
    ↓
13. ReservationsPage → GET /api/slots/provider/{id}
    ↓
14. Muestra calendario con horarios disponibles
    ↓
15. Usuario hace clic en un horario
    ↓
16. Modal de confirmación
    ↓
17. POST /api/slots/reserve
    ↓
18. Toast: "¡Cita reservada!"
    ↓
19. Redirect a AppointmentsPage (cita aparece en la lista)
```

---

## Scripts Disponibles

```bash
npm run dev          # Inicia servidor de desarrollo (puerto 5173)
npm run build        # Genera build optimizado para producción
npm run preview      # Previsualiza el build de producción localmente
npm run lint         # Ejecuta ESLint
npm run type-check   # Valida tipos TypeScript
```

---

## Validaciones (Zod)

En `src/utils/validations.ts` se definen esquemas que validan:
- Email válido
- Contraseña con requisitos de seguridad
- Campos requeridos
- Tipos de datos correctos

Se usan con React Hook Form para validación real-time en formularios.

---

## Persistencia

- **LocalStorage**: Token JWT se guarda para mantener sesión entre recargas
- **Zustand**: Estado en memoria (se pierde al recargar, pero se restaura del token)
- **API**: Datos persistentes en SQL Server del backend

---

## Seguridad

Token JWT en Authorization header
Validación de tipos con TypeScript
CORS habilitado (proxy en dev)
Logout automático si token expira
Roles-based access control (RBAC)

---

## Configuración Necesaria

1. Backend corriendo en `http://localhost:5174`
2. Base de datos SQL configurada
3. Variables de entorno (si aplica)

---

## Ejemplo: Cómo Agregar una Nueva Página

1. Crear archivo en `src/pages/NewPage.tsx`
2. Agregar ruta en `src/routes/AppRoutes.tsx`
3. Importar en Sidebar si es accesible
4. Usar hooks (`useAuth`, `useToast`, `useAsync`)
5. Llamar APIs desde `src/api/index.ts`

---

Sistema completo y listo para gestionar citas médicas.
