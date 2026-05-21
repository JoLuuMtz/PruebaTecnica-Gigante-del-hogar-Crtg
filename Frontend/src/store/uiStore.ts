import { create } from 'zustand'

interface UiState {
  globalLoading: boolean
  sidebarOpen: boolean
  setGlobalLoading: (value: boolean) => void
  setSidebarOpen: (value: boolean) => void
  toggleSidebar: () => void
}

export const useUiStore = create<UiState>((set) => ({
  globalLoading: false,
  sidebarOpen: false,
  setGlobalLoading: (value) => set({ globalLoading: value }),
  setSidebarOpen: (value) => set({ sidebarOpen: value }),
  toggleSidebar: () => set((state) => ({ sidebarOpen: !state.sidebarOpen })),
}))
