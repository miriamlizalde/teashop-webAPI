import { defineStore } from 'pinia'
import axios from 'axios'

export interface User {
    usuarioId: number
    nombre: string
    email: string
    rol: 'usuario' | 'admin'
    saldo: number
    esSocio: boolean
  }
  
export const useAuthenStore = defineStore('authen', {
  state: () => ({
    user: null as User | null,
    token: null as string | null
  }),

  getters: {
    isAuthenticated: (state) => !!state.token,
    isAdmin: (state) => state.user?.rol === 'admin'
  },

  actions: {
    async register(nombre: string, email: string, password: string, esSocio: boolean) {
      const response = await axios.post('http://localhost:7863/Auth/Register', {
        nombre,
        email,
        password,
        esSocio
      })
    
      this.token = response.data
      if (this.token) {
        localStorage.setItem('token', this.token)
        axios.defaults.headers.common['Authorization'] = `Bearer ${this.token}`
        const userResponse = await axios.get('http://localhost:7863/Usuarios/me')
        this.user = userResponse.data
      }
    },
    
    async login(email: string, password: string) {
      try {
        const response = await axios.post('http://localhost:7863/Auth/login', {
          email,
          password
        })

        this.token = response.data

        if (this.token){
            localStorage.setItem('token', this.token)
            axios.defaults.headers.common['Authorization'] = `Bearer ${this.token}`
            const userResponse = await axios.get('http://localhost:7863/Usuarios/me')
            this.user = userResponse.data
        }
        } catch (error) {
            console.error('Error al iniciar sesión:', error)
        throw error
      }
    },

    logout() {
      this.token = null
      this.user = null
      localStorage.removeItem('token')
      delete axios.defaults.headers.common['Authorization']
    },

    async fetchUser() {
      const token = localStorage.getItem('token')
      if (token) {
        this.token = token
        axios.defaults.headers.common['Authorization'] = `Bearer ${token}`
        try {
          const response = await axios.get('http://localhost:7863/Usuarios/me')
          this.user = response.data
        } catch (error) {
          console.error('Error al obtener el usuario:', error)
          this.logout()
        }
      }
    }
  }
})
