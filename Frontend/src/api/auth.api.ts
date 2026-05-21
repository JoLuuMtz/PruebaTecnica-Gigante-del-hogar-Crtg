import { apiClient } from '@/api/client'
import type { ApiResponse } from '@/types/api.types'
import type { AuthResponse, LoginRequest, RegisterRequest } from '@/types/auth.types'
import { API_PATHS } from '@/utils/constants'

export const authApi = {
  register(payload: RegisterRequest) {
    return apiClient.post<ApiResponse<AuthResponse>>(API_PATHS.AUTH_REGISTER, payload)
  },

  login(payload: LoginRequest) {
    return apiClient.post<ApiResponse<AuthResponse>>(API_PATHS.AUTH_LOGIN, payload)
  },
}
