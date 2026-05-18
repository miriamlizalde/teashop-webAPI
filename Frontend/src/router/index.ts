import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/views/HomeView.vue'
import LoginView from '@/views/LoginView.vue'
import RegisterView from '@/views/RegisterView.vue'
import ProductosView from '@/views/ProductosView.vue'
import ProductoDetailView from '@/views/ProductoDetailView.vue'
import UsuariosView from '@/views/UsuariosView.vue'
import UsuarioDetailView from '@/views/UsuarioDetailView.vue'
import AdminView from '@/views/AdminView.vue'
import PedidosView from '@/views/PedidosView.vue'
import PedidoDetailView from '@/views/PedidoDetailView.vue'
import { useAuthenStore } from '@/store/authen'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
      meta: { requiresAuthen: true }
    },
    {
      path: '/productos',
      name: 'productos',
      component: ProductosView,
      meta: { requiresAuthen: false } 
    },
    {
      path: '/productos/:id',
      name: 'producto-detail',
      component: ProductoDetailView,
      meta: { requiresAuthen: false }
    },
    {
      path: '/usuarios',
      name: 'usuarios',
      component: UsuariosView,
      meta: { requiresAuthen: true, requiresAdmin: true }
    },
    {
      path: '/usuarios/:id',
      name: 'usuarioDetail',
      component: UsuarioDetailView,
      meta: { requiresAuthen: true, requiresAdmin: true }
    },
    {
      path: '/admin',
      name: 'admin',
      component: AdminView,
      meta: { requiresAuthen: true, requiresAdmin: true }
    },
    {
      path: '/login',
      name: 'login',
      component: LoginView
    },
    {
      path: '/register',
      name: 'register',
      component: RegisterView
    },
    {
      path: '/pedidos',
      name: 'pedidos',
      component: PedidosView,
      meta: { requiresAuthen: true }
    },
    {
      path: '/pedidos/:id',
      name: 'pedidoDetail',
      component: PedidoDetailView,
      meta: { requiresAuthen: true }
    },
    {
      path: '/:pathMatch(.*)*',
      name: 'notFound',
      component: ProductosView
    }
  ]
})

router.beforeEach((to, _from, next) => {
  const auth = useAuthenStore()
  const publicPages = ['/login', '/register', '/productos']
  const authRequired = !publicPages.includes(to.path)

  if (authRequired && !auth.token) {
    return next('/login')
  }

  next()
})

export default router