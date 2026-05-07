<template>
  <v-container class="py-6">
    <div v-if="loading">Cargando datos del usuario...</div>
    <div v-else-if="error">
      <p class="error">{{ error }}</p>
      <v-btn @click="router.back()" color="secondary">Volver</v-btn>
    </div>

    <v-card v-else-if="usuario" class="pa-4">
      <v-card-title class="text-h5">{{ usuario.nombre }}</v-card-title>
      <v-card-text>
        <p><strong>ID:</strong> {{ usuario.usuarioId }}</p>
        <p><strong>Email:</strong> {{ usuario.email }}</p>
        <p><strong>Rol:</strong> {{ usuario.rol }}</p>
        <p><strong>Saldo:</strong> {{ usuario.saldo }}€</p>
        <v-chip :color="usuario.esSocio ? 'green' : 'grey'" class="mt-2">
          {{ usuario.esSocio ? 'Socio' : 'No socio' }}
        </v-chip>
      </v-card-text>

      <v-card-text>
        <h3 class="mb-2">Añadir saldo</h3>
        <div class="d-flex gap-3 align-center">
          <v-text-field
            label="Cantidad (€)"
            type="number"
            v-model.number="cantSaldo"
            style="max-width: 150px;"
          />
          <v-btn color="green-darken-1" @click="addSaldo" :loading="añadiendo">Añadir</v-btn>
        </div>
      </v-card-text>

      <v-card-actions>
        <v-btn color="green-darken-1" @click="router.back()">Volver</v-btn>
      </v-card-actions>
    </v-card>

    <!-- PEDIDOS DEL USUARIO -->
    <h3 class="text-h6 mb-3">Pedidos de {{ usuario?.nombre }}</h3>
    <div v-if="loadingPedidos">Cargando pedidos...</div>
    <v-alert v-else-if="pedidos.length === 0" tyoe="info">Este usuario no tiene pedidos.</v-alert>
    <v-row v-else>
      <v-col v-for="pedido in pedidos" :key="pedido.pedidoId" cols="12" sm="6" md="4">
        <v-card class="pa-3">
          <v-card-title>Pedido #{{ pedido.pedidoId }}</v-card-title>
          <v-card-subtitle>{{ new Date(pedido.fecha).toLocaleDateString() }}</v-card-subtitle>
          <v-card-text>
            <p><strong>Total:</strong> {{ pedido.precioTotal }}€</p>
            <p><strong>Estado:</strong> {{ pedido.estado }}</p>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import axios from 'axios'
import type { User } from '@/store/authen'
import type { Pedido } from '@/store/pedidoStore'

const route = useRoute()
const router = useRouter()
const usuario = ref<User | null>(null)
const pedidos = ref<Pedido[]>([])
const loading = ref(true)
const loadingPedidos = ref(true)
const error = ref('')
const cantSaldo = ref(0)
const añadiendo = ref(false)


onMounted(async () => {
  try {
    const id = Number(route.params.id)
    const response = await axios.get<User>(`http://localhost:7863/Usuarios/${id}`)
    usuario.value = response.data
    const resPedidos = await axios.get<Pedido[]>(`http://localhost:7863/Pedidos`, 
    { params: { usuarioId: id } })
    pedidos.value = resPedidos.data
  } catch (err) {
    error.value = 'No se pudo cargar el usuario.'
  } finally {
    loading.value = false
    loadingPedidos.value = false
  }
})

async function addSaldo() {
  if (!usuario.value || cantSaldo.value <= 0) return
  añadiendo.value = true
  try {
    await axios.post(`http://localhost:7863/Usuarios/${usuario.value.usuarioId}/saldo`, 
      Number(cantSaldo.value),
      { headers: { 'Content-Type': 'application/json' } }
    )
    usuario.value.saldo += Number(cantSaldo.value)
    cantSaldo.value = 0
  } catch (err) {
    error.value = 'No se pudo añadir saldo.'
  } finally {
    añadiendo.value = false
  }
}
</script>

<style scoped>
.error { color: red; font-weight: bold; }
</style>