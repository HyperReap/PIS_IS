import { createStore } from 'vuex'

export default createStore({
  state: {
      reservationRooms: {
          rooms: []
      },
      reservationDetails: [],
      customerEmail: null
  },
  mutations: {
      saveReservationRooms(state, rooms) {
          state.reservationRooms.rooms = rooms
      },
      saveReservationDetails(state, reservationDetails) {
          state.reservationDetails.push(reservationDetails);
      },
      saveCustomerEmail(state, email) {
          state.customerEmail = email
      }
  },
  actions: {
      saveReservationRooms(context, rooms) {
          context.commit('saveReservationRooms', rooms)
      },
      saveReservationDetails(context, reservationDetails) {
          context.commit('saveReservationDetails', reservationDetails)
      },
      saveCustomerEmail(context, email) {
          context.commit('saveCustomerEmail', email)
      }
  },
  getters: {
      getReservationRooms: state => {
          return state.reservationRooms
      },
      getReservationDetails: state => {
          return state.reservationDetails
      },
      getCustomerEmail: state => {
          return state.customerEmail
      },
  }
})
