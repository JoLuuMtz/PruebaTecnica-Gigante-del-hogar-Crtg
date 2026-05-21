import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { CalendarPlus } from 'lucide-react'
import { appointmentsApi } from '@/api'
import { Card, CardHeader } from '@/components/ui/Card'
import { PageHeader } from '@/components/common/PageHeader'
import { Input } from '@/components/ui/Input'
import { Textarea } from '@/components/ui/Textarea'
import { Button } from '@/components/ui/Button'
import { useToast } from '@/hooks/useToast'
import { minAppointmentDate } from '@/utils/format'
import { createAppointmentSchema, type CreateAppointmentFormValues } from '@/utils/validations'

export function CreateAppointmentPage() {
  const navigate = useNavigate()
  const toast = useToast()
  const [loading, setLoading] = useState(false)

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<CreateAppointmentFormValues>({
    resolver: zodResolver(createAppointmentSchema),
    defaultValues: {
      descripcion: '',
      fecha: minAppointmentDate(),
      cuposTotales: 1,
    },
  })

  const onSubmit = async (values: CreateAppointmentFormValues) => {
    setLoading(true)
    try {
      const { data } = await appointmentsApi.create({
        descripcion: values.descripcion,
        fecha: new Date(values.fecha).toISOString(),
        cuposTotales: values.cuposTotales,
      })
      toast.success(data.message || 'Cita creada exitosamente')
      navigate('/appointments')
    } catch (error) {
      toast.error(error, 'No se pudo crear la cita')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div>
      <PageHeader
        title="Crear cita"
        description="Publica una nueva cita con cupos limitados. Requiere rol prestador."
      />

      <Card className="max-w-2xl">
        <CardHeader
          title="Formulario de cita"
          description="La fecha debe ser futura (validación del backend: posterior a mañana UTC)."
        />
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-5">
          <Textarea
            label="Descripción"
            placeholder="Consulta general, evaluación, etc."
            error={errors.descripcion?.message}
            {...register('descripcion')}
          />
          <Input
            label="Fecha y hora"
            type="datetime-local"
            min={minAppointmentDate()}
            error={errors.fecha?.message}
            {...register('fecha')}
          />
          <Input
            label="Cupos totales"
            type="number"
            min={1}
            error={errors.cuposTotales?.message}
            {...register('cuposTotales', { valueAsNumber: true })}
          />
          <div className="flex flex-col gap-3 sm:flex-row">
            <Button type="submit" loading={loading}>
              <CalendarPlus className="h-4 w-4" />
              Crear cita
            </Button>
            <Button type="button" variant="outline" onClick={() => navigate('/appointments')}>
              Cancelar
            </Button>
          </div>
        </form>
      </Card>
    </div>
  )
}
