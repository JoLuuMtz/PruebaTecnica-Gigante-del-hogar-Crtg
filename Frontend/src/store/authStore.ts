import { create } from 'zustand'
import { authApi } from '@/api'
import type { AuthResponse, AuthUser, LoginRequest, RegisterRequest } from '@/types/auth.types'
import { storage } from '@/utils/storage'

interface AuthState {
  user: AuthUser | null
  isAuthenticated: boolean
  isHydrated: boolean
  login: (payload: LoginRequest) => Promise<void>
  register: (payload: RegisterRequest) => Promise<void>
  logout: () => void
  hydrate: () => void
  hasRole: (role: string) => boolean
}

function mapAuthResponse(data: AuthResponse): AuthUser {
  return {
    token: data.token,
    userId: data.userId,
    username: data.username,
    roles: data.roles,
  }
}

function persistSession(user: AuthUser): void {
  storage.setToken(user.token)
  storage.setUser(user)
}

export const useAuthStore = create<AuthState>((set, get) => ({
  user: null,
  isAuthenticated: false,
  isHydrated: false,

  hydrate() {
    const stored = storage.getUser()
    const token = storage.getToken()

    if (stored && token) {
      set({ user: { ...stored, token }, isAuthenticated: true, isHydrated: true })
      return
    }

    storage.clearAuth()
    set({ user: null, isAuthenticated: false, isHydrated: true })
  },

  async login(payload) {
    const { data } = await authApi.login(payload)
    const user = mapAuthResponse(data.data)
    persistSession(user)
    set({ user, isAuthenticated: true })
  },

  async register(payload) {
    const { data } = await authApi.register(payload)
    const user = mapAuthResponse(data.data)
    persistSession(user)
    set({ user, isAuthenticated: true })
  },

  logout() {
    storage.clearAuth()
    set({ user: null, isAuthenticated: false })
  },

  hasRole(role) {
    const roles = get().user?.roles ?? []
    return roles.map((r) => r.toLowerCase()).includes(role.toLowerCase())
  },
}))
