import { apiClient } from '@/api/client'
import type { ApiResponse } from '@/types/api.types'
import type { Appointment, CreateAppointmentRequest } from '@/types/appointment.types'
import { API_PATHS } from '@/utils/constants'

export const appointmentsApi = {
  getAll() {
    return apiClient.get<ApiResponse<Appointment[]>>(API_PATHS.APPOINTMENTS)
  },

  getByProvider(providerId: number) {
    return apiClient.get<ApiResponse<Appointment[]>>(
      API_PATHS.APPOINTMENTS_BY_PROVIDER(providerId),
    )
  },

  create(payload: CreateAppointmentRequest) {
    return apiClient.post<ApiResponse<Appointment>>(API_PATHS.APPOINTMENTS, payload)
  },
}
