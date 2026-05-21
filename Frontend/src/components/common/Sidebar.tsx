import type { ComponentType } from 'react'
import { NavLink } from 'react-router-dom'
import {
  CalendarPlus,
  CalendarRange,
  ClipboardList,
  LayoutDashboard,
  Shield,
  UserPlus,
  Users,
  X,
} from 'lucide-react'
import { cn } from '@/utils/cn'
import { useAuth } from '@/hooks/useAuth'
import { useUiStore } from '@/store/uiStore'
import { Button } from '@/components/ui/Button'

interface NavItem {
  to: string
  label: string
  icon: ComponentType<{ className?: string }>
  roles?: string[]
}

const navItems: NavItem[] = [
  { to: '/dashboard', label: 'Dashboard', icon: LayoutDashboard },
  { to: '/roles', label: 'Roles', icon: Shield },
  { to: '/providers', label: 'Prestadores', icon: Users, roles: ['solicitante'] },
  { to: '/appointments', label: 'Citas', icon: ClipboardList },
  {
    to: '/appointments/create',
    label: 'Crear cita',
    icon: CalendarPlus,
    roles: ['prestador'],
  },
  { to: '/reservations', label: 'Mis reservas', icon: CalendarRange, roles: ['solicitante'] },
]

export function Sidebar() {
  const { hasRole } = useAuth()
  const sidebarOpen = useUiStore((s) => s.sidebarOpen)
  const setSidebarOpen = useUiStore((s) => s.setSidebarOpen)

  const visibleItems = navItems.filter((item) => {
    if (!item.roles?.length) return true
    return item.roles.some((role) => hasRole(role))
  })

  return (
    <>
      {sidebarOpen && (
        <button
          type="button"
          className="fixed inset-0 z-40 bg-slate-900/40 lg:hidden"
          aria-label="Cerrar menú"
          onClick={() => setSidebarOpen(false)}
        />
      )}

      <aside
        className={cn(
          'fixed inset-y-0 left-0 z-50 w-72 border-r border-slate-200 bg-white pt-16 transition-transform lg:static lg:translate-x-0 lg:pt-0',
          sidebarOpen ? 'translate-x-0' : '-translate-x-full lg:translate-x-0',
        )}
      >
        <div className="flex items-center justify-between px-4 py-3 lg:hidden">
          <span className="text-sm font-semibold text-slate-700">Menú</span>
          <Button variant="ghost" size="sm" onClick={() => setSidebarOpen(false)}>
            <X className="h-4 w-4" />
          </Button>
        </div>

        <nav className="space-y-1 px-3 py-4">
          {visibleItems.map((item) => (
            <NavLink
              key={item.to}
              to={item.to}
              onClick={() => setSidebarOpen(false)}
              className={({ isActive }) =>
                cn(
                  'flex items-center gap-3 rounded-xl px-3 py-2.5 text-sm font-medium transition',
                  isActive
                    ? 'bg-brand-50 text-brand-700'
                    : 'text-slate-600 hover:bg-slate-100 hover:text-slate-900',
                )
              }
            >
              <item.icon className="h-4 w-4" />
              {item.label}
            </NavLink>
          ))}
        </nav>

        <div className="mx-3 mt-6 rounded-xl border border-dashed border-slate-200 bg-slate-50 p-4">
          <div className="flex items-center gap-2 text-sm font-medium text-slate-700">
            <UserPlus className="h-4 w-4 text-brand-600" />
            Gestión de citas
          </div>
          <p className="mt-2 text-xs leading-relaxed text-slate-500">
            Sistema de reservas con cupos limitados entre solicitantes y prestadores.
          </p>
        </div>
      </aside>
    </>
  )
}
