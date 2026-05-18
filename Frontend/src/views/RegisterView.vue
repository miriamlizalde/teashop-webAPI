<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthenStore } from '@/store/authen'

const nombre = ref('')
const email = ref('')
const password = ref('')
const esSocio = ref(false)
const error = ref('')
const loading = ref(false)
const router = useRouter()
const authen = useAuthenStore()

const handleRegister = async () => {
  error.value = ''
  loading.value = true
  try {
    await authen.register(nombre.value, email.value, password.value, esSocio.value)
    router.push({ name: 'home' })
  } catch (err) {
    error.value = 'Error al registrar usuario'
    console.error('Registro fallido:', err)
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <v-container class="fill-height" fluid>
    <v-row align="center" justify="center">
      <v-col cols="12" sm="8" md="4">
        <v-card class="pa-4">
          <v-card-title class="text-h5 text-center">Registrar usuario</v-card-title>
          <v-card-text>
            <v-text-field label="Nombre" v-model="nombre" />
            <v-text-field label="Email" type="email" v-model="email" />
            <v-text-field label="Contraseña" type="password" v-model="password" />
            <v-checkbox label="¿Eres socio?" v-model="esSocio" />
            <v-btn :loading="loading" color="primary" block @click="handleRegister">
              Registrarse
            </v-btn>
            <v-alert v-if="error" type="error" class="mt-3">{{ error }}</v-alert>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>