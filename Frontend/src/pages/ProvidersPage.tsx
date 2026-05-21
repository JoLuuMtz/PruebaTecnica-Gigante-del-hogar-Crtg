import { useEffect, useMemo, useState } from 'react'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { UserCheck } from 'lucide-react'
import { appointmentsApi, providersApi } from '@/api'
import { Card, CardHeader } from '@/components/ui/Card'
import { PageHeader } from '@/components/common/PageHeader'
import { Input } from '@/components/ui/Input'
import { Button } from '@/components/ui/Button'
import { Badge } from '@/components/ui/Badge'
import { EmptyState } from '@/components/common/EmptyState'
import { LoadingOverlay } from '@/components/common/LoadingOverlay'
import { useToast } from '@/hooks/useToast'
import type { Appointment } from '@/types/appointment.types'
import { subscribeSchema, type SubscribeFormValues } from '@/utils/validations'

export function ProvidersPage() {
  const toast = useToast()
  const [appointments, setAppointments] = useState<Appointment[]>([])
  const [loading, setLoading] = useState(true)
  const [submitting, setSubmitting] = useState(false)

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<SubscribeFormValues>({
    resolver: zodResolver(subscribeSchema),
    defaultValues: { providerId: 1 },
  })

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

  const providers = useMemo(() => {
    const map = new Map<number, number>()
    appointments.forEach((item) => {
      if (item.codUsuarioPrestador) {
        map.set(
          item.codUsuarioPrestador,
          (map.get(item.codUsuarioPrestador) ?? 0) + 1,
        )
      }
    })
    return Array.from(map.entries()).map(([id, count]) => ({ id, count }))
  }, [appointments])

  const onSubmit = async (values: SubscribeFormValues) => {
    setSubmitting(true)
    try {
      const { data } = await providersApi.subscribe(values.providerId)
      toast.success(data.message || 'Suscripción exitosa')
      reset({ providerId: 1 })
    } catch (error) {
      toast.error(error, 'No se pudo suscribir al prestador')
    } finally {
      setSubmitting(false)
    }
  }

  return (
    <div>
      <PageHeader
        title="Prestadores"
        description="Suscríbete a prestadores para poder reservar cupos en sus citas."
      />

      <div className="grid gap-6 lg:grid-cols-2">
        <Card>
          <CardHeader
            title="Suscribirse"
            description="Ingresa el ID del prestador. El backend requiere rol solicitante."
          />
          <form onSubmit={handleSubmit(onSubmit)} className="space-y-5">
            <Input
              label="ID del prestador"
              type="number"
              placeholder="Ej. 2"
              error={errors.providerId?.message}
              {...register('providerId', { valueAsNumber: true })}
            />
            <Button type="submit" loading={submitting}>
              <UserCheck className="h-4 w-4" />
              Suscribirse
            </Button>
          </form>
        </Card>

        <Card>
          <CardHeader
            title="Prestadores detectados"
            description="Listado inferido desde citas activas (CodUsuarioPrestador)."
          />
          {loading ? (
            <LoadingOverlay />
          ) : providers.length === 0 ? (
            <EmptyState
              title="Sin prestadores detectados"
              description="Crea o consulta citas activas para identificar prestadores disponibles."
            />
          ) : (
            <ul className="space-y-3">
              {providers.map((provider) => (
                <li
                  key={provider.id}
                  className="flex items-center justify-between rounded-xl border border-slate-200 px-4 py-3"
                >
                  <div>
                    <p className="font-medium text-slate-900">Prestador #{provider.id}</p>
                    <p className="text-xs text-slate-500">{provider.count} cita(s) activa(s)</p>
                  </div>
                  <Badge variant="info">ID {provider.id}</Badge>
                </li>
              ))}
            </ul>
          )}
        </Card>
      </div>
    </div>
  )
}
