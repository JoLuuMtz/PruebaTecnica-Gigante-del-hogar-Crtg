import { useState } from 'react'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { Shield } from 'lucide-react'
import { usersApi } from '@/api'
import { Card, CardHeader } from '@/components/ui/Card'
import { PageHeader } from '@/components/common/PageHeader'
import { Input } from '@/components/ui/Input'
import { Select } from '@/components/ui/Select'
import { Button } from '@/components/ui/Button'
import { useToast } from '@/hooks/useToast'
import { ROLE_IDS } from '@/utils/constants'
import { assignRoleSchema, type AssignRoleFormValues } from '@/utils/validations'

export function RolesPage() {
  const toast = useToast()
  const [loading, setLoading] = useState(false)

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<AssignRoleFormValues>({
    resolver: zodResolver(assignRoleSchema),
    defaultValues: { userId: 1, roleId: ROLE_IDS.SOLICITANTE },
  })

  const onSubmit = async (values: AssignRoleFormValues) => {
    setLoading(true)
    try {
      const { data } = await usersApi.assignRole(values)
      toast.success(data.message || 'Rol asignado exitosamente')
      reset({ userId: 1, roleId: ROLE_IDS.SOLICITANTE })
    } catch (error) {
      toast.error(error, 'No se pudo asignar el rol')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div>
      <PageHeader
        title="Gestión de roles"
        description="Asigna roles adicionales a usuarios existentes mediante la API de administración."
      />

      <Card className="max-w-xl">
        <CardHeader
          title="Asignar rol"
          description="RoleId 1 = solicitante, RoleId 2 = prestador (según seed del backend)."
        />
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-5">
          <Input
            label="ID de usuario"
            type="number"
            placeholder="Ej. 3"
            error={errors.userId?.message}
            {...register('userId', { valueAsNumber: true })}
          />
          <Select
            label="Rol"
            options={[
              { value: ROLE_IDS.SOLICITANTE, label: 'Solicitante (1)' },
              { value: ROLE_IDS.PRESTADOR, label: 'Prestador (2)' },
            ]}
            error={errors.roleId?.message}
            {...register('roleId', { valueAsNumber: true })}
          />
          <Button type="submit" loading={loading}>
            <Shield className="h-4 w-4" />
            Asignar rol
          </Button>
        </form>
      </Card>
    </div>
  )
}
