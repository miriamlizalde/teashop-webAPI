<script setup lang="ts">
import Header from './components/Header.vue'
import Footer from './components/Footer.vue'
import { RouterLink } from 'vue-router';
import { useAuthenStore } from '@/store/authen';

const authen = useAuthenStore()
authen.fetchUser()
</script>

<template>
  <v-app>
    <Header />
    <nav>
      <RouterLink to="/">Home</RouterLink>
    </nav>
    <v-main>
      <v-container>
        <h1>TeaShop.</h1>
        <router-view />
      </v-container>
    </v-main>
    <Footer />
  </v-app>
</template>

<style scoped>
.logo {
  height: 6em;
  padding: 1.5em;
  will-change: filter;
  transition: filter 300ms;
}
.logo:hover {
  filter: drop-shadow(0 0 2em #646cffaa);
}
.logo.vue:hover {
  filter: drop-shadow(0 0 2em #42b883aa);
}
</style>

