<template>
  <div class="admin-view">
    <h1>Panel de Administración</h1>

    <p v-if="!isAdmin" class="error">
      No tienes permisos para acceder a esta sección.
    </p>

    <div v-else>
      <p>Bienvenido, <strong>{{ authen.user?.nombre }}</strong></p>

      <v-row class="mt-4">
        <v-col cols="12" md="4">
          <v-card class="pa-4" @click="router.push({ name: 'productos' })" style="cursor:pointer">
            <v-icon size="36" color="green">mdi-leaf</v-icon>
            <h3 class="mt-2">Gestión de productos</h3>
            <p class="text-body-2">Añadir, editar y eliminar productos del catálogo</p>
          </v-card>
        </v-col>
        <v-col cols="12" md="4">
          <v-card class="pa-4" @click="router.push({ name: 'usuarios' })" style="cursor:pointer">
            <v-icon size="36" color="green">mdi-account-group</v-icon>
            <h3 class="mt-2">Gestión de usuarios</h3>
            <p class="text-body-2">Ver y gestionar los usuarios registrados</p>
          </v-card>
        </v-col>
        <v-col cols="12" md="4">
          <v-card class="pa-4">
            <v-icon size="36" color="green">mdi-cart</v-icon>
            <h3 class="mt-2">Gestión de pedidos</h3>
            <p class="text-body-2">Ver y actualizar el estado de los pedidos</p>
          </v-card>
        </v-col>
      </v-row>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useAuthenStore } from '@/store/authen'
import { useRouter } from 'vue-router'

const authen = useAuthenStore()
const router = useRouter()
const isAdmin = authen.isAdmin
</script>

<style scoped>
.admin-view { padding: 2rem; }
.error { color: red; font-weight: bold; }
</style>