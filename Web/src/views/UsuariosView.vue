<template>
  <v-container class="pt-6">
    <div class="d-flex align-center justify-space-between mb-4">
      <h2 class="text-h4 mb-4">Listado de usuarios</h2>
      <v-btn color="green-darken-1" @click="openDialog">Añadir usuario</v-btn>
    </div>

    <div v-if="loading">Cargando usuarios...</div>
    <div v-else-if="error" class="error">{{ error }}</div>
    <v-row v-else>
      <v-col v-for="usuario in usuarios" :key="usuario.usuarioId" cols="12" sm="6" md="4">
        <v-card class="pa-3">
          <v-card-title>{{ usuario.nombre }}</v-card-title>
          <v-card-subtitle>{{ usuario.email }}</v-card-subtitle>
          <v-card-text>
            <p><strong>Rol:</strong> {{ usuario.rol }}</p>
            <p><strong>Saldo:</strong> {{ usuario.saldo }}€</p>
            <v-chip :color="usuario.esSocio ? 'green' : 'grey'" size="small">
              {{ usuario.esSocio ? 'Socio' : 'No socio' }}
            </v-chip>
          </v-card-text>
          <v-card-actions>
            <v-btn variant="text" color="green"
              @click="router.push({ name: 'usuarioDetail', params: { id: usuario.usuarioId } })">
              Ver detalle
            </v-btn>
            <v-btn variant="text" color="red" @click="eliminar(usuario.usuarioId)">
              Eliminar
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>

    <v-dialog v-model="dialog" max-width="480">
      <v-card>
        <v-card-title>Nuevo usuario</v-card-title>
        <v-card-text>
          <v-text-field label="Nombre" v-model="form.nombre" />
          <v-text-field label="Email" v-model="form.email" />
          <v-text-field label="Contraseña" type="password" v-model="form.password" />
          <v-select :items="['usuario', 'admin']" label="Rol" v-model="form.rol" />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn variant="text" @click="dialog=false">Cancelar</v-btn>
          <v-btn color="green-darken-1" @click="crearUsuario" :loading="creando">Crear</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import axios from 'axios'
import type { User } from '@/store/authen'

const router = useRouter()
const usuarios = ref<User[]>([])
const loading = ref(true)
const error = ref('')
const dialog = ref(false)
const creando = ref(false)
const form = ref({
  nombre: '',
  email: '',
  password: '',
  rol: 'usuario',
})

onMounted(async () => {
  try {
    const response = await axios.get<User[]>('http://localhost:7863/Usuarios')
    usuarios.value = response.data
  } catch (err) {
    error.value = 'Error al cargar los usuarios.'
  } finally {
    loading.value = false
  }
})

function openDialog() {
  form.value = { nombre: '', email: '', password: '', rol: 'usuario' }
  dialog.value = true
}

async function crearUsuario() {
  if (!form.value.nombre || !form.value.email || !form.value.password) return
  creando.value = true
  try {
    await axios.post('http://localhost:7863/Auth/Register', form.value)
    const response = await axios.get<User[]>('http://localhost:7863/Usuarios')
    usuarios.value = response.data
    dialog.value = false
  } catch (err) {
    error.value = 'Error al crear el usuario.'
  } finally {
    creando.value = false
  }
}

async function eliminar(id: number) {
  if (confirm('¿Seguro que quieres eliminar este usuario?')) {
    await axios.delete(`http://localhost:7863/Usuarios/${id}`)
    usuarios.value = usuarios.value.filter(u => u.usuarioId !== id)
  }
}
</script>

<style scoped>
.error { color: red; font-weight: bold; }
</style>