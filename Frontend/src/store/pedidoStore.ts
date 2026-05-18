import { defineStore } from 'pinia'
import axios from 'axios'

export interface ItemPedido {
  productoId: number
  cantidad: number
  precio?: number
}

export interface Pedido {
  pedidoId: number
  usuarioId: number
  nombreUsuario: string
  fecha: string
  precioTotal: number
  estado: string
  items: ItemPedido[]
}

const baseURL = 'http://localhost:7863'

export const usePedidoStore = defineStore('pedidoStore', {
  state: () => ({
    pedidos: [] as Pedido[]
  }),

  actions: {
    async fetchPedidos() {
      const { data } = await axios.get<Pedido[]>(`${baseURL}/Pedidos`)
      this.pedidos = data
    },

    async getPedido(id: number) {
      const { data } = await axios.get<Pedido>(`${baseURL}/Pedidos/${id}`)
      return data
    },

    async addPedido(items: ItemPedido[], usuarioId?: number) {
      const body: any = { items }
      if (usuarioId) body.usuarioId = usuarioId
      await axios.post(`${baseURL}/Pedidos`, body)
      await this.fetchPedidos()
    },

    async updateEstado(id: number, nuevoEstado: string) {
      await axios.patch(`${baseURL}/Pedidos/${id}/estado`, `"${nuevoEstado}"`, {
        headers: { 'Content-Type': 'application/json' }
      })
      await this.fetchPedidos()
    },

    async deletePedido(id: number) {
      await axios.delete(`${baseURL}/Pedidos/${id}`)
      this.pedidos = this.pedidos.filter(p => p.pedidoId !== id)
    }
  }
})