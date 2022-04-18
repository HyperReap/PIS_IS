import { createStore } from 'vuex'

export default createStore({
  state: {
      reservationDetails: {
          rooms: Array,
          checkIn: null,
          checkOut: null
      },
  },
  mutations: {
      saveReservationRooms(state, rooms) {
          state.reservationDetails.rooms = rooms
      },
  },
  actions: {
      saveReservationRooms(context, rooms) {
          context.commit('saveReservationRooms', rooms)
      },
  },
  getters: {
  }
})
