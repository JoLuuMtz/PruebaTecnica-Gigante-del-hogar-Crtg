import { useAuthStore } from '@/store/authStore'
import { ROLES } from '@/utils/constants'

export function useAuth() {
  const user = useAuthStore((s) => s.user)
  const isAuthenticated = useAuthStore((s) => s.isAuthenticated)
  const isHydrated = useAuthStore((s) => s.isHydrated)
  const login = useAuthStore((s) => s.login)
  const register = useAuthStore((s) => s.register)
  const logout = useAuthStore((s) => s.logout)
  const hasRole = useAuthStore((s) => s.hasRole)

  return {
    user,
    isAuthenticated,
    isHydrated,
    login,
    register,
    logout,
    hasRole,
    isSolicitante: hasRole(ROLES.SOLICITANTE),
    isPrestador: hasRole(ROLES.PRESTADOR),
  }
}
