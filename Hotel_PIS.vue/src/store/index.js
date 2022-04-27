import { createStore } from 'vuex'

export default createStore({
  state: {
      reservationRooms: {
          rooms: []
      },
      reservationDetails: [],
      customerEmail: null,
      loggedUser: {
          email: null,
          firstName: null,
          secondName: null,
          token: null
      }
  },
  mutations: {
      saveReservationRooms(state, rooms) {
          state.reservationRooms.rooms = rooms
      },
      saveReservationDetails(state, reservationDetails) {
          state.reservationDetails.push(reservationDetails);
      },
      clearReservationsDetails(state, data) {
          state.reservationDetails = data;
      },
      saveCustomerEmail(state, email) {
          state.customerEmail = email
      },
      setLoggedUser(state, user) {
          state.loggedUser = user
      }
  },
  actions: {
      saveReservationRooms(context, rooms) {
          context.commit('saveReservationRooms', rooms)
      },
      saveReservationDetails(context, reservationDetails) {
          context.commit('saveReservationDetails', reservationDetails);
      },
      clearReservationsDetails(context, data) {
          context.commit('clearReservationsDetails', data);
      },
      saveCustomerEmail(context, email) {
          context.commit('saveCustomerEmail', email)
      },
      setLoggedUser(context, user) {
          context.commit('setLoggedUser', user)
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
      getLoggedUser: state => {
          return state.loggedUser
      },
      isAuthenticated: state => {
          return !!state.loggedUser.email
      },
      getLoggedUserToken: state => {
          return state.loggedUser.token
      }
  }
})
