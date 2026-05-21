import type { ReactNode } from 'react'
import { Toaster } from 'react-hot-toast'

export function ToastProvider({ children }: { children: ReactNode }) {
  return (
    <>
      {children}
      <Toaster position="top-right" />
    </>
  )
}
