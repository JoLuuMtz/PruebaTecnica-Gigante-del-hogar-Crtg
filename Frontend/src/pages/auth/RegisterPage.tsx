import { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { UserPlus } from 'lucide-react'
import { Card } from '@/components/ui/Card'
import { Input } from '@/components/ui/Input'
import { Select } from '@/components/ui/Select'
import { Button } from '@/components/ui/Button'
import { useAuth } from '@/hooks/useAuth'
import { useToast } from '@/hooks/useToast'
import { REGISTER_ROLES } from '@/utils/constants'
import { registerSchema, type RegisterFormValues } from '@/utils/validations'

export function RegisterPage() {
  const navigate = useNavigate()
  const { register: registerUser } = useAuth()
  const toast = useToast()
  const [loading, setLoading] = useState(false)

  const {
    register,
    handleSubmit,
    watch,
    formState: { errors },
  } = useForm<RegisterFormValues>({
    resolver: zodResolver(registerSchema),
    defaultValues: {
      username: '',
      password: '',
      razonSocial: '',
      role: 'solicitante',
      especialidad: '',
    },
  })

  const selectedRole = watch('role')
  const showEspecialidad = selectedRole === 'prestador' || selectedRole === 'ambos'

  const onSubmit = async (values: RegisterFormValues) => {
    setLoading(true)
    try {
      await registerUser({
        username: values.username,
        password: values.password,
        razonSocial: values.razonSocial,
        role: values.role,
        especialidad: showEspecialidad ? values.especialidad : undefined,
      })
      toast.success('Registro exitoso')
      navigate('/dashboard')
    } catch (error) {
      toast.error(error, 'No se pudo completar el registro')
    } finally {
      setLoading(false)
    }
  }

  return (
    <Card padding="lg" className="shadow-xl shadow-slate-200/60">
      <div className="mb-8 text-center lg:text-left">
        <h2 className="text-2xl font-bold text-slate-900">Crear cuenta</h2>
        <p className="mt-2 text-sm text-slate-500">Regístrate como solicitante o prestador</p>
      </div>

      <form onSubmit={handleSubmit(onSubmit)} className="space-y-5">
        <Input
          label="Usuario"
          placeholder="nombre.usuario"
          error={errors.username?.message}
          {...register('username')}
        />
        <Input
          label="Contraseña"
          type="password"
          placeholder="Mínimo 6 caracteres"
          error={errors.password?.message}
          {...register('password')}
        />
        <Input
          label="Razón social"
          placeholder="Nombre o empresa"
          error={errors.razonSocial?.message}
          {...register('razonSocial')}
        />
        <Select
          label="Rol inicial"
          options={REGISTER_ROLES.map((r) => ({ value: r.value, label: r.label }))}
          error={errors.role?.message}
          {...register('role')}
        />
        {showEspecialidad && (
          <Input
            label="Especialidad"
            placeholder="Ej. Medicina general"
            error={errors.especialidad?.message}
            {...register('especialidad')}
          />
        )}

        <Button type="submit" fullWidth loading={loading}>
          <UserPlus className="h-4 w-4" />
          Registrarse
        </Button>
      </form>

      <p className="mt-6 text-center text-sm text-slate-500">
        ¿Ya tienes cuenta?{' '}
        <Link to="/login" className="font-medium text-brand-600 hover:text-brand-700">
          Inicia sesión
        </Link>
      </p>
    </Card>
  )
}
