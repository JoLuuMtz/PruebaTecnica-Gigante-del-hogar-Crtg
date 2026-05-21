import { Outlet } from 'react-router-dom'
import { Navbar } from '@/components/common/Navbar'
import { Sidebar } from '@/components/common/Sidebar'
import { useUiStore } from '@/store/uiStore'
import { Spinner } from '@/components/ui/Spinner'

export function AppLayout() {
  const globalLoading = useUiStore((s) => s.globalLoading)

  return (
    <div className="min-h-screen bg-slate-50">
      <Navbar />
      <div className="mx-auto flex max-w-7xl">
        <Sidebar />
        <main className="relative min-h-[calc(100vh-4rem)] flex-1 px-4 py-6 sm:px-6 lg:px-8">
          {globalLoading && (
            <div className="absolute inset-0 z-30 flex items-center justify-center bg-white/70 backdrop-blur-[1px]">
              <Spinner size="lg" />
            </div>
          )}
          <Outlet />
        </main>
      </div>
    </div>
  )
}
