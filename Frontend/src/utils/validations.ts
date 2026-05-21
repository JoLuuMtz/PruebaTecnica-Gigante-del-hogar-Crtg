import { z } from 'zod'

export const loginSchema = z.object({
  username: z.string().min(1, 'El usuario es obligatorio'),
  password: z.string().min(6, 'La contraseña debe tener al menos 6 caracteres'),
})

export const registerSchema = z
  .object({
    username: z.string().min(1, 'El usuario es obligatorio').max(100),
    password: z.string().min(6, 'La contraseña debe tener al menos 6 caracteres'),
    razonSocial: z.string().min(1, 'La razón social es obligatoria').max(200),
    role: z.enum(['solicitante', 'prestador', 'ambos']),
    especialidad: z.string().optional(),
  })
  .superRefine((data, ctx) => {
    if (
      (data.role === 'prestador' || data.role === 'ambos') &&
      !data.especialidad?.trim()
    ) {
      ctx.addIssue({
        code: 'custom',
        message: 'La especialidad es obligatoria para prestadores',
        path: ['especialidad'],
      })
    }
  })

export const assignRoleSchema = z.object({
  userId: z.number().int().positive('ID de usuario inválido'),
  roleId: z.number().int().positive('ID de rol inválido'),
})

export const subscribeSchema = z.object({
  providerId: z.number().int().positive('ID de prestador inválido'),
})

export const createAppointmentSchema = z.object({
  descripcion: z.string().min(1, 'La descripción es obligatoria').max(255),
  fecha: z.string().min(1, 'La fecha es obligatoria'),
  cuposTotales: z.number().int().positive('Los cupos deben ser mayores a 0'),
})

export const reserveSchema = z.object({
  appointmentId: z.number().int().positive('Seleccione una cita válida'),
})

export type LoginFormValues = z.infer<typeof loginSchema>
export type RegisterFormValues = z.infer<typeof registerSchema>
export type AssignRoleFormValues = z.infer<typeof assignRoleSchema>
export type SubscribeFormValues = z.infer<typeof subscribeSchema>
export type CreateAppointmentFormValues = z.infer<typeof createAppointmentSchema>
