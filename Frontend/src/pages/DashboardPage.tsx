import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { CalendarDays, ClipboardList, Shield, Users } from 'lucide-react'
import { appointmentsApi } from '@/api'
import { Card, CardHeader } from '@/components/ui/Card'
import { PageHeader } from '@/components/common/PageHeader'
import { Badge } from '@/components/ui/Badge'
import { useAuth } from '@/hooks/useAuth'
import { useToast } from '@/hooks/useToast'
import type { Appointment } from '@/types/appointment.types'

export function DashboardPage() {
  const { user, isSolicitante, isPrestador } = useAuth()
  const toast = useToast()
  const [appointments, setAppointments] = useState<Appointment[]>([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    const load = async () => {
      try {
        const { data } = await appointmentsApi.getAll()
        setAppointments(data.data ?? [])
      } catch (error) {
        toast.error(error)
      } finally {
        setLoading(false)
      }
    }
    void load()
  }, [toast])

  const availableSlots = appointments.reduce((acc, item) => acc + item.cuposDisponibles, 0)

  const quickLinks = [
    { to: '/appointments', label: 'Ver citas', icon: ClipboardList, show: true },
    { to: '/providers', label: 'Prestadores', icon: Users, show: isSolicitante },
    { to: '/appointments/create', label: 'Crear cita', icon: CalendarDays, show: isPrestador },
    { to: '/roles', label: 'Asignar rol', icon: Shield, show: true },
  ].filter((item) => item.show)

  return (
    <div>
      <PageHeader
        title={`Hola, ${user?.username ?? 'Usuario'}`}
        description="Panel principal del sistema de gestión de citas con cupos limitados."
      />

      <div className="mb-8 grid gap-4 sm:grid-cols-2 xl:grid-cols-4">
        <Card>
          <p className="text-sm text-slate-500">Roles activos</p>
          <div className="mt-3 flex flex-wrap gap-2">
            {user?.roles.map((role) => (
              <Badge key={role} variant="info">
                {role}
              </Badge>
            ))}
          </div>
        </Card>
        <Card>
          <p className="text-sm text-slate-500">Citas activas</p>
          <p className="mt-2 text-3xl font-bold text-slate-900">{loading ? '—' : appointments.length}</p>
        </Card>
        <Card>
          <p className="text-sm text-slate-500">Cupos disponibles</p>
          <p className="mt-2 text-3xl font-bold text-brand-600">{loading ? '—' : availableSlots}</p>
        </Card>
        <Card>
          <p className="text-sm text-slate-500">ID de usuario</p>
          <p className="mt-2 text-3xl font-bold text-slate-900">{user?.userId}</p>
        </Card>
      </div>

      <Card>
        <CardHeader title="Accesos rápidos" description="Navega a las secciones principales del sistema" />
        <div className="grid gap-3 sm:grid-cols-2">
          {quickLinks.map((item) => (
            <Link
              key={item.to}
              to={item.to}
              className="flex items-center gap-3 rounded-xl border border-slate-200 p-4 transition hover:border-brand-300 hover:bg-brand-50/40"
            >
              <span className="flex h-10 w-10 items-center justify-center rounded-lg bg-brand-100 text-brand-700">
                <item.icon className="h-5 w-5" />
              </span>
              <span className="font-medium text-slate-800">{item.label}</span>
            </Link>
          ))}
        </div>
      </Card>
    </div>
  )
}
