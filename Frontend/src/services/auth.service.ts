import { authApi } from '@/api'
import type { LoginRequest, RegisterRequest } from '@/types/auth.types'

export const authService = {
  login: (payload: LoginRequest) => authApi.login(payload),
  register: (payload: RegisterRequest) => authApi.register(payload),
}
