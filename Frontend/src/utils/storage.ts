import type { AuthUser } from '@/types/auth.types'
import { STORAGE_KEYS } from '@/utils/constants'

export const storage = {
  getToken(): string | null {
    return localStorage.getItem(STORAGE_KEYS.TOKEN)
  },

  setToken(token: string): void {
    localStorage.setItem(STORAGE_KEYS.TOKEN, token)
  },

  removeToken(): void {
    localStorage.removeItem(STORAGE_KEYS.TOKEN)
  },

  getUser(): AuthUser | null {
    const raw = localStorage.getItem(STORAGE_KEYS.USER)
    if (!raw) return null
    try {
      return JSON.parse(raw) as AuthUser
    } catch {
      return null
    }
  },

  setUser(user: AuthUser): void {
    localStorage.setItem(STORAGE_KEYS.USER, JSON.stringify(user))
  },

  removeUser(): void {
    localStorage.removeItem(STORAGE_KEYS.USER)
  },

  clearAuth(): void {
    this.removeToken()
    this.removeUser()
  },
}
