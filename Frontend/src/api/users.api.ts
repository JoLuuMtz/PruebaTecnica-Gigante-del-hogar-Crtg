import { apiClient } from '@/api/client'
import type { ApiResponse } from '@/types/api.types'
import type { AssignRoleRequest } from '@/types/appointment.types'
import { API_PATHS } from '@/utils/constants'

export const usersApi = {
  assignRole(payload: AssignRoleRequest) {
    return apiClient.post<ApiResponse<null>>(API_PATHS.USERS_ASSIGN_ROLE, payload)
  },
}
