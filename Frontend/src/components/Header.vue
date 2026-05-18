<template>
  <v-app-bar app color="green-darken-1" dark>
    <RouterLink to="/" class="d-flex align-center text-decoration-none">
      <v-img
        :src="logoUrl"
        alt="Logo"
        height="35"
        width="35"
        class="mr-2"
        :cover="false"   
      />
      <span class="sr-only">TeaShop</span>
    </RouterLink>

    <v-spacer />

    <v-btn variant="text" :to="'/'">Inicio</v-btn>
    <v-btn variant="text" :to="'/productos'">Productos</v-btn>
    <v-btn variant="text" :to="'/pedidos'">Pedidos</v-btn>
    <v-btn variant="text" v-if="isAdmin" :to="'/usuarios'">Usuarios</v-btn>

    <v-btn variant="text" v-if="!isAuthenticated" :to="'/login'">Login</v-btn>
    <v-btn variant="text" v-else @click="logout">Logout</v-btn>
  </v-app-bar>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthenStore } from '@/store/authen'
import logoUrl from '@/assets/images/logo.png'

const authen = useAuthenStore()
const router = useRouter()

const isAuthenticated = computed(() => authen.isAuthenticated)
const isAdmin = computed(() => authen.user?.rol === 'admin')

function logout() {
  authen.logout()
  router.push('/login')
}
</script>

<style scoped>
.v-img {
  object-fit: contain;
}
.sr-only {
  position: absolute;
  width: 1px;
  height: 1px;
  padding: 0;
  margin: -1px;
  overflow: hidden;
  clip: rect(0, 0, 0, 0); 
  white-space: nowrap;
  border: 0;
}
</style>
