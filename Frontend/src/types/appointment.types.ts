export interface Appointment {
  cod: number
  descripcion: string
  fecha: string
  cuposTotales: number
  cuposDisponibles: number
  codUsuarioPrestador?: number
}

export interface CreateAppointmentRequest {
  descripcion: string
  fecha: string
  cuposTotales: number
}

export interface Reservation {
  codCita: number
  descripcion: string
  fecha: string
  prestador: string
  fechaReserva: string
}

export interface ReserveSlotRequest {
  appointmentId: number
}

export interface AssignRoleRequest {
  userId: number
  roleId: number
}
