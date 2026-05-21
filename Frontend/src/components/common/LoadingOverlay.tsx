import { Spinner } from '@/components/ui/Spinner'

export function LoadingOverlay({ label = 'Cargando...' }: { label?: string }) {
  return (
    <div className="flex min-h-[240px] flex-col items-center justify-center gap-4">
      <Spinner size="lg" />
      <p className="text-sm text-slate-500">{label}</p>
    </div>
  )
}
