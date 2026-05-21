import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { CalendarPlus, RefreshCw } from 'lucide-react'
import { appointmentsApi, slotsApi } from '@/api'
import { Card, CardHeader } from '@/components/ui/Card'
import { PageHeader } from '@/components/common/PageHeader'
import { Button } from '@/components/ui/Button'
import { Badge } from '@/components/ui/Badge'
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeaderCell,
  TableRow,
} from '@/components/ui/Table'
import { EmptyState } from '@/components/common/EmptyState'
import { LoadingOverlay } from '@/components/common/LoadingOverlay'
import { Modal } from '@/components/ui/Modal'
import { useAuth } from '@/hooks/useAuth'
import { useToast } from '@/hooks/useToast'
import { formatDate } from '@/utils/format'
import type { Appointment } from '@/types/appointment.types'

export function AppointmentsPage() {
  const { isSolicitante, isPrestador, user } = useAuth()
  const toast = useToast()
  const [appointments, setAppointments] = useState<Appointment[]>([])
  const [loading, setLoading] = useState(true)
  const [selected, setSelected] = useState<Appointment | null>(null)
  const [submitting, setSubmitting] = useState(false)

  const loadAppointments = async () => {
    setLoading(true)
    try {
      if (isPrestador && user?.userId) {
        const { data } = await appointmentsApi.getByProvider(user.userId)
        setAppointments(data.data ?? [])
      } else {
        const { data } = await appointmentsApi.getAll()
        setAppointments(data.data ?? [])
      }
    } catch (error) {
      toast.error(error)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    void loadAppointments()
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [isPrestador, user?.userId])

  const handleReserve = async () => {
    if (!selected) return
    setSubmitting(true)
    try {
      const { data } = await slotsApi.reserve({ appointmentId: selected.cod })
      toast.success(data.message || 'Reserva realizada')
      setSelected(null)
      await loadAppointments()
    } catch (error) {
      toast.error(error, 'No se pudo reservar el cupo')
    } finally {
      setSubmitting(false)
    }
  }

  return (
    <div>
      <PageHeader
        title="Citas"
        description={
          isPrestador
            ? 'Listado de citas creadas por ti como prestador.'
            : 'Consulta citas activas y reserva cupos disponibles.'
        }
        action={
          <div className="flex gap-2">
            <Button variant="outline" onClick={() => void loadAppointments()}>
              <RefreshCw className="h-4 w-4" />
              Actualizar
            </Button>
            {isPrestador && (
              <Link to="/appointments/create">
                <Button>
                  <CalendarPlus className="h-4 w-4" />
                  Nueva cita
                </Button>
              </Link>
            )}
          </div>
        }
      />

      <Card>
        <CardHeader title="Listado de citas" description="Citas activas con cupos disponibles" />

        {loading ? (
          <LoadingOverlay />
        ) : appointments.length === 0 ? (
          <EmptyState
            title="No hay citas"
            description="Aún no existen citas activas para mostrar."
            action={
              isPrestador ? (
                <Link to="/appointments/create">
                  <Button>Crear primera cita</Button>
                </Link>
              ) : undefined
            }
          />
        ) : (
          <Table>
            <TableHead>
              <TableRow>
                <TableHeaderCell>ID</TableHeaderCell>
                <TableHeaderCell>Descripción</TableHeaderCell>
                <TableHeaderCell>Fecha</TableHeaderCell>
                <TableHeaderCell>Cupos</TableHeaderCell>
                <TableHeaderCell>Prestador</TableHeaderCell>
                {isSolicitante && <TableHeaderCell>Acción</TableHeaderCell>}
              </TableRow>
            </TableHead>
            <TableBody>
              {appointments.map((appointment) => (
                <TableRow key={appointment.cod}>
                  <TableCell>#{appointment.cod}</TableCell>
                  <TableCell className="font-medium text-slate-900">
                    {appointment.descripcion}
                  </TableCell>
                  <TableCell>{formatDate(appointment.fecha)}</TableCell>
                  <TableCell>
                    <Badge variant={appointment.cuposDisponibles > 0 ? 'success' : 'danger'}>
                      {appointment.cuposDisponibles}/{appointment.cuposTotales}
                    </Badge>
                  </TableCell>
                  <TableCell>
                    {appointment.codUsuarioPrestador
                      ? `#${appointment.codUsuarioPrestador}`
                      : '—'}
                  </TableCell>
                  {isSolicitante && (
                    <TableCell>
                      <Button
                        size="sm"
                        disabled={appointment.cuposDisponibles <= 0}
                        onClick={() => setSelected(appointment)}
                      >
                        Reservar
                      </Button>
                    </TableCell>
                  )}
                </TableRow>
              ))}
            </TableBody>
          </Table>
        )}
      </Card>

      <Modal
        open={!!selected}
        title="Confirmar reserva"
        onClose={() => setSelected(null)}
        footer={
          <>
            <Button variant="outline" onClick={() => setSelected(null)}>
              Cancelar
            </Button>
            <Button loading={submitting} onClick={() => void handleReserve()}>
              Confirmar reserva
            </Button>
          </>
        }
      >
        {selected && (
          <div className="space-y-2 text-sm text-slate-600">
            <p>
              <span className="font-medium text-slate-800">Cita:</span> {selected.descripcion}
            </p>
            <p>
              <span className="font-medium text-slate-800">Fecha:</span> {formatDate(selected.fecha)}
            </p>
            <p>
              <span className="font-medium text-slate-800">Cupos disponibles:</span>{' '}
              {selected.cuposDisponibles}
            </p>
          </div>
        )}
      </Modal>
    </div>
  )
}
