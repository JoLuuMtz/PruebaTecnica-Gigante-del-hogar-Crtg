export interface ApiResponse<T> {
  success: boolean
  message: string
  data: T
  errors?: string[]
}

export interface ApiErrorBody {
  success: false
  message: string
  errors?: string[]
}
