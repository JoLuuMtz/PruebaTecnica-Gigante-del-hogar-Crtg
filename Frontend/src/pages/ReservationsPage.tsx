import { useEffect, useState } from 'react'
import { RefreshCw } from 'lucide-react'
import { slotsApi } from '@/api'
import { Card, CardHeader } from '@/components/ui/Card'
import { PageHeader } from '@/components/common/PageHeader'
import { Button } from '@/components/ui/Button'
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
import { useToast } from '@/hooks/useToast'
import { formatDate } from '@/utils/format'
import type { Reservation } from '@/types/appointment.types'

export function ReservationsPage() {
  const toast = useToast()
  const [reservations, setReservations] = useState<Reservation[]>([])
  const [loading, setLoading] = useState(true)

  const load = async () => {
    setLoading(true)
    try {
      const { data } = await slotsApi.getMyReservations()
      setReservations(data.data ?? [])
    } catch (error) {
      toast.error(error)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    void load()
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [])

  return (
    <div>
      <PageHeader
        title="Mis reservas"
        description="Consulta las reservas realizadas como solicitante."
        action={
          <Button variant="outline" onClick={() => void load()}>
            <RefreshCw className="h-4 w-4" />
            Actualizar
          </Button>
        }
      />

      <Card>
        <CardHeader title="Historial de reservas" />

        {loading ? (
          <LoadingOverlay />
        ) : reservations.length === 0 ? (
          <EmptyState
            title="Sin reservas"
            description="Aún no has reservado cupos. Ve a Citas para reservar."
          />
        ) : (
          <Table>
            <TableHead>
              <TableRow>
                <TableHeaderCell>Cita</TableHeaderCell>
                <TableHeaderCell>Descripción</TableHeaderCell>
                <TableHeaderCell>Fecha cita</TableHeaderCell>
                <TableHeaderCell>Prestador</TableHeaderCell>
                <TableHeaderCell>Fecha reserva</TableHeaderCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {reservations.map((item, index) => (
                <TableRow key={`${item.codCita}-${index}`}>
                  <TableCell>#{item.codCita}</TableCell>
                  <TableCell className="font-medium text-slate-900">{item.descripcion}</TableCell>
                  <TableCell>{formatDate(item.fecha)}</TableCell>
                  <TableCell>{item.prestador}</TableCell>
                  <TableCell>{formatDate(item.fechaReserva)}</TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        )}
      </Card>
    </div>
  )
}
