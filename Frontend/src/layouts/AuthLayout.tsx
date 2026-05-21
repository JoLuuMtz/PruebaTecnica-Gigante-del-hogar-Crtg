import { Outlet } from 'react-router-dom'
import { CalendarDays } from 'lucide-react'

export function AuthLayout() {
  return (
    <div className="min-h-screen bg-gradient-to-br from-slate-50 via-white to-brand-50">
      <div className="mx-auto grid min-h-screen max-w-7xl lg:grid-cols-2">
        <section className="hidden flex-col justify-between p-10 lg:flex">
          <div className="flex items-center gap-3 text-brand-700">
            <span className="flex h-11 w-11 items-center justify-center rounded-2xl bg-brand-600 text-white shadow-lg shadow-brand-600/30">
              <CalendarDays className="h-6 w-6" />
            </span>
            <div>
              <p className="text-lg font-bold text-slate-900">Booking System</p>
              <p className="text-sm text-slate-500">Gestión profesional de citas</p>
            </div>
          </div>

          <div>
            <h1 className="max-w-md text-4xl font-bold tracking-tight text-slate-900">
              Reserva cupos con control total
            </h1>
            <p className="mt-4 max-w-lg text-base leading-relaxed text-slate-600">
              Plataforma modular para solicitantes y prestadores. Administra citas, suscripciones y
              reservas con una experiencia moderna y responsive.
            </p>
          </div>

          <p className="text-sm text-slate-400">© {new Date().getFullYear()} Booking System</p>
        </section>

        <section className="flex items-center justify-center px-4 py-10 sm:px-8">
          <div className="w-full max-w-md">
            <Outlet />
          </div>
        </section>
      </div>
    </div>
  )
}
