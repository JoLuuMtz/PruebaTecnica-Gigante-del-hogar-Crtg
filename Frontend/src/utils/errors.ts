import axios, { type AxiosError } from 'axios'
import type { ApiErrorBody } from '@/types/api.types'

export function getErrorMessage(error: unknown): string {
  if (axios.isAxiosError(error)) {
    const axiosError = error as AxiosError<ApiErrorBody>
    const data = axiosError.response?.data

    if (data?.errors?.length) {
      return data.errors.join('. ')
    }

    if (data?.message) {
      return data.message
    }

    if (axiosError.message) {
      return axiosError.message
    }
  }

  if (error instanceof Error) {
    return error.message
  }

  return 'Ocurrió un error inesperado'
}
