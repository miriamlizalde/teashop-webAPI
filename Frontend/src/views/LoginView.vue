<template>
  <div class="login-page">
    <h1>Iniciar sesión</h1>

    <p v-if="$route.name === 'unauthorized'" class="error">
      Acceso denegado.
    </p>
    <form @submit.prevent="handleLogin">
      <v-text-field
      v-model.trim="email"
      label="Correo electrónico"
      type="email"
      required
      outlined
      class="mb-4"
      />

      <v-text-field
      v-model="password"
      label="Contraseña"
      type="password"
      required
      outlined
      class="mb-4"
      />
      <v-btn type="submit" color="primary" :loading="loading">
        Iniciar sesión
      </v-btn>
      <p v-if="error" class="error">{{ error }}</p>
      <div class="text-center mt-4">
        <span>Si aún no tienes cuenta:</span>
        <RouterLink to="/register">Regístrate</RouterLink>
      </div>

    </form>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthenStore } from '@/store/authen';

const email = ref('')
const password = ref('')
const error = ref('')
const loading = ref(false)
const authen = useAuthenStore()
const router = useRouter()

const handleLogin = async() => {
  error.value = ''
  loading.value = true
  try {
    await authen.login(email.value, password.value)
    router.push({ name: 'home' })
  } catch (err) {
    error.value = 'El email o la contraseña son incorrectos'
    console.log('No se ha podido iniciar seesión:', error)
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.error {
  color: red;
  font-weight: bold;
}
</style>
