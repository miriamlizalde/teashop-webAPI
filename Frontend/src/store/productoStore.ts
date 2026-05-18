import { defineStore } from 'pinia'
import axios from 'axios'

export interface Producto {
  productoId: number
  nombre: string
  origen: string
  precio: number
  stock: number
  esOrganico: boolean
  fechaCaducidad: string
  tipoProducto: string
  tipoHoja?: string
  tipoComida?: string
  gluten?: boolean
  imagenUrl?: string
}

const baseURL = 'http://localhost:7863'

function tipoProducto(p: any): string {
  console.log('producto:', p.nombre, '| tipoHoja:', p.tipoHoja, '| tipoComida:', p.tipoComida)
  if (p.tipoHoja !== undefined && p.tipoHoja !== null && p.tipoHoja !== '') return 'Te'
  if (p.tipoComida !== undefined && p.tipoComida !== null && p.tipoComida !== '') return 'Comida'
  return 'Te'
}

export const useProductoStore = defineStore('productoStore', {
  state: () => ({
    productos: [] as Producto[]
  }),

  getters: {
    productoCount: (state) => state.productos.length,
    porId: (state) => (id: number) => state.productos.find(p => p.productoId === id)
  },

  actions: {
    async fetchProductos() {
      const { data } = await axios.get<any[]>(`${baseURL}/Productos`)
      this.productos = data.map(p => ({
        ...p,
        tipoProducto: tipoProducto(p)
      }))
    },

    async getProducto(id: number) {
      const { data } = await axios.get<any>(`${baseURL}/Productos/${id}`)
      return {
        ...data,
        tipoProducto: tipoProducto(data)
      } as Producto
    },

    async addProducto(producto: Omit<Producto, 'productoId'> & { imagen?: File }) {
        const formData = new FormData()
        formData.append('tipo',           producto.tipoProducto)
        formData.append('nombre',         producto.nombre)
        formData.append('origen',         producto.origen)
        formData.append('precio',         String(producto.precio))
        formData.append('stock',          String(producto.stock))
        formData.append('esOrganico',     String(producto.esOrganico))
        formData.append('fechaCaducidad', producto.fechaCaducidad)
        if (producto.tipoHoja)   formData.append('tipoHoja',   producto.tipoHoja)
        if (producto.tipoComida) formData.append('tipoComida', producto.tipoComida)
        if (producto.gluten !== undefined) formData.append('gluten', String(producto.gluten))
        if (producto.imagen)     formData.append('imagen',     producto.imagen)
  
        await axios.post(`${baseURL}/Productos`, formData, {
          headers: { 'Content-Type': 'multipart/form-data' }
        })
        await this.fetchProductos()
      },
  
      async updateProducto(id: number, producto: Omit<Producto, 'productoId'> & { imagen?: File }) {
        const formData = new FormData()
        formData.append('tipo',           producto.tipoProducto)
        formData.append('nombre',         producto.nombre)
        formData.append('origen',         producto.origen)
        formData.append('precio',         String(producto.precio))
        formData.append('stock',          String(producto.stock))
        formData.append('esOrganico',     String(producto.esOrganico))
        formData.append('fechaCaducidad', producto.fechaCaducidad)
        if (producto.tipoHoja)   formData.append('tipoHoja',   producto.tipoHoja)
        if (producto.tipoComida) formData.append('tipoComida', producto.tipoComida)
        if (producto.gluten !== undefined) formData.append('gluten', String(producto.gluten))
        if (producto.imagen)     formData.append('imagen',     producto.imagen)
  
        await axios.put(`${baseURL}/Productos/${id}`, formData, {
          headers: { 'Content-Type': 'multipart/form-data' }
        })
        await this.fetchProductos()
      },

    async deleteProducto(id: number) {
      await axios.delete(`${baseURL}/Productos/${id}`)
      this.productos = this.productos.filter(p => p.productoId !== id)
    }
  }
})