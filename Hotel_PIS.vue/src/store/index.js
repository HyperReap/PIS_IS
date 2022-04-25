import { createStore } from 'vuex'

export default createStore({
  state: {
      reservationRooms: {
          rooms: []
      },
      reservationDetails: {}
  },
  mutations: {
      saveReservationRooms(state, rooms) {
          state.reservationRooms.rooms = rooms
      },
      saveReservationDetails(state, reservationDetails) {
          state.reservationDetails = reservationDetails
      }
  },
  actions: {
      saveReservationRooms(context, rooms) {
          context.commit('saveReservationRooms', rooms)
      },
      saveReservationDetails(context, reservationDetails) {
          context.commit('saveReservationDetails', reservationDetails)
      }
  },
  getters: {
      getReservationRooms: state => {
          return state.reservationRooms
      },
      getReservationDetails: state => {
          return state.reservationDetails
      },
  }
})
