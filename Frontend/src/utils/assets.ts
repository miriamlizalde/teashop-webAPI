const modules = import.meta.glob('@/assets/images/*', { eager: true, as: 'url' }) as Record<string, string>

export function imageUrl(nameOrPath?: string) {
  const base = (nameOrPath ?? '').split('/').pop()
  if (!base) return ''
  const hit = Object.entries(modules).find(([p]) => p.endsWith('/' + base))
  return hit ? hit[1] : ''
}
