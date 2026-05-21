import toast from 'react-hot-toast'
import { getErrorMessage } from '@/utils/errors'

export function useToast() {
  return {
    success: (message: string) => toast.success(message),
    error: (error: unknown, fallback = 'Ocurrió un error') =>
      toast.error(getErrorMessage(error) || fallback),
    info: (message: string) => toast(message),
    loading: (message: string) => toast.loading(message),
    dismiss: toast.dismiss,
  }
}
