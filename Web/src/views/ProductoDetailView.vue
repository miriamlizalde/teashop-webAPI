<template>
  <v-container class="py-6" v-if="producto">
    <div class="d-flex align-center justify-space-between mb-4">
      <h2 class="text-h4">{{ producto.nombre }}</h2>
      <div v-if="authen.isAdmin">
        <v-btn color="error" variant="tonal" class="mr-2" @click="eliminar">Eliminar</v-btn>
        <v-btn color="green-darken-1" @click="guardar">Guardar cambios</v-btn>
      </div>
    </div>

    <v-row dense>
      <v-col cols="12" md="6">
        <v-img
          :src="producto.imagenUrl || 'https://images.unsplash.com/photo-1556679343-c7306c1976bc?w=400'"
          height="250"
          cover
          class="rounded mb-4"
        />

        <v-file-input
          v-if="authen.isAdmin"
          label="Cambiar imagen"
          accept="image/jpeg,image/png,image/gif"
          prepend-icon="mdi-camera"
          v-model="imagenFile"
          class="mb-2"
        />
        <v-text-field label="Nombre" v-model="form.nombre" :readonly="!authen.isAdmin" />
        <v-text-field label="Origen" v-model="form.origen" :readonly="!authen.isAdmin" />
        <v-text-field label="Precio (€/kg)" type="number" v-model="form.precio" :readonly="!authen.isAdmin" />
        <v-text-field label="Stock (g)" type="number" v-model="form.stock" :readonly="!authen.isAdmin" />
        <v-checkbox label="¿Es orgánico?" v-model="form.esOrganico" :readonly="!authen.isAdmin" />
        <v-text-field v-if="producto.tipoProducto === 'Te'"
          label="Tipo de hoja" v-model="form.tipoHoja" :readonly="!authen.isAdmin" />
        <v-text-field v-if="producto.tipoProducto === 'Comida'"
          label="Tipo de comida" v-model="form.tipoComida" :readonly="!authen.isAdmin" />
        <v-checkbox v-if="producto.tipoProducto === 'Comida'"
          label="¿Contiene gluten?" v-model="form.gluten" :readonly="!authen.isAdmin" />
      </v-col>
      <v-col cols="12" md="6">
        <v-chip :color="producto.esOrganico ? 'green' : 'grey'" class="mb-2">
          {{ producto.esOrganico ? 'Orgánico' : 'No orgánico' }}
        </v-chip>
        <p class="text-caption mt-2">
          Fecha caducidad: <strong>{{ new Date(producto.fechaCaducidad).toLocaleDateString() }}</strong>
        </p>
        <p class="text-caption">
          Tipo: <strong>{{ producto.tipoProducto }}</strong>
        </p>
      </v-col>
    </v-row>
    <v-btn class="mt-4" @click="router.push({ name: 'productos' })">Volver al catálogo</v-btn>
  </v-container>

  <v-container v-else class="py-6">
    <v-skeleton-loader type="image, article" />
  </v-container>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useProductoStore, type Producto } from '@/store/productoStore'
import { useAuthenStore } from '@/store/authen'

const route = useRoute()
const router = useRouter()
const store = useProductoStore()
const authen = useAuthenStore()

const producto = ref<Producto | undefined>()
const form = ref<Partial<Producto>>({})
const imagenFile = ref<File | null>(null)

onMounted(async () => {
  const id = Number(route.params.id)
  const found = await store.getProducto(id)
  producto.value = found
  if (found) {
    form.value = {
      tipoProducto:   found.tipoProducto,
      nombre:         found.nombre,
      origen:         found.origen,
      precio:         found.precio,
      stock:          found.stock,
      esOrganico:     found.esOrganico,
      fechaCaducidad: found.fechaCaducidad,
      tipoHoja:       found.tipoHoja,
      tipoComida:     found.tipoComida,
      gluten:         found.gluten
    }
  }
})

async function guardar() {
  if (!producto.value) return
  await store.updateProducto(producto.value.productoId, {
    tipoProducto:   form.value.tipoProducto   ?? producto.value.tipoProducto,
    nombre:         form.value.nombre         ?? producto.value.nombre,
    origen:         form.value.origen         ?? producto.value.origen,
    precio:         form.value.precio         ?? producto.value.precio,
    stock:          form.value.stock          ?? producto.value.stock,
    esOrganico:     form.value.esOrganico     ?? producto.value.esOrganico,
    fechaCaducidad: form.value.fechaCaducidad ?? producto.value.fechaCaducidad,
    tipoHoja:       form.value.tipoHoja,
    tipoComida:     form.value.tipoComida,
    gluten:         form.value.gluten,
    imagen:         imagenFile.value ?? undefined
  })
  router.push({ name: 'productos' })
}

async function eliminar() {
  if (!producto.value) return
  if (confirm('¿Seguro que quieres eliminar este producto?')) {
    await store.deleteProducto(producto.value.productoId)
    router.push({ name: 'productos' })
  }
}
</script>