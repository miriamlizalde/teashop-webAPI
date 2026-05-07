<template>
    <v-container class="py-6">
      <div v-if="loading">Cargando pedido...</div>
      <v-card v-else-if="pedido" class="pa-4">
        <v-card-title class="text-h5">Pedido #{{ pedido.pedidoId }}</v-card-title>
        <v-card-text>
          <p><strong>Fecha:</strong> {{ new Date(pedido.fecha).toLocaleDateString() }}</p>
          <p><strong>Total:</strong> {{ pedido.precioTotal }}€</p>
          <p><strong>Usuario:</strong> {{ pedido.nombreUsuario }}</p>  
          <p><strong>Estado:</strong>
            <v-chip :color="colorEstado(pedido.estado)" size="small" class="ml-2">
              {{ pedido.estado }}
            </v-chip>
          </p>
  
          <div v-if="authen.isAdmin" class="mt-4">
            <v-select
              :items="['Pendiente', 'Enviado', 'Entregado', 'Cancelado']"
              label="Cambiar estado"
              v-model="nuevoEstado"
            />
            <v-btn color="green" @click="cambiarEstado">Guardar estado</v-btn>
          </div>
  
          <h3 class="mt-4 mb-2">Productos del pedido</h3>
          <v-table>
            <thead>
              <tr>
                <th>Producto ID</th>
                <th>Cantidad (g)</th>
                <th>Precio (€/kg)</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="item in pedido.items" :key="item.productoId">
                <td>{{ item.productoId }}</td>
                <td>{{ item.cantidad }}g</td>
                <td>{{ item.precio }}€/kg</td>
              </tr>
            </tbody>
          </v-table>
        </v-card-text>
        <v-card-actions>
          <v-btn @click="router.push({ name: 'pedidos' })">Volver</v-btn>
        </v-card-actions>
      </v-card>
    </v-container>
  </template>
  
  <script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import { useRoute, useRouter } from 'vue-router'
  import { usePedidoStore, type Pedido } from '@/store/pedidoStore'
  import { useAuthenStore } from '@/store/authen'
  
  const route = useRoute()
  const router = useRouter()
  const pedidoStore = usePedidoStore()
  const authen = useAuthenStore()
  
  const pedido = ref<Pedido | null>(null)
  const loading = ref(true)
  const nuevoEstado = ref('')
  
  onMounted(async () => {
    const id = Number(route.params.id)
    pedido.value = await pedidoStore.getPedido(id)
    nuevoEstado.value = pedido.value?.estado ?? ''
    loading.value = false
  })
  
  function colorEstado(estado: string) {
    if (estado === 'Pendiente') return 'orange'
    if (estado === 'Entregado') return 'green'
    if (estado === 'Cancelado') return 'red'
    return 'grey'
  }
  
  async function cambiarEstado() {
    if (!pedido.value) return
    await pedidoStore.updateEstado(pedido.value.pedidoId, nuevoEstado.value)
    pedido.value.estado = nuevoEstado.value
  }
  </script>