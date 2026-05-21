export const STORAGE_KEYS = {
  TOKEN: 'booking_token',
  USER: 'booking_user',
} as const

export const ROLES = {
  SOLICITANTE: 'solicitante',
  PRESTADOR: 'prestador',
} as const

export const ROLE_IDS = {
  SOLICITANTE: 1,
  PRESTADOR: 2,
} as const

export const REGISTER_ROLES = [
  { value: 'solicitante', label: 'Solicitante' },
  { value: 'prestador', label: 'Prestador' },
  { value: 'ambos', label: 'Ambos' },
] as const

export const API_PATHS = {
  AUTH_REGISTER: '/api/auth/register',
  AUTH_LOGIN: '/api/auth/login',
  USERS_ASSIGN_ROLE: '/api/users/assign-role',
  PROVIDERS_SUBSCRIBE: '/api/providers/subscribe',
  APPOINTMENTS: '/api/appointments',
  APPOINTMENTS_BY_PROVIDER: (id: number) => `/api/appointments/provider/${id}`,
  SLOTS_RESERVE: '/api/slots/reserve',
  SLOTS_MY_RESERVATIONS: '/api/slots/my-reservations',
} as const
