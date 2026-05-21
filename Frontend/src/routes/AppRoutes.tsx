import { Navigate, Route, Routes } from 'react-router-dom'
import { AppLayout } from '@/layouts/AppLayout'
import { AuthLayout } from '@/layouts/AuthLayout'
import { ProtectedRoute } from '@/routes/ProtectedRoute'
import { GuestRoute } from '@/routes/GuestRoute'
import { LoginPage } from '@/pages/auth/LoginPage'
import { RegisterPage } from '@/pages/auth/RegisterPage'
import { DashboardPage } from '@/pages/DashboardPage'
import { RolesPage } from '@/pages/RolesPage'
import { ProvidersPage } from '@/pages/ProvidersPage'
import { AppointmentsPage } from '@/pages/AppointmentsPage'
import { CreateAppointmentPage } from '@/pages/CreateAppointmentPage'
import { ReservationsPage } from '@/pages/ReservationsPage'
import { ROLES } from '@/utils/constants'

export function AppRoutes() {
  return (
    <Routes>
      <Route element={<GuestRoute />}>
        <Route element={<AuthLayout />}>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
        </Route>
      </Route>

      <Route element={<ProtectedRoute />}>
        <Route element={<AppLayout />}>
          <Route path="/dashboard" element={<DashboardPage />} />
          <Route path="/roles" element={<RolesPage />} />
          <Route
            path="/providers"
            element={<ProtectedRoute roles={[ROLES.SOLICITANTE]} />}
          >
            <Route index element={<ProvidersPage />} />
          </Route>
          <Route path="/appointments" element={<AppointmentsPage />} />
          <Route
            path="/appointments/create"
            element={<ProtectedRoute roles={[ROLES.PRESTADOR]} />}
          >
            <Route index element={<CreateAppointmentPage />} />
          </Route>
          <Route
            path="/reservations"
            element={<ProtectedRoute roles={[ROLES.SOLICITANTE]} />}
          >
            <Route index element={<ReservationsPage />} />
          </Route>
        </Route>
      </Route>

      <Route path="/" element={<Navigate to="/dashboard" replace />} />
      <Route path="*" element={<Navigate to="/dashboard" replace />} />
    </Routes>
  )
}
