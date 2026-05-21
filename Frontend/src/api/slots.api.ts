import { apiClient } from '@/api/client'
import type { ApiResponse } from '@/types/api.types'
import type { Reservation, ReserveSlotRequest } from '@/types/appointment.types'
import { API_PATHS } from '@/utils/constants'

export const slotsApi = {
  reserve(payload: ReserveSlotRequest) {
    return apiClient.post<ApiResponse<null>>(API_PATHS.SLOTS_RESERVE, payload)
  },

  getMyReservations() {
    return apiClient.get<ApiResponse<Reservation[]>>(API_PATHS.SLOTS_MY_RESERVATIONS)
  },
}
