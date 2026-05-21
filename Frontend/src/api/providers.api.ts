import { apiClient } from '@/api/client'
import type { ApiResponse } from '@/types/api.types'
import { API_PATHS } from '@/utils/constants'

export const providersApi = {
  subscribe(providerId: number) {
    return apiClient.post<ApiResponse<null>>(API_PATHS.PROVIDERS_SUBSCRIBE, providerId)
  },
}
