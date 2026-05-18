<template>
  <v-container fluid class="pa-0">
    <div class="text-center py-8 px-4">
      <h1 class="text-h3 font-weight-bold">Bienvenido a TeaShop</h1>
      <p class="text-subtitle-1 mt-2">Descubre nuestra selección de tés y comidas.</p>
      <div class="mt-4">
        <v-btn color="green-darken-1" class="mr-3" @click="router.push('/productos')">
          Ver catálogo
        </v-btn>
        <v-btn color="green-darken-1" variant="outlined" @click="router.push('/login')" v-if="!authen.isAuthenticated">
          Iniciar sesión
        </v-btn>
      </div>
    </div>

    <div style="display:flex; align-items:center; padding: 0 16px; margin-bottom: 32px;">
      <v-btn icon="mdi-chevron-left" variant="text" size="large" @click="prev" />
      
      <div ref="track" style="display:flex; flex-direction:row; gap:16px; overflow-x:auto; flex:1; scrollbar-width:none;">
        <div v-for="(img, i) in images" :key="i" style="min-width:300px; flex-shrink:0;">
          <v-img :src="img.src" height="350" cover class="rounded-lg"></v-img>
        </div>
      </div>

      <v-btn icon="mdi-chevron-right" variant="text" size="large" @click="next" />
    </div>

    <v-container>
      <v-row class="mb-8">
        <v-col cols="12" md="4">
          <v-card class="pa-4 text-center">
            <v-icon size="48" color="green">mdi-leaf</v-icon>
            <h3 class="mt-2">Tés de calidad</h3>
            <p class="text-body-2 mt-1">Selección de tés orgánicos de todo el mundo</p>
          </v-card>
        </v-col>
        <v-col cols="12" md="4">
          <v-card class="pa-4 text-center">
            <v-icon size="48" color="green">mdi-food</v-icon>
            <h3 class="mt-2">Repostería artesanal</h3>
            <p class="text-body-2 mt-1">Dulces y salados para acompañar tu té</p>
          </v-card>
        </v-col>
        <v-col cols="12" md="4">
          <v-card class="pa-4 text-center">
            <v-icon size="48" color="green">mdi-cart</v-icon>
            <h3 class="mt-2">Pedidos online</h3>
            <p class="text-body-2 mt-1">Realiza tu pedido y recíbelo en casa</p>
          </v-card>
        </v-col>
      </v-row>
    </v-container>
  </v-container>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthenStore } from '@/store/authen'

const router = useRouter()
const authen = useAuthenStore()
const track = ref<HTMLDivElement | null>(null)

const images = [
  {src: 'https://res.cloudinary.com/depr9z7dy/image/upload/v1775758033/pexels-exnl-2227126_hrzwrh.jpg'},
  {src: 'https://res.cloudinary.com/depr9z7dy/image/upload/v1775757987/tes_r7dmif.png'},
  {src: 'https://res.cloudinary.com/depr9z7dy/image/upload/v1775757350/ginger-tea-te-de-jengibre-ClaraGon4565_psfrbh.jpg'},
  {src: 'https://res.cloudinary.com/depr9z7dy/image/upload/v1775757989/te3_hrrvm9.jpg'},
  {scr: 'https://res.cloudinary.com/depr9z7dy/image/upload/v1775757345/beneficios-del-te-verde_17350_x4prae.jpg'},
  {src: 'https://res.cloudinary.com/depr9z7dy/image/upload/v1775759421/sanw_c8c3z6_a_-90_nkpacn.jpg'},
  {src: 'https://res.cloudinary.com/depr9z7dy/image/upload/v1775757470/Fish-Tacos-1337495_mw5wx8.jpg'},
  {src: 'https://res.cloudinary.com/depr9z7dy/image/upload/v1775757989/tecomida_er3ao8.jpg'},
  {src: 'https://res.cloudinary.com/depr9z7dy/image/upload/v1775665873/cld-sample-4.jpg'},
]
function prev() {
  if (track.value) track.value.scrollBy({ left: -320, behavior: 'smooth' })
}
function next() {
  if (track.value) track.value.scrollBy({ left: 320, behavior: 'smooth' })
}
</script>