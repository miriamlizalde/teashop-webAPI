<template>
    <v-container class="pt-6">
      <div class="d-flex align-center justify-space-between mb-4">
        <h2 class="text-h4">Pedidos</h2>
        <v-btn color="green" @click="openDialog">Nuevo pedido</v-btn>
      </div>
  
      <div v-if="loading">Cargando pedidos...</div>
  
      <v-alert v-else-if="pedidoStore.pedidos.length === 0" type="info">
        No hay pedidos todavía.
      </v-alert>
  
      <v-row v-else>
        <v-col v-for="pedido in pedidoStore.pedidos" :key="pedido.pedidoId" cols="12" sm="6" md="4">
          <v-card class="pa-3">
            <v-card-title>Pedido #{{ pedido.pedidoId }}</v-card-title>
            <v-card-subtitle>{{ new Date(pedido.fecha).toLocaleDateString() }}</v-card-subtitle>
            <v-card-text>
              <p><strong>Usuario:</strong> {{ pedido.nombreUsuario }}</p>
              <p><strong>Total:</strong> {{ pedido.precioTotal }}€</p>
              <v-chip :color="colorEstado(pedido.estado)" size="small">
                {{ pedido.estado }}
              </v-chip>
            </v-card-text>
            <v-card-actions>
              <v-btn variant="text" color="green"
                @click="router.push({ name: 'pedidoDetail', params: { id: pedido.pedidoId } })">
                Ver detalle
              </v-btn>
              <v-btn v-if="authen.isAdmin && pedido.estado === 'Pendiente'"
                variant="text" color="red" @click="eliminar(pedido.pedidoId)">
                Eliminar
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-col>
      </v-row>
  
      <v-dialog v-model="dialog" max-width="600">
        <v-card>
          <v-card-title>Nuevo pedido</v-card-title>
          <v-card-text>
            <v-select 
              v-if="authen.isAdmin"
              :items="usuarios" 
              item-title="nombre" 
              item-value="usuarioId" 
              label="Para usuario" 
              v-model="usuarioSelect"
              clearable
              class="mb-3"
            />
            <div v-for="(item, index) in items" :key="index" class="d-flex gap-3 mb-2">
              <v-select
                :items="productoStore.productos"
                item-title="nombre"
                item-value="productoId"
                label="Producto"
                v-model="item.productoId"
                class="flex-grow-1"
              />
              <v-text-field
                label="Cantidad (g)"
                type="number"
                v-model="item.cantidad"
                style="max-width: 120px"
              />
              <v-btn icon="mdi-delete" variant="text" color="red" @click="items.splice(index, 1)" />
            </div>
            <v-btn variant="text" color="green" @click="items.push({ productoId: 0, cantidad: 0 })">
              + Añadir producto
            </v-btn>
          </v-card-text>
          <v-card-actions>
            <v-spacer />
            <v-btn variant="text" @click="dialog=false">Cancelar</v-btn>
            <v-btn color="green" @click="crearPedido">Crear pedido</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </v-container>
  </template>
  
  <script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import { useRouter } from 'vue-router'
  import axios from 'axios'
  import { usePedidoStore } from '@/store/pedidoStore'
  import { useProductoStore } from '@/store/productoStore'
  import { useAuthenStore } from '@/store/authen'
  import type { User } from '@/store/authen'
  
  const router = useRouter()
  const pedidoStore = usePedidoStore()
  const productoStore = useProductoStore()
  const authen = useAuthenStore()
  const loading = ref(true)
  const usuarios = ref<User[]>([])
  const usuarioSelect = ref<number | null>(null)
  
  onMounted(async () => {
    await productoStore.fetchProductos()
    await pedidoStore.fetchPedidos()
    if (authen.isAdmin) {
      try {
        const res = await axios.get<User[]>('http://localhost:7863/Usuarios')
        usuarios.value = res.data
      } catch (err) {
        console.error('Error al cargar usuarios:', err)
      }
    }
    loading.value = false
  })
  
  function colorEstado(estado: string) {
    if (estado === 'Pendiente') return 'orange'
    if (estado === 'Entregado') return 'green'
    if (estado === 'Cancelado') return 'red'
    return 'grey'
  }
  
  async function eliminar(id: number) {
    if (confirm('¿Eliminar este pedido?')) {
      await pedidoStore.deletePedido(id)
    }
  }
  
  const dialog = ref(false)
  const items = ref([{ productoId: 0, cantidad: 0 }])
  
  function openDialog() {
    items.value = [{ productoId: 0, cantidad: 0 }]
    dialog.value = true
  }
  
  async function crearPedido() {
    const itemsValidos = items.value.filter(i => i.productoId > 0 && i.cantidad > 0)
    if (itemsValidos.length === 0) return
    await pedidoStore.addPedido(itemsValidos)
    dialog.value = false
  }
  </script>