<template>
  <v-container class="pt-6">
    <div class="d-flex align-center justify-space-between mb-4">
      <h2 class="text-h4">Catálogo de Productos</h2>
      <v-btn v-if="authen.isAdmin" color="green-darken-1" @click="openDialog">
        Añadir producto
      </v-btn>
    </div>

    <div class="d-flex gap-3 mb-4">
      <v-chip :color="filtroTipo === 'todos' ? 'green' : undefined" @click="filtroTipo='todos'">Todos</v-chip>
      <v-chip :color="filtroTipo === 'Te' ? 'green' : undefined" @click="filtroTipo='Te'">Tés</v-chip>
      <v-chip :color="filtroTipo === 'Comida' ? 'green' : undefined" @click="filtroTipo='Comida'">Comidas</v-chip>
    </div>

    <v-text-field
      v-model="searchTerm"
      label="Buscar producto"
      prepend-icon="mdi-magnify"
      class="mb-4"
      clearable
    />

    <v-alert v-if="filtrados.length === 0" type="info" class="mb-4">
      No se encontraron productos.
    </v-alert>

    <v-row>
      <v-col v-for="producto in filtrados" :key="producto.productoId" cols="12" sm="6" md="4">
        <v-card>
          <v-img
            :src="producto.imagenUrl || 'https://images.unsplash.com/photo-1556679343-c7306c1976bc?w=400'"
            height="200"
            cover
          />
          <v-card-title>{{ producto.nombre }}</v-card-title>
          <v-card-subtitle>{{ producto.origen }}</v-card-subtitle>
          <v-card-text>
            <p><strong>Precio:</strong> {{ producto.precio }}€/kg</p>
            <p><strong>Stock:</strong> {{ producto.stock }}g</p>
            <p><strong>Tipo:</strong> {{ producto.tipoProducto }}</p>
            <v-chip :color="producto.esOrganico ? 'green' : 'grey'" size="small" class="mt-1">
              {{ producto.esOrganico ? 'Orgánico' : 'No orgánico' }}
            </v-chip>
          </v-card-text>
          <v-card-actions>
            <v-btn variant="text" color="green-darken-1" @click="goDetail(producto.productoId)">
              Ver más
            </v-btn>
            <v-btn v-if="authen.isAdmin" variant="text" color="red" @click="eliminar(producto.productoId)">
              Eliminar
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>

    <v-dialog v-model="dialog" max-width="520">
      <v-card>
        <v-card-title>Nuevo producto</v-card-title>
        <v-card-text>
          <v-select :items="['Te', 'Comida']" label="Tipo" v-model="form.tipo" />
          <v-text-field label="Nombre" v-model="form.nombre" />
          <v-text-field label="Origen" v-model="form.origen" />
          <v-text-field label="Precio (€/kg)" type="number" v-model="form.precio" />
          <v-text-field label="Stock (g)" type="number" v-model="form.stock" />
          <v-checkbox label="¿Es orgánico?" v-model="form.esOrganico" />
          <v-text-field v-if="form.tipo === 'Te'" label="Tipo de hoja" v-model="form.tipoHoja" />
          <v-text-field v-if="form.tipo === 'Comida'" label="Tipo de comida" v-model="form.tipoComida" />
          <v-checkbox v-if="form.tipo === 'Comida'" label="¿Contiene gluten?" v-model="form.gluten" />
          <v-file-input
            label="Imagen del producto"
            accept="image/jpeg,image/png,image/gif"
            prepend-icon="mdi-camera"
            v-model="imagenFile"
            class="mt-2"
          />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn variant="text" @click="dialog=false">Cancelar</v-btn>
          <v-btn color="green-darken-1" @click="crearProducto" :loading="creando">Crear</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useProductoStore } from '@/store/productoStore'
import { useAuthenStore } from '@/store/authen'
import { useRouter } from 'vue-router'

const productoStore = useProductoStore()
const authen = useAuthenStore()
const router = useRouter()

const searchTerm  = ref('')
const filtroTipo  = ref('todos')
const imagenFile  = ref<File | null>(null)
const creando     = ref(false)

const filtrados = computed(() => {
  let lista = productoStore.productos
  if (filtroTipo.value !== 'todos') {
    lista = lista.filter(p => p.tipoProducto === filtroTipo.value)
  }
  return lista.filter(p =>
    p.nombre.toLowerCase().includes(searchTerm.value.toLowerCase())
  )
})

onMounted(() => { productoStore.fetchProductos() })

function goDetail(id: number) {
  router.push({ name: 'producto-detail', params: { id } })
}

async function eliminar(id: number) {
  if (confirm('¿Seguro que quieres eliminar este producto?')) {
    await productoStore.deleteProducto(id)
  }
}

const dialog = ref(false)
const form = ref({
  tipo:           'Te',
  nombre:         '',
  origen:         '',
  precio:         0,
  stock:          0,
  esOrganico:     false,
  tipoHoja:       '',
  tipoComida:     '',
  gluten:         false,
  fechaCaducidad: new Date().toISOString()
})

function openDialog() {
  imagenFile.value = null
  dialog.value = true
}

async function crearProducto() {
  if (!form.value.nombre || !form.value.origen) return
  creando.value = true
  try {
    await productoStore.addProducto({
      tipoProducto:   form.value.tipo,
      nombre:         form.value.nombre,
      origen:         form.value.origen,
      precio:         Number(form.value.precio),
      stock:          Number(form.value.stock),
      esOrganico:     form.value.esOrganico,
      tipoHoja:       form.value.tipoHoja,
      tipoComida:     form.value.tipoComida,
      gluten:         form.value.gluten,
      fechaCaducidad: form.value.fechaCaducidad,
      imagen:         imagenFile.value ?? undefined
    })
    dialog.value = false
  } finally {
    creando.value = false
  }
}
</script>