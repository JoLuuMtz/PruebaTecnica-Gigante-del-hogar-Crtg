import { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { LogIn } from 'lucide-react'
import { Card } from '@/components/ui/Card'
import { Input } from '@/components/ui/Input'
import { Button } from '@/components/ui/Button'
import { useAuth } from '@/hooks/useAuth'
import { useToast } from '@/hooks/useToast'
import { loginSchema, type LoginFormValues } from '@/utils/validations'

export function LoginPage() {
  const navigate = useNavigate()
  const { login } = useAuth()
  const toast = useToast()
  const [loading, setLoading] = useState(false)

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginFormValues>({
    resolver: zodResolver(loginSchema),
    defaultValues: { username: '', password: '' },
  })

  const onSubmit = async (values: LoginFormValues) => {
    setLoading(true)
    try {
      await login(values)
      toast.success('Sesión iniciada correctamente')
      navigate('/dashboard')
    } catch (error) {
      toast.error(error, 'No se pudo iniciar sesión')
    } finally {
      setLoading(false)
    }
  }

  return (
    <Card padding="lg" className="shadow-xl shadow-slate-200/60">
      <div className="mb-8 text-center lg:text-left">
        <h2 className="text-2xl font-bold text-slate-900">Iniciar sesión</h2>
        <p className="mt-2 text-sm text-slate-500">Accede a tu cuenta para gestionar citas</p>
      </div>

      <form onSubmit={handleSubmit(onSubmit)} className="space-y-5">
        <Input
          label="Usuario"
          placeholder="nombre.usuario"
          autoComplete="username"
          error={errors.username?.message}
          {...register('username')}
        />
        <Input
          label="Contraseña"
          type="password"
          placeholder="••••••••"
          autoComplete="current-password"
          error={errors.password?.message}
          {...register('password')}
        />

        <Button type="submit" fullWidth loading={loading}>
          <LogIn className="h-4 w-4" />
          Entrar
        </Button>
      </form>

      <p className="mt-6 text-center text-sm text-slate-500">
        ¿No tienes cuenta?{' '}
        <Link to="/register" className="font-medium text-brand-600 hover:text-brand-700">
          Regístrate
        </Link>
      </p>
    </Card>
  )
}
