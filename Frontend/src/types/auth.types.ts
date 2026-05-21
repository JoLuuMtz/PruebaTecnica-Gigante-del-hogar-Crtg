export interface LoginRequest {
  username: string
  password: string
}

export interface RegisterRequest {
  username: string
  password: string
  razonSocial: string
  role: 'solicitante' | 'prestador' | 'ambos'
  especialidad?: string
}

export interface AuthResponse {
  token: string
  userId: number
  username: string
  roles: string[]
}

export interface AuthUser {
  userId: number
  username: string
  roles: string[]
  token: string
}
